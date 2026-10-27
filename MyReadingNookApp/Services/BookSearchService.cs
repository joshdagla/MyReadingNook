using Google.Apis.Books.v1.Data;
using Google.Apis.Services;

namespace MyReadingNookApp.Services;

public class BookSearchService
{
    private readonly Google.Apis.Books.v1.BooksService _googleBooksClient;

    public BookSearchService(IConfiguration configuration)
    {
        var apiKey = configuration["GoogleBooks:ApiKey"];

        _googleBooksClient = new Google.Apis.Books.v1.BooksService(new BaseClientService.Initializer()
        {
            ApiKey = apiKey,
            ApplicationName = "MyReadingNookApp"
        });
    }

    public async Task<IList<Volume>> SearchBooksAsync(string? title, string? author, string? isbn, int maxResults = 10)
    {
        var queryParts = new List<string>();

        // Plain keywords: fielded prefixes (intitle:, inauthor:, isbn:) returned no results
        if (!string.IsNullOrWhiteSpace(title))
            queryParts.Add(title.Trim());

        if (!string.IsNullOrWhiteSpace(author))
            queryParts.Add(author.Trim());

        if (!string.IsNullOrWhiteSpace(isbn))
            queryParts.Add(isbn.Trim());

        string finalQuery = string.Join(" ", queryParts);

        if (string.IsNullOrWhiteSpace(finalQuery))
        {
            return new List<Volume>();
        }

        var listRequest = _googleBooksClient.Volumes.List(finalQuery);
        listRequest.OrderBy = Google.Apis.Books.v1.VolumesResource.ListRequest.OrderByEnum.Relevance;
        listRequest.MaxResults = maxResults;

        var result = await listRequest.ExecuteAsync();
        return result.Items ?? new List<Volume>();
    }
}
