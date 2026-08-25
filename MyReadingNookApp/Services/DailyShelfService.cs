using Google.Apis.Books.v1;
using Google.Apis.Books.v1.Data;
using Google.Apis.Services;

namespace MyReadingNookApp.Services;

public class DailyShelfService 
{
    private readonly BooksService _googleBooksClient;

    public DailyShelfService(IConfiguration configuration)
    {
        var apiKey = configuration["GoogleBooks:ApiKey"];
        
        _googleBooksClient = new BooksService(new BaseClientService.Initializer()
        {
            ApiKey = apiKey,
            ApplicationName = "MyReadingNookApp"
        });
    }
    
    // Hard-coded list of subjects to query from
    private static readonly string[] ShelfSubjects =
    {
        "fantasy", "classics", "philosophy",
        "mystery", "biography", "horror", "romance", "nonfiction"
    };

    private static List<string> GetTodaysSubjects(int count = 5)
    {
        // DateTime.UtcNow.Date.GetHashCode() changes daily
        int seed = DateTime.UtcNow.Date.GetHashCode();
        var random = new Random(seed);
        
        return ShelfSubjects
            .OrderBy(_ => random.Next())
            .Take(count)
            .ToList();
    }

    public async Task<IList<Volume>> GetTodayShelfAsync()
    {
        var subjects = GetTodaysSubjects();

        var tasks = subjects.Select(async genre =>
        {
            try
            {
                var booksRequest = _googleBooksClient.Volumes.List($"subject:\"{genre}\"");
                booksRequest.MaxResults = 5;

                var response = await booksRequest.ExecuteAsync();
                return response.Items ?? new List<Volume>();
            }
            catch
            {
                return new List<Volume>() as IList<Volume>;
            }
        });

        var results = await Task.WhenAll(tasks);

        return results.SelectMany(b => b).ToList();
    }
}