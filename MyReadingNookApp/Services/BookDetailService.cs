using Microsoft.EntityFrameworkCore;
using MyReadingNookApp.Dtos.Books;
using MyReadingNookApp.Models;

namespace MyReadingNookApp.Services;

public class BookDetailService : IBookDetailService
{
    private readonly NookContext _nookContext;
    private readonly BookSearchService _bookSearchService; // or IBookSearchService, if it has one

    public BookDetailService(NookContext nookContext, BookSearchService bookSearchService)
    {
        _nookContext = nookContext;
        _bookSearchService = bookSearchService;
    }
    public async Task<BookDetailsResult<BookDetailsResponseDto>> GetBookDetailsAsync(string isbn, int userId)
    {
            bool userExists = await _nookContext.Users.AnyAsync(u => u.Id == userId);

            // If user does not exist
            if (!userExists)
                return new BookDetailsResult<BookDetailsResponseDto>
                {
                    Success = false,
                    ErrorMessage = "User not found",
                };

            var bookExists = await _nookContext.Books.FirstOrDefaultAsync(b => b.Isbn == isbn);

            // If book does not exist in database
            if (bookExists == null)
            {
                var googleResults = await _bookSearchService.SearchBooksAsync(null, null, isbn, maxResults: 1);

                // If book with the designated Isbn is not found
                if (!googleResults.Any())
                {
                    return new BookDetailsResult<BookDetailsResponseDto>
                    {
                        Success = false,
                        ErrorMessage = "No books found",
                    };
                }
                
                var googleBook =  googleResults.First();
                
                return new BookDetailsResult<BookDetailsResponseDto>
                {
                    Success = true,
                    Data = new BookDetailsResponseDto
                    {
                        IsInCollection = false,
                        Title = googleBook.VolumeInfo.Title,
                        Author = googleBook.VolumeInfo.Authors != null
                            ? string.Join(", ", googleBook.VolumeInfo.Authors)
                            : "Unknown Author(s)",
                        Publisher = googleBook.VolumeInfo.Publisher,
                        Description = googleBook.VolumeInfo.Description,
                        Isbn = isbn, // you already have it — it's the parameter you searched with
                        Thumbnail = googleBook.VolumeInfo.ImageLinks?.Thumbnail                        
                    }
                };
            }

            var userBookExists = await _nookContext.UserBooks
                .FirstOrDefaultAsync(ub => ub.BookId == bookExists.Id && ub.UserId == userId);

            // If book is in database but current user does not have it in their collection
            if (userBookExists == null)
            {
                return new BookDetailsResult<BookDetailsResponseDto>
                {
                    Success = true,
                    Data = new BookDetailsResponseDto
                    {
                        Author = bookExists.Author,
                        Title = bookExists.Title,
                        Description = bookExists.Description,
                        Isbn = bookExists.Isbn,
                        IsInCollection = false,
                        Publisher =  bookExists.Publisher,
                        //TODO: Need to add thumbnail functionality
                    }
                };
            }
            
            // If user has it added in their collection
            return new BookDetailsResult<BookDetailsResponseDto>
            {
                Success = true,
                Data = new BookDetailsResponseDto
                {
                    Author = bookExists.Author,
                    Title = bookExists.Title,
                    Description = bookExists.Description,
                    Isbn = bookExists.Isbn,
                    IsInCollection = true,
                    Publisher =  bookExists.Publisher,
                    //TODO: Need to add thumbnail functionality
                    UserBookInfo = new UserBookInfoDto
                    {
                        BookRating = userBookExists.BookRating,
                        BookStatus =  userBookExists.BookStatus,
                        UserNotes =  userBookExists.UserNotes,
                    }
                }
            };

    }
}