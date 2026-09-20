namespace MyReadingNookApp.Dtos.Friends;

// Dto used to send the friend invite input
public class SendFriendInviteDto
{
    public int AddresseeId { get; set; }

    public string? Message { get; set; } = string.Empty;
}

public class AcceptInviteDto
{
    public int RequesterId { get; set; }

    public string InviteStatus { get; set; } = string.Empty;
}

public class FriendInviteResponseDto
{
    public int RequesterId { get; set; }

    public string? Message { get; set; } = string.Empty;
}

public class AllFriendInviteDto
{
    public int RequesterId { get; set; }

    public int AddresseeId { get; set; }

    public string InviteStatus { get; set; } = string.Empty;

    public DateTimeOffset SentAt { get; set; }
}

public class GetMyFriendsResponseDto
{
    public int FriendId { get; set; }

    public string Username { get; set; } = string.Empty;

    public DateTimeOffset FriendsSince { get; set; }
}

