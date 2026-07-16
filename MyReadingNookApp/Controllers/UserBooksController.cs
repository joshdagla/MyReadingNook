using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyReadingNookApp.Dtos.Books;
using MyReadingNookApp.Models;

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

   [Authorize]
   [HttpPut]
   public IActionResult BookStatusAsync([FromBody] BookInfoDto bookInfoDto)
   {
      
   }
}