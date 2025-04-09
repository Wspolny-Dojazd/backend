namespace API.Models.Errors;

/// <summary>
/// Defines error codes related to user location operations, returned in API error responses.
/// </summary>
public enum UserLocationErrorCode
{
    /// <summary>
    /// The coordinates provided are outside the valid range.
    /// </summary>
    INVALID_COORDINATES,
}
