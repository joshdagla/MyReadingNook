using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyReadingNookApp.Dtos.Friends;
using MyReadingNookApp.Extensions;
using MyReadingNookApp.Services;

namespace MyReadingNookApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FriendController : ControllerBase
{
    private readonly UserSearchService _userSearchService;
    
    public FriendController(UserSearchService userSearchService)
    {
        _userSearchService = userSearchService;
    }

    [Authorize]
    [HttpGet("userSearch")]
    public async Task<IActionResult> SearchUser([FromBody] SearchUsernameDto dto)
    {
        int userId = User.GetUserId();

        var result = await _userSearchService.SearchUserAsync(dto, userId);

        if (result.Success)
        {
            return Ok(result.Data);
        }
        
        return BadRequest("Error occured while searching user. " + result.ErrorMessage);
    }
}