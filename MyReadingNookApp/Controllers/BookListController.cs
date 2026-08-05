using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyReadingNookApp.Extensions;
using MyReadingNookApp.Models;

namespace MyReadingNookApp.Controllers;

public class BookListController : ControllerBase
{
    private readonly NookContext _nookContext;

    public BookListController(NookContext nookContext)
    {
        _nookContext = nookContext;
    }

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

    [Authorize]
    [HttpPut("update")]
    public async Task<IActionResult> UpdateListName([FromBody] string updatedListName)
    {
        int userId = User.GetUserId();

        var bookListModel = await _nookContext.BookLists
            .Where(u => u.UserModel.Id == userId)
            .FirstOrDefaultAsync();

        if (bookListModel == null)
        {
            return NotFound("Book list not found");
        }

        bookListModel.ListName = updatedListName;

        await _nookContext.SaveChangesAsync();
        
        return Ok("Book list name updated successfully");
    }
}