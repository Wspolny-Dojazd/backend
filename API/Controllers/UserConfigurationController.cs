using System.Security.Claims;
using API.Models.Errors;
using Application.DTOs;
using Application.Interfaces;
using Domain.Model;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <summary>
/// Provides API endpoints for managing and retrieving data related to a user configuration.
/// </summary>
/// <param name="userConfigurationService">The user configuration service that handles configuration data operations.</param>
[Route("api/user-configuration")]
[ApiController]
public class UserConfigurationController(IUserConfigurationService userConfigurationService)
    : ControllerBase
{
    /// <summary>
    /// Retrieves a user configuration.
    /// </summary>
    /// <returns>The user configuration.</returns>
    /// <response code="200">The user configuration was found.</response>
    /// <response code="404">The user configuration was not found.</response>
    [HttpGet]
    [ProducesResponseType(typeof(UserConfigurationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse<UserConfigurationErrorCode>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserConfigurationDto>> Get()
    {
        var userId = int.Parse(this.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var configuration = await userConfigurationService.GetByUserIdAsync(userId);
        return this.Ok(configuration);
    }

    /// <summary>
    /// Updates user configuration.
    /// </summary>
    /// <param name="dto">The user configuration fields to update with.</param>
    /// <returns>An <see cref="ActionResult"/> representing the result of the operation.</returns>
    /// <response code="204">The user configuration was updated successfully.</response>
    /// <response code="400">The user configuration data had invalid format.</response>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse<UserConfigurationErrorCode>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Put([FromBody] UserConfigurationDto dto)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(new ErrorResponse(UserConfigurationErrorCode.VALIDATION_ERROR, "User Configuration was invalid."));
        }

        var userId = int.Parse(this.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await userConfigurationService.UpdateAsync(userId, dto);
        return this.NoContent();
    }
}
