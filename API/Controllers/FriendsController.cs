using API.Extensions;
using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Enums.ErrorCodes;

namespace API.Controllers;

/// <summary>
/// Provides API endpoints for managing users' friendship relationships.
/// </summary>
/// <param name="friendService">The service that handles friend-related logic.</param>
// [Route("api/[controller]")]
[ApiController]
public class FriendsController(IFriendService friendService)
    : ControllerBase
{
    /// <summary>
    /// Retrieves a friend list for currently logged in user.
    /// </summary>
    /// <returns>The requested friend list details.</returns>
    /// <response code="200">The friend list was successfully retrieved.</response>
    /// <response code="404">The friend list was not found.</response>
    [HttpGet]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse<UserErrorCode>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAllFriends()
    {
        var userId = this.User.GetUserId();

        var friends = await friendService.GetAllAsync(userId);
        return this.Ok(friends);
    }
}
