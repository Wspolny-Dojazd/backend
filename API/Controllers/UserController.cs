using Application.DTOs;
using Application.Interfaces;
using Domain.Model;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <summary>
/// Represents controller that defines api user endpoints.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService userService;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserController"/> class.
    /// </summary>
    /// <param name="userService">User service that defines user data methods.</param>
    public UserController(IUserService userService)
    {
        this.userService = userService;
    }

    /// <summary>
    /// Method that defines get user by id endpoint.
    /// </summary>
    /// <param name="id">Unique user's identifier.</param>
    /// <returns>Returns action result object.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUserById(int id)
    {
        return this.Ok(await this.userService.GetUserByIdAsync(id));
    }

    /// <summary>
    /// Method that defines delete user by nickname.
    /// </summary>
    /// <param name="nickname">Unique user's nickname.</param>
    /// <returns>Returns action result object.</returns>
    [HttpDelete]
    public async Task<ActionResult> DeleteUserByNickname(string nickname)
    {
        await this.userService.DeleteUserByNicknameAsync(nickname);
        return this.Ok();
    }

    /// <summary>
    /// Method that defines get users by nickname end point.
    /// </summary>
    /// <param name="value">String by which the users are found.</param>
    /// <returns>Returns action result object.</returns>
    [HttpGet]
    public async Task<ActionResult<List<UserDto>>> GetUsersByNicknameAsync(string value)
    {
        return this.Ok(await this.userService.GetUsersByNicknameAsync(value));
    }
}
