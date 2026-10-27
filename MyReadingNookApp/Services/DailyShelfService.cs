using Google.Apis.Books.v1;
using Google.Apis.Books.v1.Data;
using Google.Apis.Services;

namespace MyReadingNookApp.Services;

public class DailyShelfService
{
    private readonly BooksService _googleBooksClient;
    private readonly ILogger<DailyShelfService> _logger;

    public DailyShelfService(IConfiguration configuration, ILogger<DailyShelfService> logger)
    {
        var apiKey = configuration["GoogleBooks:ApiKey"];
        Console.WriteLine($"Key loaded: {!string.IsNullOrEmpty(apiKey)}");

        _googleBooksClient = new BooksService(
            new BaseClientService.Initializer()
            {
                ApiKey = apiKey,
                ApplicationName = "MyReadingNookApp",
            }
        );

        _logger = logger;
    }

    // Hard-coded list of subjects to query from
    private static readonly string[] ShelfSubjects =
    {
        "fantasy",
        "classics",
        "philosophy",
        "mystery",
        "biography",
        "horror",
        "romance",
        "nonfiction",
    };

    private static List<string> GetTodaysSubjects(int count = 5)
    {
        // DateTime.UtcNow.Date.GetHashCode() changes daily
        int seed = DateTime.UtcNow.Date.GetHashCode();
        var random = new Random(seed);

        return ShelfSubjects.OrderBy(_ => random.Next()).Take(count).ToList();
    }

    public async Task<IList<Volume>> GetTodayShelfAsync()
    {
        var subjects = GetTodaysSubjects();

        var tasks = subjects.Select(async genre =>
        {
            var booksRequest = _googleBooksClient.Volumes.List(genre);
            booksRequest.OrderBy = VolumesResource.ListRequest.OrderByEnum.Relevance;
            booksRequest.MaxResults = 3;

            var response = await booksRequest.ExecuteAsync();
            return response.Items ?? new List<Volume>();
        });

        var results = await Task.WhenAll(tasks);

        return results.SelectMany(b => b).ToList();
    }
}
