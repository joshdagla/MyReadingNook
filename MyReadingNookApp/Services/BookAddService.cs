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
    
    public async Task<BookAddResult> BookAddAsync(BookAddDto bookAddDto, int userId)
    {
        bool bookExists = await _nookContext.Books.AnyAsync(b => b.Isbn == bookAddDto.Isbn);

        if (bookExists)
        {
            return new BookAddResult
            {
                Success = false,
                ErrorMessage = "Book Already exists."
            };
        }
        
        BookModel newBookModel = new BookModel
        {
            
        }
    }
}