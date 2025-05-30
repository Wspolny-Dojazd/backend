namespace Shared.Enums.ErrorCodes;

/// <summary>
/// Defines error codes related to group path operations,
/// returned in API error responses.
/// </summary>
public enum GroupPathErrorCode
{
    /// <summary>
    /// The group was not found.
    /// </summary>
    GROUP_NOT_FOUND,

    /// <summary>
    /// The path was not found.
    /// </summary>
    PATH_NOT_FOUND,

    /// <summary>
    /// The path is not in the group.
    /// </summary>
    PATH_NOT_IN_GROUP,

    /// <summary>
    /// The path is already accepted.
    /// </summary>
    PATH_ALREADY_ACCEPTED,

    /// <summary>
    /// The specified user does not have access to group.
    /// </summary>
    ACCESS_DENIED,
}