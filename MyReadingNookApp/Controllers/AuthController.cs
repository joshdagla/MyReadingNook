using Microsoft.AspNetCore.Mvc;
using MyReadingNookApp.Dtos;
using MyReadingNookApp.Services;

namespace MyReadingNookApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    
    public AuthController(IAuthService authService)
    {
        this._authService = authService;
    }

    // RegisterUser: Handles user registration, receives data from user and returns success or failure message
    // Also gives user a token for login
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterRequestDto registerRequest)
    {
        try
        {
            var result = await _authService.RegisterAsync(registerRequest);

            if (!result.Success) return Conflict(result);
            return Created("User created successfully", result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
    {
        try
        {
            var result = await _authService.LoginAsync(loginRequest);
        
            if (!result.Success) return Unauthorized(result);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }
}