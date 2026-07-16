using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyReadingNookApp.Dtos.Books;
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
      this._nookContext = nookContext;
   }

   // Allows user to update personal book info such as book status, rating and personal notes
   // Book id is received from route and 
   [Authorize]
   [HttpPut("{bookId}")] // Route: /api/userbooks/{bookid}
   public async Task<IActionResult> BookInfoUpdate([FromBody] BookInfoDto bookInfoDto, [FromRoute] int bookId)
   {
       try
       {
           int userId = User.GetUserId();

           var userBookId = await _nookContext.UserBooks.Where(ub => ub.UserId == userId && ub.BookId == bookId)
               .FirstOrDefaultAsync();

           userBookId.BookStatus = bookInfoDto.BookStatus;
           userBookId.BookRating = bookInfoDto.BookRating;

           if (bookInfoDto.UserNote.Length > 250)
           {
               return BadRequest("User note exceeds character limit");
           }
           userBookId.UserNotes = bookInfoDto.UserNote;
           
           return Ok(userBookId);
       }
       catch (Exception ex)
       {
           return StatusCode(500, ex.Message);
       }
   }
}