﻿using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Auth;

/// <summary>
/// Represents the data transfer object used for user authentication responses.
/// </summary>
/// <param name="Id">The unique identifier of the user.</param>
/// <param name="Username">The username of the user.</param>
/// <param name="Nickname">The nickname of the user.</param>
/// <param name="Email">The email address of the user.</param>
/// <param name="Token">The access token.</param>
/// <param name="RefreshToken">The refresh token.</param>
public record AuthResponseDto(
    [property: Required] Guid Id,
    [property: Required] string Username,
    [property: Required] string Nickname,
    [property: Required] string Email,
    [property: Required] string Token,
    [property: Required] string RefreshToken);
