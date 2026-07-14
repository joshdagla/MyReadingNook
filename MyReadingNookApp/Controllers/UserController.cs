using Microsoft.AspNetCore.Mvc;
using MyReadingNookApp.Models;

namespace MyReadingNookApp.Controllers;

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
    public async Task<IActionResult> CreateUser([FromBody] UserModel newUserModel)
    {
        if (newUserModel == null)
        {
            return BadRequest("User data is null");
        }

        _nookContext.Users.Add(newUserModel);

        await _nookContext.SaveChangesAsync();

        return CreatedAtAction(nameof(CreateUser), new { id = newUserModel.Id }, newUserModel);
    }
}

