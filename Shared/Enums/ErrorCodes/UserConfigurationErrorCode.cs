namespace Shared.Enums.ErrorCodes;

/// <summary>
/// Defines error codes related to user configuration operations, returned in API error responses.
/// </summary>
public enum UserConfigurationErrorCode
{
    /// <summary>
    /// The user configuration was not found.
    /// </summary>
    USER_CONFIGURATION_NOT_FOUND,

    /// <summary>
    /// The user configuration format was invalid.
    /// </summary>
    VALIDATION_ERROR,
}
