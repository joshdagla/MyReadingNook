using Microsoft.EntityFrameworkCore;
using MyReadingNookApp.Dtos.Friends;
using MyReadingNookApp.Models;

namespace MyReadingNookApp.Services;

public class FriendInviteService : IFriendInviteService
{
    private readonly NookContext _nookContext;

    public FriendInviteService(NookContext nookContext)
    {
        _nookContext = nookContext;
    }

    // Service to send a friend request
    public async Task<ServiceResult<SendFriendInviteDto>> SendFriendInviteAsync(
        SendFriendInviteDto dto,
        int RequesterId
    )
    {
        // User who sends the invite
        bool requesterCheck = await _nookContext.Users.Where(u => u.Id == RequesterId).AnyAsync();

        // User who recieves the request
        bool addresseeCheck = await _nookContext
            .Users.Where(u => u.Id == dto.AddresseeId)
            .AnyAsync();

        if (requesterCheck == false)
        {
            return new ServiceResult<SendFriendInviteDto>
            {
                Success = false,
                ErrorMessage = "addressee not found",
            };
        }

        if (addresseeCheck == false)
        {
            return new ServiceResult<SendFriendInviteDto>
            {
                Success = false,
                ErrorMessage = "addressee not found",
            };
        }

        // Checks if both RequesterId and AddresseeId are same. Essentially checks for self friend requests
        if (RequesterId == dto.AddresseeId)
        {
            return new ServiceResult<SendFriendInviteDto>
            {
                Success = false,
                ErrorMessage = "You cannot send a friend request to yourself",
            };
        }

        // Checks if a friend invite has been sent before
        var friendInvite = await _nookContext
            .Friends.Where(f => f.RequesterId == RequesterId && f.AddresseeId == dto.AddresseeId)
            .FirstOrDefaultAsync();

        // Friend invite hasnt been sent before, new friend invite is created
        if (friendInvite == null)
        {
            var newFriendInvite = new FriendsModel
            {
                RequesterId = RequesterId,
                AddresseeId = dto.AddresseeId,
            };

            await _nookContext.AddAsync(newFriendInvite);

            await _nookContext.SaveChangesAsync();

            return new ServiceResult<SendFriendInviteDto>
            {
                Success = true,
                Data = new SendFriendInviteDto
                {
                    AddresseeId = newFriendInvite.AddresseeId,
                    Message = "Friend request sent successfully",
                },
            };
        }
        else
        {
            // If Invite has already been accepted and both users are friends
            if (friendInvite.Status == "Accepted")
            {
                return new ServiceResult<SendFriendInviteDto>
                {
                    Success = false,
                    ErrorMessage = "You are already friends with this person",
                };
            }

            // Any other status (Pending or Declined) — check cooldown
            TimeSpan difference = DateTimeOffset.UtcNow - friendInvite.CreatedAt;

            if (difference.TotalDays < 2)
            {
                return new ServiceResult<SendFriendInviteDto>
                {
                    Success = false,
                    ErrorMessage = "New friend requests can only be sent after 2 days are complete",
                };
            }
            // Deletes the rejected friend request and sends a new one
            else
            {
                var newFriendInvite = new FriendsModel
                {
                    RequesterId = RequesterId,
                    AddresseeId = dto.AddresseeId,
                };

                _nookContext.Friends.Remove(friendInvite);
                await _nookContext.AddAsync(newFriendInvite);
                await _nookContext.SaveChangesAsync();

                return new ServiceResult<SendFriendInviteDto>
                {
                    Success = true,
                    Data = new SendFriendInviteDto { AddresseeId = newFriendInvite.AddresseeId },
                };
            }
        }
    }

    // Service to respond to a friend request
    public async Task<ServiceResult<FriendInviteResponseDto>> RespondToInviteAsync(
        AcceptInviteDto dto,
        int RequesterId
    )
    {
        // User who sends the invite
        bool requesterCheck = await _nookContext.Users.Where(u => u.Id == RequesterId).AnyAsync();

        // User who recieves the request
        bool addresseeCheck = await _nookContext
            .Users.Where(u => u.Id == dto.AddresseeId)
            .AnyAsync();

        if (requesterCheck == false)
        {
            return new ServiceResult<FriendInviteResponseDto>
            {
                Success = false,
                ErrorMessage = "requester not found",
            };
        }

        if (addresseeCheck == false)
        {
            return new ServiceResult<FriendInviteResponseDto>
            {
                Success = false,
                ErrorMessage = "addressee not found",
            };
        }

        // Fetching the invite
        var friendInvite = await _nookContext
            .Friends.Where(f => f.RequesterId == RequesterId && f.AddresseeId == dto.AddresseeId)
            .FirstOrDefaultAsync();

        if (friendInvite == null)
        {
            return new ServiceResult<FriendInviteResponseDto>
            {
                Success = false,
                ErrorMessage = "Invite doesnt exist",
            };
        }

        if (friendInvite.Status == "Accepted")
        {
            return new ServiceResult<FriendInviteResponseDto>
            {
                Success = false,
                ErrorMessage = "You are already friends with this person",
            };
        }

        // If an invite with the rejected status is found, automatically delete it.
        if (friendInvite.Status == "Rejected")
        {
            _nookContext.Friends.Remove(friendInvite);

            await _nookContext.SaveChangesAsync();

            return new ServiceResult<FriendInviteResponseDto> { Success = true };
        }

        if (friendInvite.Status == "Pending")
        {
            // If user accepted the invite
            if (dto.InviteStatus == "Accepted")
            {
                friendInvite.Status = dto.InviteStatus;

                await _nookContext.SaveChangesAsync();

                return new ServiceResult<FriendInviteResponseDto>
                {
                    Success = true,
                    Data = new FriendInviteResponseDto
                    {
                        RequesterId = RequesterId,
                        Message = "Friend request has been accepted",
                    },
                };
            }

            // if user rejected the invite
            friendInvite.Status = dto.InviteStatus;

            _nookContext.Friends.Remove(friendInvite);

            await _nookContext.SaveChangesAsync();

            return new ServiceResult<FriendInviteResponseDto>
            {
                Success = true,
                Data = new FriendInviteResponseDto { Message = "Invite deleted successfully" },
            };
        }

        return new ServiceResult<FriendInviteResponseDto>
        {
            Success = false,
            ErrorMessage = "An unexpected error has occured",
        };
    }

    // Service to Fetch all friend requests sent by the user
    public async Task<ServiceResult<List<AllFriendInviteDto>>> GetSentInvitesAsync(int userId)
    {
        var sentInviteList = await _nookContext
            .Friends.Where(f => f.RequesterId == userId && f.AddresseeId != userId)
            .Select(f => new AllFriendInviteDto
            {
                RequesterId = userId,
                AddresseeId = f.AddresseeId,
                InviteStatus = f.Status,
                SentAt = f.CreatedAt,
            })
            .ToListAsync();

        // No friend invites have been sent
        if (sentInviteList.Count == 0)
        {
            return new ServiceResult<List<AllFriendInviteDto>> { Success = true };
        }

        return new ServiceResult<List<AllFriendInviteDto>>
        {
            Success = true,
            Data = sentInviteList,
        };
    }

    // Service to Fetch all friend requests sent to the user
    public async Task<ServiceResult<List<AllFriendInviteDto>>> GetReceivedInvitesAsync(int userId)
    {
        var recievedInviteList = await _nookContext
            .Friends.Where(f => f.AddresseeId == userId && f.RequesterId != userId)
            .Select(f => new AllFriendInviteDto
            {
                RequesterId = userId,
                AddresseeId = f.AddresseeId,
                InviteStatus = f.Status,
                SentAt = f.CreatedAt,
            })
            .ToListAsync();

        // No friend invites have been sent
        if (recievedInviteList.Count == 0)
        {
            return new ServiceResult<List<AllFriendInviteDto>> { Success = true };
        }

        return new ServiceResult<List<AllFriendInviteDto>>
        {
            Success = true,
            Data = recievedInviteList,
        };
    }
}
