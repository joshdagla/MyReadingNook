using MyReadingNookApp.Dtos.Friends;

namespace MyReadingNookApp.Services;

public interface IUserSearchService
{
    Task<ServiceResult<List<UserSearchResponseDto>>>  SearchUserAsync(SearchUsernameDto username, int userId);
}