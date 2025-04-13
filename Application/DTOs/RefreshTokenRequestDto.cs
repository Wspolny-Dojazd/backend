using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

/// <summary>
/// Represents the data transfer object used for refresh token information.
/// </summary>
public class RefreshTokenRequestDto
{
    /// <summary>
    /// Gets the access token.
    /// </summary>
    [Required]
    public required string Token { get; init; }

    /// <summary>
    /// Gets the refresh token.
    /// </summary>
    [Required]
    public required string RefreshToken { get; init; }
}
