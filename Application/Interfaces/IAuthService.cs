using Application.DTOs;
using Application.DTOs.Auth;

namespace Application.Interfaces;

/// <summary>
/// Defines a contract for authentication operations.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Authenticates a user with given credentials.
    /// </summary>
    /// <param name="request">The login request.</param>
    /// <returns>The authentication response.</returns>
    Task<AuthResponseDto> LoginAsync(LoginRequestDto request);

    /// <summary>
    /// Registers a new user with given details.
    /// </summary>
    /// <param name="request">The registration request.</param>
    /// <returns>The authentication response.</returns>
    Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request);

    /// <summary>
    /// Returns the authenticated user profile.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>The authenticated user data.</returns>
    Task<UserDto> GetMeAsync(Guid userId);

    /// <summary>
    /// Refreshes the access token using the provided refresh token.
    /// </summary>
    /// <param name="request">The refresh token request.</param>
    /// <returns>The authentication response.</returns>
    Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request);

    /// <summary>
    /// Changes the password of the authenticated user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="request">The change-password request.</param>
    /// <returns>The authentication response.</returns>
    Task<AuthResponseDto> ChangePasswordAsync(Guid userId, ChangePasswordRequestDto request);
}
