using API.Extensions;
using API.Models.Errors;
using Application.DTOs;
using Application.DTOs.UserLocation;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <summary>
/// Provides API endpoints for managing and retrieving user-related data.
/// </summary>
/// <param name="userService">The user service that handles user data operations.</param>
/// <param name="userLocationService">The service that handles user location operations.</param>
[Route("api/[controller]")]
[ApiController]
public class UsersController(IUserService userService, IUserLocationService userLocationService)
    : ControllerBase
{
    /// <summary>
    /// Retrieves a user by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user to retrieve.</param>
    /// <returns>The user data.</returns>
    /// <response code="200">The user was found.</response>
    /// <response code="404">The user was not found.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse<UserErrorCode>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDto>> GetUserById(Guid id)
    {
        var user = await userService.GetByIdAsync(id);
        return this.Ok(user);
    }

    /// <summary>
    /// Updates the current user's location.
    /// </summary>
    /// <param name="userLocationDto">The location data containing latitude and longitude.</param>
    /// <returns>The updated user location.</returns>
    /// <response code="200">The location was updated successfully.</response>
    /// <response code="400">The request was invalid.</response>
    [HttpPost("me/location")]
    [ProducesResponseType(typeof(UserLocationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse<UserLocationErrorCode>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserLocationDto>> UpdateUserLocation([FromBody] UserLocationRequestDto userLocationDto)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(new ErrorResponse(
                UserLocationErrorCode.INVALID_COORDINATES,
                "Invalid user location data format."));
        }

        var userId = this.User.GetUserId();
        var updatedUserLocation = await userLocationService.UpdateAsync(userId, userLocationDto);
        return this.Ok(updatedUserLocation);
    }
}
