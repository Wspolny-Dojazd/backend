using System.Security.Claims;
using API.Models.Errors;
using API.Models.Errors.Auth;
using Application.DTOs.Auth;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <summary>
/// Provides authentication endpoints for user login, registration, and profile.
/// </summary>
/// <param name="authService">The service that handles authentication logic.</param>
[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    /// <summary>
    /// Authenticates a user using provided credentials and returns a JWT token.
    /// </summary>
    /// <param name="request">The login request containing email and password.</param>
    /// <returns>The authenticated user's data and a JWT token.</returns>
    /// <response code="200">The user has been authenticated successfully.</response>
    /// <response code="400">The request payload is invalid.</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse<LoginErrorCode>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginRequestDto request)
    {
        if (!this.ModelState.IsValid)
        {
            var hasEmailError = this.ModelState["Email"]?.Errors.Any(e => e.ErrorMessage.Contains("email", StringComparison.OrdinalIgnoreCase)) == true;
            var code = hasEmailError ? LoginErrorCode.INVALID_EMAIL_FORMAT : LoginErrorCode.VALIDATION_ERROR;
            return this.BadRequest(new ErrorResponse(code));
        }

        var result = await authService.LoginAsync(request);
        return this.Ok(result);
    }

    /// <summary>
    /// Registers a new user and returns a JWT token.
    /// </summary>
    /// <param name="request">The registration request containing email, nickname, and password.</param>
    /// <returns>The newly registered user's data and a JWT token.</returns>
    /// <response code="200">The user has been registered successfully.</response>
    /// <response code="400">The request payload is invalid.</response>
    /// <response code="409">The email address is already in use.</response>
    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse<RegisterErrorCode>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse<RegisterErrorCode>), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterRequestDto request)
    {
        if (!this.ModelState.IsValid)
        {
            var hasEmailError = this.ModelState["Email"]?.Errors.Any(e => e.ErrorMessage.Contains("email", StringComparison.OrdinalIgnoreCase)) == true;
            var code = hasEmailError ? LoginErrorCode.INVALID_EMAIL_FORMAT : LoginErrorCode.VALIDATION_ERROR;
            return this.BadRequest(new ErrorResponse(code));
        }

        var result = await authService.RegisterAsync(request);
        return this.Ok(result);
    }

    /// <summary>
    /// Retrieves the currently authenticated user's profile and JWT token.
    /// </summary>
    /// <returns>The authenticated user's data and token.</returns>
    /// <response code="200">Authenticated user found; returns user info and token.</response>
    /// <response code="401">The user is not authenticated.</response>
    [Authorize]
    [HttpGet("me")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse<AuthErrorCode>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse<UserErrorCode>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AuthResponseDto>> Me()
    {
        var userId = int.Parse(this.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await authService.GetMeAsync(userId);
        return this.Ok(result);
    }
}
