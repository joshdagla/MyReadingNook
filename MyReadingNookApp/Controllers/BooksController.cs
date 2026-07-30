using Microsoft.AspNetCore.Authorization;
using MyReadingNookApp.Services;
using Microsoft.AspNetCore.Mvc;
using MyReadingNookApp.Dtos.Books;
using MyReadingNookApp.Extensions;
using MyReadingNookApp.Models;

namespace MyReadingNookApp.Controllers;

// TODO: On the Client-side, BookSearch API will have to be handled using JS fetch
[ApiController]
[Route("api/[controller]")] // Route: api/books
public class BooksController : ControllerBase
{
    private readonly BookSearchService _bookService;
    private readonly BookAddService _bookAddService;

    public BooksController(BookSearchService bookService, BookAddService bookAddService)
    {
        _bookService = bookService;
        _bookAddService = bookAddService;
    }
    
    // Controller to search and query books from Google books api
    [HttpGet("search")] // Route resolves to: api/books/search
    public async Task<IActionResult> SearchBooks(
        [FromQuery] string? title, 
        [FromQuery] string? author, 
        [FromQuery] string? isbn, 
        [FromQuery] int maxResults = 10)
    {
        // Validation: Make sure at least one search criterion is provided
        if (string.IsNullOrWhiteSpace(title) && 
            string.IsNullOrWhiteSpace(author) && 
            string.IsNullOrWhiteSpace(isbn))
        {
            return BadRequest(new { message = "You must provide at least one search parameter (title, author, or isbn)." });
        }

        try
        {
            var books = await _bookService.SearchBooksAsync(title, author, isbn, maxResults);

            if (!books.Any())
            {
                return NotFound(new { message = "No books found matching your criteria." });
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
                Thumbnail = b.VolumeInfo.ImageLinks?.Thumbnail
            });

            return Ok(formattedResponse);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Google Books API Error", details = ex.Message });
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
}
