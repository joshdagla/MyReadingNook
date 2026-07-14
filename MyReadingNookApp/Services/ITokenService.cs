namespace MyReadingNookApp.Services;

public class TokenResult
{
    public string Token { get; set; }
    public DateTimeOffset ExpiresAt { get; set; }
}

public interface ITokenService
{
    TokenResult GenerateToken(int userId, string username);
}