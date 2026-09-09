using MyReadingNookApp.Dtos.Friends;

namespace MyReadingNookApp.Services;

public interface IFriendInviteService
{
    Task<ServiceResult<SendFriendInviteDto>> SendFriendInviteAsync(
        SendFriendInviteDto dto,
        int requesterId
    );

    Task<ServiceResult<FriendInviteResponseDto>> RespondToInviteAsync(
        AcceptInviteDto dto,
        int RequesterId
    );

    Task<ServiceResult<List<AllFriendInviteDto>>> GetSentInvitesAsync(int userId);

    Task<ServiceResult<List<AllFriendInviteDto>>> GetReceivedInvitesAsync(int userId);
}

