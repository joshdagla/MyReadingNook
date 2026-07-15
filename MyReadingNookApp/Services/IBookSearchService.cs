using Google.Apis.Books.v1.Data;

namespace MyReadingNookApp.Services;

public interface IBookSearchService
{
    public Task<IList<Volume>> SearchBookAsync(string query, int maxResults = 10);
}