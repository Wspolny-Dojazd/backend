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
    /// <returns>The authentication response.</returns>
    Task<AuthResponseDto> GetMeAsync(int userId);
}
