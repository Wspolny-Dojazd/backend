using System.Security.Claims;
using API.Models.Errors;
using Application.DTOs;
using Application.Interfaces;
using Domain.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <summary>
/// Provides API endpoints for managing and retrieving user-related data.
/// </summary>
/// <param name="userConfigurationService">The user service that handles user data operations.</param>
[Route("api/user-configuration")]
[ApiController]
public class UserConfigurationController(IUserConfigurationService userConfigurationService)
    : ControllerBase
{
    /// <summary>
    /// Retrieves a user by their unique identifier.
    /// </summary>
    /// <param name="userId">The unique identifier of the user to retrieve.</param>
    /// <returns>The user data.</returns>
    /// <response code="200">The user was found.</response>
    /// <response code="404">The user was not found.</response>
    [Authorize]
    [HttpGet]
    [ProducesResponseType(typeof(UserConfigurationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UserConfigurationDto), StatusCodes.Status401Unauthorized)] // TODO change typeof to ErrorResponse
    [ProducesResponseType(typeof(ErrorResponse<UserErrorCode>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserConfigurationDto>> Get()
    {
        var userId = int.Parse(this.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var configuration = await userConfigurationService.GetByUserIdAsync(userId);
        return this.Ok(configuration);
    }

    /// <summary>
    /// Retrieves a user by their unique identifier.
    /// </summary>
    /// <param name="dto">The unique identifier of the user to retrieve.</param>
    /// <returns>The user data.</returns>
    /// <response code="200">The user was found.</response>
    /// <response code="404">The user was not found.</response>
    [Authorize]
    [HttpPut]
    [ProducesResponseType(typeof(UserConfigurationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UserConfigurationDto), StatusCodes.Status401Unauthorized)] // TODO change typeof to ErrorResponse
    public async Task<ActionResult> Put([FromBody] UserConfigurationDto dto)
    {
        var userId = int.Parse(this.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await userConfigurationService.UpdateAsync(userId, dto);
        return this.Ok();
    }
}
