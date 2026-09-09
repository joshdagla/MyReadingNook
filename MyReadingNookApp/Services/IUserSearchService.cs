using MyReadingNookApp.Dtos.Friends;

namespace MyReadingNookApp.Services;

public class ProcessResult
{
    public bool Success { get; set; }

    public string? ErrorMsg { get; set; }
}

public interface IUserSearchService
{
    Task<ProcessResult>  SearchUserAsync(SearchUsernameDto username);
}