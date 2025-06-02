namespace Shared.Enums.ErrorCodes.Auth;

/// <summary>
/// Defines error codes returned by the login endpoint.
/// </summary>
public enum LoginErrorCode
{
    /// <summary>
    /// The provided credentials are invalid.
    /// </summary>
    INVALID_CREDENTIALS,

    /// <summary>
    /// The provided email is not in a valid format.
    /// </summary>
    INVALID_EMAIL_FORMAT,

    /// <summary>
    /// The request payload is invalid.
    /// </summary>
    VALIDATION_ERROR,
}
