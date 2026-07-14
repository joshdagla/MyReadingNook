using MyReadingNookApp.Dtos;

namespace MyReadingNookApp.Services;

// TODO: Move ServiceResult wrapper class to a separate directory, as it might be used in other places
public class ServiceResult<T>
{
   public bool Success { get; set; }
   public string? ErrorMessage { get; set; }
   public T? Data { get; set; }
}

// Auth service to send data on user input during login and signup
public interface IAuthService
{
   Task<ServiceResult<AuthResponseDto>> RegisterAsync(RegisterRequestDto registerRequest);
   
   Task<ServiceResult<AuthResponseDto>> LoginAsync(LoginRequestDto loginRequest);
}