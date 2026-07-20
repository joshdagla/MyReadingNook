using Microsoft.EntityFrameworkCore;
using MyReadingNookApp.Dtos.Books;
using MyReadingNookApp.Models;

namespace MyReadingNookApp.Services;

public class BookAddService : IBookAddService
{
    private readonly NookContext _nookContext;

    public BookAddService(NookContext nookContext)
    {
        this._nookContext = nookContext;
    }
    
    // Service to add book into Books table and simultaneously add it in the designated user's collection
    public async Task<BookAddResult> BookAddAsync(BookAddDto bookAddDto, int userId)
    {
        var userExists = await _nookContext.Users.AnyAsync(u => u.Id == userId);
        
        if (!userExists)
        {
            return new BookAddResult
            {
                Success = false,
                ErrorMessage = "User not found",
            };
        }
        var bookExists = await _nookContext.Books.FirstOrDefaultAsync(b => b.Isbn == bookAddDto.Isbn);

        if (bookExists != null)
        {
            var userBookExists = await _nookContext.UserBooks
                .FirstOrDefaultAsync(ub => ub.BookId == bookExists.Id && ub.UserId == userId);

            if (userBookExists != null)
            {
                return new BookAddResult
                {
                    Success = false,
                    ErrorMessage = "Book already exists in user's collection",
                };
            }

            UserBookModel newUserBookModel = new UserBookModel
            {
                BookId = bookExists.Id,
                UserId =  userId,
            };
            
            _nookContext.UserBooks.Add(newUserBookModel);

            await _nookContext.SaveChangesAsync();

            return new BookAddResult
            {
                Success = true,
            };
        }

        BookModel newBookModel = new BookModel
        {
            Title = bookAddDto.Title,
            Author = bookAddDto.Author,
            Isbn = bookAddDto.Isbn,
            Publisher = bookAddDto.Publisher,
            Description = bookAddDto.Description,
        };

        UserBookModel addUserBookModel = new UserBookModel
        {
            BookModel =  newBookModel,
            UserId = userId,
        };
        
        _nookContext.Books.Add(newBookModel);
        
        _nookContext.UserBooks.Add(addUserBookModel);

        await _nookContext.SaveChangesAsync();

        return new BookAddResult
        {
            Success = true,
        };
    }
}