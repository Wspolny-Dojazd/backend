namespace Shared.Enums.ErrorCodes.Auth;

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
    /// The provided refresh token is invalid, expired or malformed.
    /// </summary>
    INVALID_REFRESH_TOKEN,

    /// <summary>
    /// The token has expired and is no longer valid.
    /// </summary>
    EXPIRED_TOKEN,

    /// <summary>
    /// The specified user was not found.
    /// </summary>
    USER_NOT_FOUND,

    /// <summary>
    /// The provided current password is incorrect.
    /// </summary>
    /// <remarks>
    /// Used when the user attempts to change their password and the provided current password
    /// does not match the stored password.
    /// </remarks>
    INVALID_CURRENT_PASSWORD,

    /// <summary>
    /// The provided nickname is invalid.
    /// </summary>
    INVALID_NICKNAME,
}
