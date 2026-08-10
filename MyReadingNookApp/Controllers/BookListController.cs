using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyReadingNookApp.Extensions;
using MyReadingNookApp.Models;
using MyReadingNookApp.Services;

namespace MyReadingNookApp.Controllers;

public class BookListController : ControllerBase
{
    private readonly NookContext _nookContext;
    private readonly BookListService _bookListService;

    public BookListController(NookContext nookContext, BookListService bookListService)
    {
        _nookContext = nookContext;
        _bookListService = bookListService;
    }

    // Create new book list for user
    [Authorize]
    [HttpPost("new")]
    public async Task<IActionResult> CreateBookList([FromBody] string newListName)
    {
        int userId = User.GetUserId();

        var userNav = await _nookContext.Users
            .Where(u => u.Id == userId)
            .FirstOrDefaultAsync();

        if (userNav == null)
        {
            return BadRequest("User not found");
        }

        if (newListName == "")
        {
            return BadRequest("List name is empty");
        }

        BookListModel newBookList = new BookListModel
        {
            ListName = newListName,
            UserModel = userNav,
        };

        _nookContext.BookLists.Add(newBookList);

        await _nookContext.SaveChangesAsync();
        
        return Ok("Book list created successfully");
    }

    // Put endpoint to rename book list name
    [Authorize]
    [HttpPut("rename/{bookListId}")]
    public async Task<IActionResult> UpdateListName([FromBody] string updatedListName, [FromRoute] int bookListId)
    {
        int userId = User.GetUserId();

        var bookListModel = await _nookContext.BookLists
            .Where(u => u.UserModel.Id == userId && u.Id == bookListId)
            .FirstOrDefaultAsync();

        if (bookListModel == null)
        {
            return NotFound("Book list not found");
        }

        bookListModel.ListName = updatedListName;

        await _nookContext.SaveChangesAsync();
        
        return Ok("Book list name updated successfully");
    }

    // Delete endpoint to delete the list
    [Authorize]
    [HttpDelete("delete/{bookListId}")]
    public async Task<IActionResult> DeleteBookList([FromBody] int bookListId)
    {
        int userId = User.GetUserId();
        
        
        var bookListModel = await _nookContext.BookLists
            .Where(u => u.UserModel.Id == userId && u.Id == bookListId)
            .FirstOrDefaultAsync();

        if (bookListModel == null)
        {
            return NotFound("Book list not found");
        }

        _nookContext.BookLists.Remove(bookListModel);

        await _nookContext.SaveChangesAsync();
        
        return Ok("Book list deleted successfully");
    }

    // Add a book in the list
    [Authorize]
    [HttpPost("add/{bookListId}")]
    public async Task<IActionResult> AddBookToList([FromQuery] int bookListId, [FromBody] int bookId)
    {
        int userId = User.GetUserId();
        
        var currentBook = await _nookContext.Books.FirstOrDefaultAsync(b => b.Id == bookId);

        if (currentBook == null)
        {
            return NotFound("Book not found");
        }
        
        var result = await _bookListService.AddBookListAsync(userId, bookListId, currentBook);
        return Ok(result);
    }

    // Remove a book from the list
    [Authorize]
    [HttpDelete("remove/{bookListId}")]
    public async Task<IActionResult> RemoveBookFromList([FromQuery] int bookListId, [FromBody] int bookId)
    {
        int userId = User.GetUserId();
        
        var currentBook = await _nookContext.Books.FirstOrDefaultAsync(b => b.Id == bookId);

        if (currentBook == null)
        {
            return NotFound("Book not found");
        }
        
        var result = await _bookListService.RemoveBookListAsync(userId, bookListId, currentBook);
        return Ok(result);
    }
}