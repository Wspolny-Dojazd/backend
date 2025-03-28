namespace API.Models.Errors;

/// <summary>
/// Defines error codes related to user operations, returned in API error responses.
/// </summary>
public enum GroupErrorCode
{
    /// <summary>
    /// The specified user was not found.
    /// </summary>
    GROUP_NOT_FOUND,

    /// <summary>
    /// Failed to add user via joining code.
    /// </summary>
    JOINING_VIA_CODE_FAILED,

    /// <summary>
    /// Failed to remove user from the group.
    /// </summary>
    REMOVING_USER_FAILED,
}
