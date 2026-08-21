using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyReadingNookApp.Dtos.Books;
using MyReadingNookApp.Dtos.UserBooks;
using MyReadingNookApp.Models;
using MyReadingNookApp.Extensions;

namespace MyReadingNookApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserBooksController : ControllerBase
{
   private readonly NookContext _nookContext;

   public UserBooksController(NookContext nookContext)
   {
      _nookContext = nookContext;
   }

   // GET endpoint to fetch all books from user's collection
   [Authorize]
   [HttpGet("all")] // Route: /api/userbooks/all
   public async Task<IActionResult> FetchAllBooks()
   {
       try
       {
           int userId = User.GetUserId();

           List<AllBooksResponseDto> allUserBooks = await _nookContext.UserBooks
               .Where(ub => ub.UserId == userId)
               .Select(ub => new AllBooksResponseDto
               {
                   BookId = ub.BookModel.Id,
                   Title = ub.BookModel.Title,
                   Author = ub.BookModel.Author,
                   ImagePath = ub.BookModel.ImagePath,
                   BookStatus = ub.BookStatus,
                   BookRating = ub.BookRating
               })
               .ToListAsync();
      
           return Ok(allUserBooks);
       }
       catch (Exception e)
       {
           return StatusCode(500, "An internal server error has occurred");
       }
   }

   // Allows user to update personal book info such as book status, rating and personal notes
   // Book id is received from route
   [Authorize]
   [HttpPatch("update/{bookId:int}")] // Route: /api/userbooks/update/{bookid}
   public async Task<IActionResult> BookInfoUpdate([FromBody] BookInfoDto bookInfoDto, [FromRoute] int bookId)
   {
       try
       {
           int userId = User.GetUserId();

           if (bookId == null)
           {
               return BadRequest($"Book not found in collection");
           }

           // Check if user has book in the collection
           var userBookId = await _nookContext.UserBooks.Where(ub => ub.UserId == userId && ub.BookId == bookId)
               .FirstOrDefaultAsync();

           if (userBookId == null)
           {
               return BadRequest($"Book not found in User's collection");
           }

           if (bookInfoDto.BookRating != null) userBookId.BookRating = bookInfoDto.BookRating;
           if (bookInfoDto.BookStatus != null) userBookId.BookStatus = bookInfoDto.BookStatus;
           if (bookInfoDto.UserNote != null) userBookId.UserNotes = bookInfoDto.UserNote;

           await _nookContext.SaveChangesAsync();

           return Ok(userBookId);
       }
       catch (KeyNotFoundException)
       {
           return BadRequest($"Book not found in collection");
       }
       catch (NullReferenceException)
       {
           return StatusCode(500, "An internal server error has occurred");
       }
       catch (Exception)
       {
           return StatusCode(500, "An internal server error has occurred");
       }
   }
   
   // Remove book from user's collection
   [Authorize]
   [HttpDelete("delete/{bookId:int}")]
   public async Task<IActionResult> DeleteBook([FromRoute] int bookId)
   {
       try
       {
           int userId = User.GetUserId();

           var deleteBook = await _nookContext.UserBooks.Where(ub => ub.UserId == userId && ub.BookId == bookId)
               .ExecuteDeleteAsync();

           return Ok($"Book has been deleted from collection");
       }
       catch (KeyNotFoundException)
       {
           return BadRequest($"Book not found in collection");
       }
       catch (NullReferenceException)
       {
           return StatusCode(500, "An internal server error has occurred");
       }
       catch (Exception)
       {
           return StatusCode(500, "An internal server error has occurred");
       }
   }

   [Authorize]
   [HttpGet("getInfo/{bookId:int}")]
   public async Task<IActionResult> DisplayBookInfo([FromRoute] int bookId)
   {
       try
       {
           int userId = User.GetUserId();

           var userBookInfo = await _nookContext.UserBooks.Where(ub => ub.UserId == userId && ub.BookId == bookId)
               .Select(ub => new BookInfoDto
               {
                   BookRating = ub.BookRating,
                   BookStatus = ub.BookStatus,
                   UserNote = ub.UserNotes,
               })
               .FirstOrDefaultAsync();

           if (userBookInfo == null)
           {
               return BadRequest($"No feedback found for this book");
           }

           return Ok(userBookInfo);
       }
       catch (Exception)
       {
           return StatusCode(500, "An internal server error has occured");
       }
   }
}