using API.Models.Errors;
using Application.DTOs;
using Application.Interfaces;
using Domain.Model;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <summary>
/// Provides API endpoints for managing and retrieving user-related data.
/// </summary>
/// <param name="userService">The user service that handles user data operations.</param>
[Route("api/[controller]")]
[ApiController]
public class UsersController(IUserService userService) : ControllerBase
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
    public async Task<ActionResult<UserDto>> GetUserById(int id)
    {
        var user = await userService.GetByIdAsync(id);
        return this.Ok(user);
    }
}
