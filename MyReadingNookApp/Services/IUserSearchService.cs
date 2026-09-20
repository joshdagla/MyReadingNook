using MyReadingNookApp.Dtos.Friends;

namespace MyReadingNookApp.Services;

public interface IUserSearchService
{
    Task<ServiceResult<List<UserSearchResponseDto>>>  SearchUserAsync(string username, int userId);
}