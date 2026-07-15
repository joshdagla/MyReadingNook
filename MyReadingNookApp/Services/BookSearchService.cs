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

        // Append Google query prefixes based on user inputs
        if (!string.IsNullOrWhiteSpace(title))
            queryParts.Add($"intitle:{title}");

        if (!string.IsNullOrWhiteSpace(author))
            queryParts.Add($"inauthor:{author}");

        if (!string.IsNullOrWhiteSpace(isbn))
            queryParts.Add($"isbn:{isbn}");

        // Combining parameters separated by spaces
        string finalQuery = string.Join(" ", queryParts);

        if (string.IsNullOrWhiteSpace(finalQuery))
        {
            return new List<Volume>(); // Return empty if no parameters were supplied
        }

        var listRequest = _googleBooksClient.Volumes.List(finalQuery);
        listRequest.MaxResults = maxResults;

        var result = await listRequest.ExecuteAsync();
        return result.Items ?? new List<Volume>();
    }
}