namespace MyReadingNookApp.Dtos.Friends;

public class UserSearchResponseDto
{
    public int Id { get; set; }
    
    public string Username  { get; set; } = string.Empty;

    public string ProfileVisibility { get; set; } = string.Empty;
}