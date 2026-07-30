using MyReadingNookApp.Dtos.Books;

namespace MyReadingNookApp.Services;

public class BookDetailsResult<T>
{
    public bool Success { get; set; }

    public string? ErrorMessage { get; set; }

    public T? Data { get; set; }
}

public interface IBookDetailService
{
    Task<BookDetailsResult<BookDetailsResponseDto>> GetBookDetailsAsync(string isbn, int userId);
}