using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MyReadingNookApp.Dtos.Friends;
using MyReadingNookApp.Extensions;
using MyReadingNookApp.Services;

namespace MyReadingNookApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FriendController : ControllerBase
{
    private readonly UserSearchService _userSearchService;
    private readonly FriendInviteService _friendInviteService;

    public FriendController(
        UserSearchService userSearchService,
        FriendInviteService friendInviteService
    )
    {
        _userSearchService = userSearchService;
        _friendInviteService = friendInviteService;
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

    [Authorize]
    [HttpPost("sendInvite")]
    public async Task<IActionResult> SendInvite([FromBody] SendFriendInviteDto dto)
    {
        int requesterId = User.GetUserId();

        var result = await _friendInviteService.SendFriendInviteAsync(dto, requesterId);

        if (!result.Success)
        {
            return BadRequest(
                "An error has occured while sending friend request. " + result.ErrorMessage
            );
        }

        return Ok(result.Data);
    }

    [Authorize]
    [HttpPut("reply")]
    public async Task<IActionResult> ReplytoInvite([FromBody] AcceptInviteDto dto)
    {
        int requesterId = User.GetUserId();

        var result = await _friendInviteService.RespondToInviteAsync(dto, requesterId);

        if (!result.Success)
        {
            return BadRequest(
                "An error has occured while sending friend request. " + result.ErrorMessage
            );
        }

        return Ok(result.Data);
    }

    [Authorize]
    [HttpGet("sentInvites")]
    public async Task<IActionResult> GetAllSentInvites()
    {
        int requesterId = User.GetUserId();

        var result = await _friendInviteService.GetSentInvitesAsync(requesterId);

        if (!result.Success)
        {
            return BadRequest(
                "An error has occured while sending friend request. " + result.ErrorMessage
            );
        }

        return Ok(result.Data);
    }

    [Authorize]
    [HttpGet("receievedInvites")]
    public async Task<IActionResult> GetAllReceivedInvites()
    {
        int requesterId = User.GetUserId();

        var result = await _friendInviteService.GetReceivedInvitesAsync(requesterId);

        if (!result.Success)
        {
            return BadRequest(
                "An error has occured while sending friend request. " + result.ErrorMessage
            );
        }

        return Ok(result.Data);
    }
}
