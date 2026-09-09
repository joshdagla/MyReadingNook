namespace MyReadingNookApp.Dtos.Friends;

// Dto used to send the friend invite input
public class SendFriendInviteDto
{
    public int AddresseeId { get; set; }

    public string? Message { get; set; } = string.Empty;
}

public class AcceptInviteDto
{
    public int AddresseeId { get; set; }

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

