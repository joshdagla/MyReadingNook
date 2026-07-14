using MyReadingNookApp.Dtos.User;

namespace MyReadingNookApp.Dtos;

public class AuthResponseDto
{
    public string Token { get; set; }

    public DateTimeOffset ExpiresAt { get; set; }

    public UserInfoDto UserInfo { get; set; }
}