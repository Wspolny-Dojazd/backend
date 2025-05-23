using API.Extensions;
using Application.DTOs;
using Application.DTOs.Auth;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Shared.Enums.ErrorCodes;
using Shared.Enums.ErrorCodes.Auth;

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
                : HasInvalidUsername(this.ModelState) ? RegisterErrorCode.USERNAME_VALIDATION_ERROR :
                RegisterErrorCode.VALIDATION_ERROR;

            return this.BadRequest(new ErrorResponse(code));
        }

        var result = await authService.RegisterAsync(request);
        return this.Ok(result);
    }

    /// <summary>
    /// Retrieves the currently authenticated user's profile.
    /// </summary>
    /// <returns>The authenticated user's data.</returns>
    /// <response code="200">The user profile has been retrieved successfully.</response>
    /// <response code="404">The authenticated user was not found.</response>
    [HttpGet("me")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse<UserErrorCode>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDto>> Me()
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
    /// <responce code="404">The authenticated user was not found.</responce>
    [HttpPost("change-password")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse<AuthErrorCode>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse<AuthErrorCode>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AuthResponseDto>> ChangePassword([FromBody] ChangePasswordRequestDto request)
    {
        var userId = this.User.GetUserId();
        var result = await authService.ChangePasswordAsync(userId, request);
        return this.Ok(result);
    }

    /// <summary>
    /// Changes the nickname of the currently authenticated user.
    /// </summary>
    /// <param name="request">The change-nickname request containing current and new nickname.</param>
    /// <returns>The authenticated user's data and token.</returns>
    /// <response code="200">The user nickname has been changed successfully.</response>
    /// <response code="400">The request payload is invalid.</response>
    /// <responce code="404">The authenticated user was not found.</responce>
    [HttpPost("change-nickname")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse<AuthErrorCode>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse<AuthErrorCode>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AuthResponseDto>> ChangeNickname([FromBody] ChangeNicknameRequestDto request)
    {
        var userId = this.User.GetUserId();
        var result = await authService.ChangeNicknameAsync(userId, request);
        return this.Ok(result);
    }

    /// <summary>
    /// Refreshes the access token using the provided refresh token.
    /// </summary>
    /// <param name="request">The request containing token and refresh token.</param>
    /// <returns>A new access token and user data.</returns>
    /// <response code="200">The token has been refreshed successfully.</response>
    /// <response code="400">The request payload is invalid.</response>
    /// <response code="404">The user was not found.</response>
    [AllowAnonymous]
    [HttpPost("refresh")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse<AuthErrorCode>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse<AuthErrorCode>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AuthResponseDto>> Refresh([FromBody] RefreshTokenRequestDto request)
    {
        var result = await authService.RefreshTokenAsync(request);
        return this.Ok(result);
    }

    private static bool HasInvalidEmail(ModelStateDictionary modelState)
    {
        return modelState["Email"]?.Errors
            .Any(e => e.ErrorMessage.Contains("email", StringComparison.OrdinalIgnoreCase)) == true;
    }

    private static bool HasInvalidUsername(ModelStateDictionary modelState)
    {
        return modelState["Username"]?.Errors
            .Any(e => e.ErrorMessage.Contains("username", StringComparison.OrdinalIgnoreCase)) == true;
    }
}
