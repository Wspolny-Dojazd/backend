using System.Security.Claims;
using API.Models.Errors;
using Application.DTOs;
using Application.Interfaces;
using Domain.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <summary>
/// Provides API endpoints for managing and retrieving data related to user configuration.
/// </summary>
/// <param name="userConfigurationService">The user configuration service that handles configuration data operations.</param>
[Route("api/user-configuration")]
[ApiController]
public class UserConfigurationController(IUserConfigurationService userConfigurationService)
    : ControllerBase
{
    /// <summary>
    /// Retrieves a user configuration when authorized.
    /// </summary>
    /// <returns>The user configuration.</returns>
    /// <response code="200">The user configuration was found.</response>
    [Authorize]
    [HttpGet]
    [ProducesResponseType(typeof(UserConfigurationDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<UserConfigurationDto>> Get()
    {
        var userId = int.Parse(this.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var configuration = await userConfigurationService.GetByUserIdAsync(userId);
        return this.Ok(configuration);
    }

    /// <summary>
    /// Updates user configuration.
    /// </summary>
    /// <param name="dto">User configuration fields to update.</param>
    /// <returns>Nothing.</returns>
    /// <response code="200">The update was successful.</response>
    /// <response code="400">User configuration data had invalid format.</response>
    [Authorize]
    [HttpPut]
    [ProducesResponseType(typeof(UserConfigurationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse<UserConfigurationErrorCode>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Put([FromBody] UserConfigurationDto dto)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(new ErrorResponse(UserConfigurationErrorCode.VALIDATION_ERROR, "User Configuration was invalid."));
        }

        var userId = int.Parse(this.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await userConfigurationService.UpdateAsync(userId, dto);
        return this.Ok();
    }
}
