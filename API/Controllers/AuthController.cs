using API.Extensions;
using API.Models.Errors;
using API.Models.Errors.Auth;
using Application.DTOs.Auth;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse<LoginErrorCode>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginRequestDto request)
    {
        if (!this.ModelState.IsValid)
        {
            var code = HasInvalidEmail(this.ModelState)
                ? LoginErrorCode.INVALID_EMAIL_FORMAT
                : LoginErrorCode.VALIDATION_ERROR;

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
    [AllowAnonymous]
    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse<RegisterErrorCode>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse<RegisterErrorCode>), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterRequestDto request)
    {
        if (!this.ModelState.IsValid)
        {
            var code = HasInvalidEmail(this.ModelState)
                ? RegisterErrorCode.INVALID_EMAIL_FORMAT
                : RegisterErrorCode.VALIDATION_ERROR;

            return this.BadRequest(new ErrorResponse(code));
        }

        var result = await authService.RegisterAsync(request);
        return this.Ok(result);
    }

    /// <summary>
    /// Retrieves the currently authenticated user's profile and JWT token.
    /// </summary>
    /// <returns>The authenticated user's data and token.</returns>
    /// <response code="200">The user profile has been retrieved successfully.</response>
    /// <response code="404">The authenticated user was not found.</response>
    [HttpGet("me")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse<UserErrorCode>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AuthResponseDto>> Me()
    {
        var userId = this.User.GetUserId();
        var result = await authService.GetMeAsync(userId);
        return this.Ok(result);
    }

    /// <summary>
    /// Changes the password of the currently authenticated user.
    /// </summary>
    /// <param name="request">The change-password request containing current and new password.</param>
    /// <returns>The authenticated user's data and token.</returns>
    /// <response code="200">The user password has been changed successfully.</response>
    /// <response code="400">The request payload is invalid.</response>
    [HttpPost("change-password")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse<AuthErrorCode>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse<AuthErrorCode>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AuthResponseDto>> ChangePassword([FromBody] ChangePasswordRequestDto request)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(new ErrorResponse(AuthErrorCode.INVALID_CURRENT_PASSWORD));
        }

        var userId = this.User.GetUserId();
        var result = await authService.ChangePasswordAsync(userId, request);
        return this.Ok(result);
    }

    private static bool HasInvalidEmail(ModelStateDictionary modelState)
    {
        return modelState["Email"]?.Errors
            .Any(e => e.ErrorMessage.Contains("email", StringComparison.OrdinalIgnoreCase)) == true;
    }
}
