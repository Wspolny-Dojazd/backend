namespace API.Models.Errors;

/// <summary>
/// Defines error codes related to group operations, returned in API error responses.
/// </summary>
public enum GroupErrorCode
{
    /// <summary>
    /// The group was not found.
    /// </summary>
    GROUP_NOT_FOUND,

    /// <summary>
    /// The specified user was not found.
    /// </summary>
    USER_NOT_FOUND,

    /// <summary>
    /// The specified user is already a member.
    /// </summary>
    USER_ALREADY_IN_GROUP,

    /// <summary>
    /// The specified user is not a member.
    /// </summary>
    USER_NOT_IN_GROUP,
}
