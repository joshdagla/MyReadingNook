using Microsoft.EntityFrameworkCore;
using MyReadingNookApp.Dtos.Friends;
using MyReadingNookApp.Models;

namespace MyReadingNookApp.Services;

public class UserSearchService : IUserSearchService
{
    private readonly NookContext _nookContext;

    public UserSearchService(NookContext nookContext)
    {
        _nookContext = nookContext;
    }
    
    // Service to search from all users
    public async Task<ServiceResult<List<UserSearchResponseDto>>> SearchUserAsync(string username, int userId)
    {
        var userList = await _nookContext.Users
            .Where(u => u.Id != userId && u.Username.Contains(username))
            .Select(u => new UserSearchResponseDto
            {
                Id = u.Id,
                Username = u.Username,
                ProfileVisibility = u.ProfileVisibility,
            })
            .ToListAsync();

        return new ServiceResult<List<UserSearchResponseDto>>
        {
            Success = true,
            Data = userList
        };

    }
}