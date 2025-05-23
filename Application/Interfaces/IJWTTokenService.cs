﻿using System.Security.Claims;
using Domain.Model;

namespace Application.Interfaces;

/// <summary>
/// Defines a contract for JWT token-related operations.
/// </summary>
public interface IJWTTokenService
{
    /// <summary>
    /// Generates a signed JWT token that represents the specified user identity.
    /// </summary>
    /// <param name="user">The user for whom to generate the token.</param>
    /// <returns>A signed JWT token representing the user's identity.</returns>
    string GenerateToken(User user);

    /// <summary>
    /// Extracts user identity claims from a JWT token.
    /// </summary>
    /// <param name="token">The JWT token.</param>
    /// <returns>The claims principal extracted from the token.</returns>
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}
