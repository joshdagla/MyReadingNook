using MyReadingNookApp.Services;
using Microsoft.AspNetCore.Mvc;
using MyReadingNookApp.Dtos.Books;
using MyReadingNookApp.Models;

namespace MyReadingNookApp.Controllers;

// TODO: On the Client-side, BookSearch API will have to be handled using JS fetch
[ApiController]
[Route("api/[controller]")] // Route: api/books
public class BooksController : ControllerBase
{
    private readonly BookSearchService _bookService;

    public BooksController(BookSearchService bookService)
    {
        _bookService = bookService;
    }

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

            if (books == null || !books.Any())
            {
                return NotFound(new { message = "No books found matching your criteria." });
            }

            var formattedResponse = books
                .Where(b => b.VolumeInfo.IndustryIdentifiers?.Any(i => i.Type == "ISBN_13" || i.Type == "ISBN_10") ?? false)
                .Select(b => new
            {
                Id = b.Id,
                Title = b.VolumeInfo.Title,
                Authors = b.VolumeInfo.Authors != null
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
}
