using Microsoft.EntityFrameworkCore;
using MyReadingNookApp.Dtos;
using MyReadingNookApp.Dtos.User;
using MyReadingNookApp.Models;

namespace MyReadingNookApp.Services;

public class AuthService : IAuthService
{
    private readonly NookContext _context;
    private readonly ITokenService _tokenService;

    public AuthService(NookContext context, ITokenService tokenService)
    {
        this._context = context;
        this._tokenService = tokenService;
    }

    // RegisterAsync method to hash user password 
        public async Task<ServiceResult<AuthResponseDto>> RegisterAsync(RegisterRequestDto registerRequest)
        {
           bool emailExists = await _context.Users.AnyAsync(u => u.Email == registerRequest.Email);

           // Service sends error if email already exists
           if (emailExists)
           {
               return new ServiceResult<AuthResponseDto>
               {
                   Success = false,
                   ErrorMessage = "User does not exist"
               };
           }
           // New user is created with success message if email does not exist and all other checks pass
           {
               string hashPassword = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password);

               UserModel newUserModel = new UserModel
               {
                   Username = registerRequest.Username,
                   Email = registerRequest.Email,
                   PasswordHash = hashPassword,
               };

               // User data is added and saved to DB
               _context.Users.Add(newUserModel);
               
               await _context.SaveChangesAsync();
               
               // Token is generated for user
               var tokenResult = _tokenService.GenerateToken(newUserModel.Id, newUserModel.Username);
               
               // User is given success on registration, given a token for auto-login
               return new ServiceResult<AuthResponseDto>
               {
                   Success = true,
                   Data = new AuthResponseDto
                   {
                       ExpiresAt =  tokenResult.ExpiresAt,
                       Token = tokenResult.Token,
                       UserInfo = new UserInfoDto
                       {
                           Id = newUserModel.Id,
                           Email = registerRequest.Email,
                           Username = registerRequest.Username,
                       }
                   }
               };
           }
        }

        public async Task<ServiceResult<AuthResponseDto>> LoginAsync(LoginRequestDto loginRequest)
        {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginRequest.Email);
                
            // User does not exist
            if (user == null)
            {
                return new ServiceResult<AuthResponseDto>
                {
                    Success = false,
                    ErrorMessage = "Invalid credentials."
                };
            }
            // User does exist
            {
                bool verifyPassword = BCrypt.Net.BCrypt.Verify( loginRequest.Password, user.PasswordHash);

                // Login credential are correct
                if (verifyPassword)
                {
                    var tokenResult = _tokenService.GenerateToken(user.Id, user.Username);
                    
                    return new ServiceResult<AuthResponseDto>
                    {
                        Success = true,
                        Data = new AuthResponseDto
                        {
                            ExpiresAt = tokenResult.ExpiresAt,
                            Token = tokenResult.Token,
                            UserInfo = new UserInfoDto
                            {
                                Id = user.Id,
                                Email = user.Email,
                                Username = user.Username,
                            }
                        }
                    };
                }
            }
            // Login credentials incorrect
            {
                return new ServiceResult<AuthResponseDto>
                {
                    Success = false,
                    ErrorMessage = "Invalid credentials."
                };
            }
        }
}