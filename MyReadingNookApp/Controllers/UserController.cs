using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyReadingNookApp.Migrations;
using MyReadingNookApp.Models;

namespace MyReadingNookApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly NookContext _nookContext;

        public UserController(NookContext nookContext)
        {
            this._nookContext = nookContext;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User newUser)
        {
            if (newUser == null)
            {
                return BadRequest("Product data is null");
            }

            _nookContext.Users.Add(newUser);

            await _nookContext.SaveChangesAsync();

            return CreatedAtAction(nameof(CreateUser), new { id = newUser.Id }, newUser);
        }
    }
}
