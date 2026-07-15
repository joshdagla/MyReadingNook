using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MyReadingNookApp.Dtos.Books;

public class OpenLibraryResponse
{
    [JsonPropertyName("numFound")]
    public int NumFound { get; set; }

    [JsonPropertyName("bookList")] public List<BookSearchDto> BookList { get; set; } = new();
}

public class BookSearchDto
{
    [JsonPropertyName("title")]
    public string? Title { get; set; } = string.Empty;
    
    [JsonPropertyName("author")]
    public string? Author { get; set; }
    
    [JsonPropertyName("isbn")]
    [StringLength(13)]
    public string? Isbn { get; set; }
}

// API KEY: AIzaSyCjyHhe7d37YB00auUqHcHaJfTI2KbTcr4