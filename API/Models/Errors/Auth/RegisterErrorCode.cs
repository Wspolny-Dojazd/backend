namespace API.Models.Errors.Auth;

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
    /// The provided email is not in a valid format.
    /// </summary>
    INVALID_EMAIL_FORMAT,

    /// <summary>
    /// The request payload is invalid.
    /// </summary>
    VALIDATION_ERROR,
}
