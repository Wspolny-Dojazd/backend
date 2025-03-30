using System.Security.Claims;

namespace API.Extensions;

/// <summary>
/// Provides extension methods for <see cref="ClaimsPrincipal"/>.
/// </summary>
public static class ClaimsPrincipalExtensions
{
    /// <summary>
    /// Retrieves the User ID from the ClaimsPrincipal, parsed from the "NameIdentifier" claim.
    /// </summary>
    /// <param name="claimsPrincipal">The ClaimsPrincipal object representing the authenticated user.</param>
    /// <returns>The User ID as an integer.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the User ID claim is missing or invalid.</exception>
    public static int GetUserId(this ClaimsPrincipal claimsPrincipal)
    {
        ArgumentNullException.ThrowIfNull(claimsPrincipal);

        var userIdClaim = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);

        return string.IsNullOrEmpty(userIdClaim)
            ? throw new InvalidOperationException("User ID claim is missing.")
            : !int.TryParse(userIdClaim, out var userId)
            ? throw new InvalidOperationException("User ID claim is not a valid integer.")
            : userId;
    }
}
