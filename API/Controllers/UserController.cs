using Application.Interfaces;
using Domain.Model;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <summary>
/// Represents user's API endpoints.
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
}
