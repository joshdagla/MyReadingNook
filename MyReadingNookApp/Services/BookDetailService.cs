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
                var googleResults = await _bookSearchService.SearchBooksAsync(null, null, isbn, 5);

                var googleBook = googleResults.FirstOrDefault(b =>
                    b.VolumeInfo.IndustryIdentifiers?.Any(i => i.Identifier == isbn) ?? false);

                if (googleBook == null)
                {
                    return new BookDetailsResult<BookDetailsResponseDto>
                    {
                        Success = false,
                        ErrorMessage = "No books found",
                    };
                }

                // Book found in books API
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
                        BookId = bookExists.Id,
                        Author = bookExists.Author,
                        Title = bookExists.Title,
                        Description = bookExists.Description,
                        Isbn = bookExists.Isbn,
                        IsInCollection = false,
                        Publisher =  bookExists.Publisher,
                        Thumbnail = bookExists.ImagePath,
                    }
                };
            }

            // If user has it added in their collection
            return new BookDetailsResult<BookDetailsResponseDto>
            {
                Success = true,
                Data = new BookDetailsResponseDto
                {
                    BookId =  bookExists.Id,
                    Author = bookExists.Author,
                    Title = bookExists.Title,
                    Description = bookExists.Description,
                    Isbn = bookExists.Isbn,
                    IsInCollection = true,
                    Publisher =  bookExists.Publisher,
                    Thumbnail = bookExists.ImagePath,
                    UserBookInfo = new UserBookInfoDto
                    {
                        BookRating = userBookExists.BookRating,
                        BookStatus =  userBookExists.BookStatus,
                        UserNotes =  userBookExists.UserNotes,
                    }
                }
            };

    }

    public async Task<BookDetailsResult<BookDetailsResponseDto>> GetBookIdAsync(int bookId, int userId)
    {
        bool userExists = await _nookContext.Users.AnyAsync(u => u.Id == userId);

        // If user does not exist
        if (!userExists)
            return new BookDetailsResult<BookDetailsResponseDto>
            {
                Success = false,
                ErrorMessage = "User not found",
            };

        var bookExists = await _nookContext.Books.FirstOrDefaultAsync(b => b.Id == bookId);

        if (bookExists == null)
        {
            return new BookDetailsResult<BookDetailsResponseDto>
            {
                Success = false,
                ErrorMessage = "No book found in database",
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
                    BookId =  bookExists.Id,
                    Author = bookExists.Author,
                    Title = bookExists.Title,
                    Description = bookExists.Description,
                    Isbn = bookExists.Isbn,
                    IsInCollection = false,
                    Publisher =  bookExists.Publisher,
                    Thumbnail = bookExists.ImagePath,
                }
            };
        }

        // If user has it added in their collection
        return new BookDetailsResult<BookDetailsResponseDto>
        {
            Success = true,
            Data = new BookDetailsResponseDto
            {
                BookId =  bookExists.Id,
                Author = bookExists.Author,
                Title = bookExists.Title,
                Description = bookExists.Description,
                Isbn = bookExists.Isbn,
                IsInCollection = true,
                Publisher =  bookExists.Publisher,
                Thumbnail = bookExists.ImagePath,
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
