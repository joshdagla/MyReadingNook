using Microsoft.AspNetCore.Authorization;
using MyReadingNookApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyReadingNookApp.Dtos.Books;
using MyReadingNookApp.Extensions;
using MyReadingNookApp.Models;

namespace MyReadingNookApp.Controllers;

[ApiController]
[Route("api/[controller]")] // Route: api/books
public class BooksController : ControllerBase
{
    private readonly NookContext _nookContext;
    private readonly BookSearchService _bookService;
    private readonly BookAddService _bookAddService;
    private readonly BookDetailService _bookDetailService;
    private readonly DailyShelfService _dailyShelfService;

    public BooksController(NookContext nookContext,BookSearchService bookService, BookAddService bookAddService, BookDetailService bookDetailService, DailyShelfService dailyShelfService)
    {
        _nookContext = nookContext;
        _bookService = bookService;
        _bookAddService = bookAddService;
        _bookDetailService = bookDetailService;
        _dailyShelfService = dailyShelfService;
    }
    
    // Controller to search and query books from Google books api
    [Authorize]
    [HttpGet("search")] // Route resolves to: api/books/search
    public async Task<IActionResult> SearchBooks(
        [FromQuery] string? title, 
        [FromQuery] string? author, 
        [FromQuery] string? isbn, 
        [FromQuery] int maxResults = 10)
    {
        // Validation: Makes sure at least one search criterion is provided
        if (string.IsNullOrWhiteSpace(title) && 
            string.IsNullOrWhiteSpace(author) && 
            string.IsNullOrWhiteSpace(isbn))
        {
            return BadRequest(new { message = "You must provide at least one search parameter (title, author, or isbn)." });
        }

        try
        {
            int  userId = User.GetUserId();
            var books = await _bookService.SearchBooksAsync(title, author, isbn, maxResults);

            if (!books.Any())
            {
                return BadRequest(new { message = "No books found matching your criteria." });
            }

            var formattedResponse = books
                .Where(b => b.VolumeInfo.IndustryIdentifiers?.Any(i => i.Type is "ISBN_13" or "ISBN_10") ?? false)
                .Select(b => new
            {
                Id = b.Id,
                Title = b.VolumeInfo.Title,
                Author = b.VolumeInfo.Authors != null
                ? string.Join(", ", b.VolumeInfo.Authors)
                : "Unknown Author(s)",
                Publisher = b.VolumeInfo.Publisher,
                Description = b.VolumeInfo.Description,
                Isbn = b.VolumeInfo.IndustryIdentifiers?.FirstOrDefault(i => i.Type == "ISBN_13")?.Identifier
                       ?? b.VolumeInfo.IndustryIdentifiers?.FirstOrDefault(i => i.Type == "ISBN_10")?.Identifier
                       ?? "",
                Thumbnail = b.VolumeInfo.ImageLinks?.Thumbnail,
            });

            return Ok(formattedResponse);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Google Books API Error", details = ex.Message });
        }
    }

    [Authorize]
    [HttpGet("dailyShelf")]
    public async Task<IActionResult> GetDailyShelf()
    {
        try
        {
            var shelfBooks = await _dailyShelfService.GetTodayShelfAsync();

            if (!shelfBooks.Any())
            {
                return BadRequest(new { message = "No books found currently for daily shelf" });
            }

            var formattedResponse = shelfBooks
                .Select(b => new
                {
                    Title = b.VolumeInfo.Title,
                    Author = b.VolumeInfo.Authors != null
                        ? string.Join(", ", b.VolumeInfo.Authors)
                        : "Unknown Author(s)",
                    Isbn = b.VolumeInfo.IndustryIdentifiers?.FirstOrDefault(i => i.Type == "ISBN_13")?.Identifier
                           ?? b.VolumeInfo.IndustryIdentifiers?.FirstOrDefault(i => i.Type == "ISBN_10")?.Identifier
                           ?? "",
                    ImagePath = b.VolumeInfo.ImageLinks?.Thumbnail,
                });
            
            return Ok(formattedResponse);
        }
        catch (Exception e)
        {
            return StatusCode(500, new { message = "Google Books API Error", details = e.Message });
        }
    }

    // Controller to add book to collection and UserBook Table
    [Authorize]
    [HttpPost("add")] // Route: /add
    public async Task<IActionResult> AddBook([FromBody] BookAddDto bookAddDto)
    {
        try
        {
            var userId = User.GetUserId();
            
            var result = await _bookAddService.BookAddAsync(bookAddDto, userId);

            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            
            return Ok(result);

        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    // Fetch book details
    [Authorize]
    [HttpGet("bookDetail")]
    public async Task<IActionResult> GetBookDetails([FromQuery] string isbn)
    {
        try
        {
            int userId = User.GetUserId();
            var result = await _bookDetailService.GetBookDetailsAsync(isbn, userId);

            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result.Data);
        }
        catch (KeyNotFoundException e)
        {
            return StatusCode(500, e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    // Get book details by id (book details fetched from DB instead of GoogleAPI)
    [Authorize]
    [HttpGet("bookDetail/{bookId}")]
    public async Task<IActionResult> GetBookDetailById([FromRoute] int bookId)
    {
        try
        {
            int userId = User.GetUserId();
            var result = await _bookDetailService.GetBookIdAsync(bookId, userId);

            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            
            return Ok(result.Data);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}
