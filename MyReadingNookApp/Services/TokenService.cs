using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace MyReadingNookApp.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;
    
    public TokenService(IConfiguration config)
    {
        _config = config;
    }
    
    public TokenResult GenerateToken(int userId, string username)
    {
        // Token generation prcoess (different parts of a token)
        var tokenClaims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Name, username),
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiresAt = DateTimeOffset.UtcNow.AddMinutes(60);
        
        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: tokenClaims,
            expires: expiresAt.UtcDateTime,
            signingCredentials: credentials
            );

        return new TokenResult
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiresAt = expiresAt,
        };
    }
}