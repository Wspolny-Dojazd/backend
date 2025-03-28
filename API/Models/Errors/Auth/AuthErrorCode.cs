namespace API.Models.Errors.Auth;

/// <summary>
/// Defines error codes related to authentication operations, returned in API error responses.
/// </summary>
public enum AuthErrorCode
{
    /// <summary>
    /// The authorization token was not provided.
    /// </summary>
    MISSING_TOKEN,

    /// <summary>
    /// The provided token is invalid or malformed.
    /// </summary>
    INVALID_TOKEN,

    /// <summary>
    /// The token has expired and is no longer valid.
    /// </summary>
    EXPIRED_TOKEN,
}
