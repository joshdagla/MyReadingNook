using MyReadingNookApp.Dtos.Books;
using MyReadingNookApp.Migrations;

namespace MyReadingNookApp.Services;

public class BookAddResult
{
    public bool Success { get; set; }

    public string? ErrorMessage { get; set; }

    public int? BookId { get; set; }
}

public interface IBookAddService
{
     Task<BookAddResult> BookAddAsync(BookAddDto bookAddDto, int userId);
}