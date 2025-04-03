using API.Extensions;
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
[Route("api/[controller]")]
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
        var userId = this.User.GetUserId();
        var configuration = await userConfigurationService.GetByUserIdAsync(userId);
        return this.Ok(configuration);
    }

    /// <summary>
    /// Updates user configuration.
    /// </summary>
    /// <param name="dto">The user configuration fields to update with.</param>
    /// <returns>The updated user configuration.</returns>
    /// <response code="200">The user configuration was updated successfully.</response>
    /// <response code="400">The user configuration data had invalid format.</response>
    [HttpPut]
    [ProducesResponseType(typeof(UserConfigurationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse<UserConfigurationErrorCode>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Put([FromBody] UserConfigurationDto dto)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(new ErrorResponse(
                UserConfigurationErrorCode.VALIDATION_ERROR,
                "Invalid user configuration data format."));
        }

        var userId = this.User.GetUserId();
        var updatedDto = await userConfigurationService.UpdateAsync(userId, dto);
        return this.Ok(updatedDto);
    }
}
