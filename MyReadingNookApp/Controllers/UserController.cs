using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyReadingNookApp.Dtos.UserBooks;
using MyReadingNookApp.Extensions;
using MyReadingNookApp.Models;

namespace MyReadingNookApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly NookContext _nookContext;

    public UserController(NookContext nookContext)
    {
        _nookContext = nookContext;
    }

    [Authorize]
    [HttpGet("username")]
    public async Task<IActionResult> GetUserName()
    {
        int userId = User.GetUserId();

        var username = _nookContext.Users
            .Where(u => u.Id == userId)
            .Select(u => u.Username);

        if (username == null)
        {
            return BadRequest("Username not found");
        }
        
        return Ok(username);
    }

    [Authorize]
    [HttpGet("bookStats")]
    public async Task<IActionResult> GetUserBookStats()
    {
        int userId = User.GetUserId();

        var response = _nookContext.Users
            .Where(u => u.Id == userId)
            .Select(u => new BookStatsResponseDto
            {
                TotalBooks = u.TotalBookCount,
                CompletedBooks = u.CompletedBookCount,
                PendingBooks = u.PendingBookCount,
            });

        return Ok(response);
    }

    [Authorize]
    [HttpGet("collectionStats")]
    public async Task<IActionResult> GetUserCollectionStats()
    {
        int userId = User.GetUserId();
        
        var collectionsList = await _nookContext.BookLists
            .Where(bl => bl.UserModel.Id == userId)
            .Select(bl => new CollectionItemDto
            {
                CollectionName = bl.ListName,
                BookCount = bl.BookItems.Count()
            })
            .ToListAsync();

        if (collectionsList == null)
        {
            return BadRequest("Error in retrieving collection stats");
        }

        var response = new CollectionStatsResponseDto
        {
            TotalCollections = collectionsList.Count,
            Collections = collectionsList
        };
        
        return Ok(response);
    }
}