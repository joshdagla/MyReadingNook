using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyReadingNookApp.Extensions;
using MyReadingNookApp.Models;
using MyReadingNookApp.Services;
using MyReadingNookApp.Dtos.BookList;
using MyReadingNookApp.Dtos.UserBooks;
using MyReadingNookApp.Migrations;
using UserBookModel = MyReadingNookApp.Models.UserBookModel;

namespace MyReadingNookApp.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class BookListController : ControllerBase
{
    private readonly NookContext _nookContext;
    private readonly BookListService _bookListService;

    public BookListController(NookContext nookContext, BookListService bookListService)
    {
        _nookContext = nookContext;
        _bookListService = bookListService;
    }

    // Fetch all collections
    [Authorize]
    [HttpGet("getAll")]
    public async Task<IActionResult> GetAllCollections()
    {
        int userId = User.GetUserId();
        
        var userNav = await _nookContext.Users
            .Where(u => u.Id == userId)
            .FirstOrDefaultAsync();

        if (userNav == null)
        {
            return NotFound("User not found");
        }
        
        var collections = await _nookContext.BookLists
            .Where(u => u.UserModel.Id == userId)
            .Select(u => new GetBookListDto
            {
                Id = u.Id,
                ListName = u.ListName,
                CreatedAt = u.DateCreated,
                BookCovers = u.BookItems
                    .Select(b => b.ImagePath)
                    .Where(img => !string.IsNullOrEmpty(img))
                    .ToList()
            })
            .ToListAsync();

        if (collections.Count == 0)
        {
            return BadRequest("No collections found");
        }
        
        return Ok(collections);
    }
    
    // Create new book list for user
    [Authorize]
    [HttpPost("create")]
    public async Task<IActionResult> CreateBookList([FromBody] CreateBookListDto dto)
    {
        int userId = User.GetUserId();

        var userNav = await _nookContext.Users
            .Where(u => u.Id == userId)
            .FirstOrDefaultAsync();

        if (userNav == null)
        {
            return BadRequest("User not found");
        }

        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            return BadRequest("List name is empty");
        }

        BookListModel newBookList = new BookListModel
        {
            ListName = dto.Name,
            UserModel = userNav,
            Summary = dto.Summary,
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
            return BadRequest("Book list not found");
        }

        bookListModel.ListName = updatedListName;

        await _nookContext.SaveChangesAsync();
        
        return Ok("Book list name updated successfully");
    }

    // Put endpoint to update book list summary
    [Authorize]
    [HttpPut("update/summary/{bookListId}")]
    public async Task<IActionResult> UpdateListSummary([FromRoute] int bookListId,[FromBody] string summary)
    {
        int userId = User.GetUserId();
        
        var bookListModel = await _nookContext.BookLists
            .Where(bl => bl.UserModel.Id == userId && bl.Id == bookListId)
            .FirstOrDefaultAsync();

        if (bookListModel == null)
        {
            return BadRequest("Book list not found");
        }
        
        bookListModel.Summary = summary;
        
        await _nookContext.SaveChangesAsync();
        
        return Ok("Book list summary updated successfully");
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
            return BadRequest("Book list not found");
        }

        _nookContext.BookLists.Remove(bookListModel);

        await _nookContext.SaveChangesAsync();
        
        return Ok("Book list deleted successfully");
    }

    // Add a book in the list
    [Authorize]
    [HttpPost("add/{bookListId}")]
    public async Task<IActionResult> AddBookToList([FromRoute] int bookListId, [FromBody] int bookId)
    {
        int userId = User.GetUserId();
        
        var currentBook = await _nookContext.Books.FirstOrDefaultAsync(b => b.Id == bookId);

        if (currentBook == null)
        {
            return BadRequest("Book not found");
        }
        
        var result = await _bookListService.AddBookListAsync(userId, bookListId, currentBook);

        if (!result.Success)
        {
            return BadRequest(result.ErrorMessage);
        }
        
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
            return BadRequest("Book not found");
        }
        
        var result = await _bookListService.RemoveBookListAsync(userId, bookListId, currentBook);
        return Ok(result);
    }

    // Get all books in a collection
    [Authorize]
    [HttpGet("get/{bookListId}")]
    public async Task<IActionResult> GetAllBooksInList([FromRoute] int bookListId)
    {
        int userId = User.GetUserId();
        
        var bookListModel = await _nookContext.BookLists
            .Where(u => u.UserModel.Id == userId && u.Id == bookListId)
            .FirstOrDefaultAsync();

        if (bookListModel == null)
        {
            return BadRequest("Book list not found");
        }

        var collectionDetails = await _nookContext.BookLists
            .Where(u => u.UserModel.Id == userId && u.Id == bookListModel.Id)
            .Select(u => new CollectionDetailDto
            {
                Id = u.Id,
                Name = u.ListName,
                Description = u.Summary,
                CreatedAt = u.DateCreated,

                Books = u.BookItems.Select(b => new AllBooksResponseDto
                    {
                        BookId = b.Id,
                        Title = b.Title,
                        Author = b.Author,
                        ImagePath = b.ImagePath,
                        BookRating = _nookContext.UserBooks
                            .Where(ub => ub.UserModel.Id == userId && ub.BookModel.Id == b.Id)
                            .Select(ub => ub.BookRating)
                            .FirstOrDefault(),

                        BookStatus = _nookContext.UserBooks
                            .Where(ub => ub.UserModel.Id == userId && ub.BookModel.Id == b.Id)
                            .Select(ub => ub.BookStatus)
                            .FirstOrDefault()
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();

        return Ok(collectionDetails);
    }

    [Authorize]
    [HttpGet("options/{bookListId}")]
    public async Task<IActionResult> GetBookOptions([FromRoute] int bookListId)
    {
        int userId = User.GetUserId();

        var existingBookIds = await _nookContext.BookLists
            .Where(bl => bl.Id == bookListId)
            .SelectMany(bl => bl.BookItems.Select(b => b.Id))
            .ToListAsync();

        var allUserBooks = await _nookContext.UserBooks
            .Where(ub => ub.UserModel.Id == userId)
            .Select(ub => new
            {
                BookId = ub.BookModel.Id,
                Title = ub.BookModel.Title,
                ImagePath = ub.BookModel.ImagePath,
            })
            .ToListAsync();

        var availableBooks = allUserBooks
            .ExceptBy(existingBookIds, book => book.BookId)
            .ToList();    
        
        return Ok(availableBooks);
    }
}