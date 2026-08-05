using MyReadingNookApp.Models;

namespace MyReadingNookApp.Services;

public class BookToListResult
{
    public bool Success { get; set; }

    public string? ErrorMessage { get; set; }
}

public interface IBookListService
{
   public Task<BookToListResult> AddBookListAsync(int userId, int bookListId, BookModel bookModel); 
   
   public Task<BookToListResult> RemoveBookListAsync(int userId, int bookListId, BookModel bookModel);
}