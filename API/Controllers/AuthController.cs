using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <summary>
/// Represents controller that defines user's authentication endpoints.
/// </summary>
[Route("api/[controller]")]
[ApiController]

public class AuthController : ControllerBase
{
    private readonly IAuthService authService;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthController"/> class.
    /// </summary>
    /// <param name="authService">Authorization service that defines user data methods.</param>
    public AuthController(IAuthService authService)
    {
        this.authService = authService;
    }

    /// <summary>
    /// Method that handle logging in.
    /// Checks if the user exists, then calls the user logging in service.
    /// </summary>
    /// <param name="loginUserDto">User's data used to log in.</param>
    /// <returns>Returns action result object.</returns>
    [HttpPost]
    public async Task<ActionResult<LoginUserReturnDto>> LoginAsync(LoginUserDto loginUserDto)
    {
        var user = await this.authService.LoginUserAsync(loginUserDto);
        if (user == null)
        {
            return this.Unauthorized();
        }

        return this.Ok(user);
    }

    /// <summary>
    /// Method that handles a new user registration request.
    /// Checks if the email is unique, then calls the user registration service.
    /// </summary>
    /// <param name="userRegisterData">New user's data.</param>
    /// <returns>Returns action result object.</returns>
    [HttpPost("register")]
    public async Task<ActionResult<LoginUserReturnDto>> Register([FromBody] RegisterUserDto userRegisterData)
    {
        if (!await this.authService.ValidateEmailAsync(userRegisterData.Email))
        {
            return this.BadRequest("Email already exists");
        }

        var user = await this.authService.RegisterUserAsync(userRegisterData);

        return this.Ok(user);
    }
}
