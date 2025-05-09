namespace Shared.Enums.ErrorCodes.Auth;

/// <summary>
/// Defines error codes returned by the registration endpoint.
/// </summary>
public enum RegisterErrorCode
{
    /// <summary>
    /// The provided email is already in use.
    /// </summary>
    EMAIL_ALREADY_USED,

    /// <summary>
    /// The provided username is already in use.
    /// </summary>
    USERNAME_ALREADY_USED,

    /// <summary>
    /// The provided username is not in a valid format.
    /// </summary>
    USERNAME_VALIDATION_ERROR,

    /// <summary>
    /// The provided email is not in a valid format.
    /// </summary>
    INVALID_EMAIL_FORMAT,

    /// <summary>
    /// The request payload is invalid.
    /// </summary>
    VALIDATION_ERROR,

    /// <summary>
    /// The provided username is reserved and cannot be used.
    /// </summary>
    USERNAME_RESERVED,
}
