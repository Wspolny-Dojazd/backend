using Shared.Enums.ErrorCodes;

namespace Application.Exceptions;

/// <summary>
/// Represents an exception thrown when a user attempts to get resources that do not belong to them.
/// </summary>
public class UserNotInGroupException : AppException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserNotInGroupException"/> class.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="groupId">The unique identifier of the group.</param>
    public UserNotInGroupException(Guid userId, int groupId)
        : this("ID", userId.ToString(), groupId.ToString())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserNotInGroupException"/> class.
    /// </summary>
    /// <param name="username">The username of the user.</param>
    /// <param name="groupId">The unique identifier of the group.</param>
    public UserNotInGroupException(string username, int groupId)
        : this("username", username, groupId.ToString())
    {
    }

    private UserNotInGroupException(string userIdentifierType, string userIdentifier, string groupIdentifier)
        : base(
            403,
            GroupErrorCode.ACCESS_DENIED,
            $"The user with {userIdentifierType} '{userIdentifier}' does not belong to the group with ID '{groupIdentifier}'.")
    {
    }
}
