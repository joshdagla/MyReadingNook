using Microsoft.EntityFrameworkCore;
using MyReadingNookApp.Models;

namespace MyReadingNookApp.Services;

public class BookListService : IBookListService
{
    private readonly NookContext _nookContext;

    public BookListService(NookContext nookContext)
    {
        _nookContext = nookContext;
    }
    
    // Service to add a book to the Book List
    public async Task<BookToListResult> AddBookListAsync(int userId, int bookListId, BookModel bookModel)
    {
        var bookList = await _nookContext.BookLists
            .Where(bl => bl.Id == bookListId && bl.UserModel.Id == userId)
            .FirstOrDefaultAsync();
        
        bool bookExistsInList = await _nookContext.BookLists
            .Where(bl => bl.Id == bookListId && bl.UserModel.Id == userId && bl.BookItems.Any(bi => bi.Id == bookModel.Id))
            .AnyAsync();

        // Book not found in the list
        if (bookExistsInList == true)
        {
            return new BookToListResult
            {
                Success = false,
                ErrorMessage = "Book already exists in the list",
            };
        }

        // Book list not found
        if (bookList == null)
        {
            return new BookToListResult
            {
                Success = false,
                ErrorMessage = "Book list not found",
            };
        }

        // Book model not in the database
        if (bookModel.Id == null)
        {
            return new BookToListResult
            {
                Success = false,
                ErrorMessage = "Book not found",
            };
        }
        
        bookList.BookItems.Add(bookModel);
        
        await _nookContext.SaveChangesAsync();

        return new BookToListResult
        {
            Success = true,
        };
    }

    // Service to remove a book from the book list
    public async Task<BookToListResult> RemoveBookListAsync(int userId, int bookListId, BookModel bookModel)
    {
        var bookList = await _nookContext.BookLists
            .Where(bl => bl.Id == bookListId && bl.UserModel.Id == userId)
            .FirstOrDefaultAsync();
        
        bool bookExistsInList = await _nookContext.BookLists
            .Where(bl => bl.Id == bookListId && bl.UserModel.Id == userId && bl.BookItems.Any(bi => bi.Id == bookModel.Id))
            .AnyAsync();

        // Book not found in the list
        if (bookExistsInList)
        {
            return new BookToListResult
            {
                Success = false,
                ErrorMessage = "Book already exists in the list",
            };
        }

        // Book list not found
        if (bookList == null)
        {
            return new BookToListResult
            {
                Success = false,
                ErrorMessage = "Book list not found",
            };
        }

        // Book model not in the database
        if (bookModel.Id == null)
        {
            return new BookToListResult
            {
                Success = false,
                ErrorMessage = "Book not found",
            };
        }
        
        bookList.BookItems.Remove(bookModel);
        
        await _nookContext.SaveChangesAsync();

        return new BookToListResult
        {
            Success = true,
        }; 
    }
}