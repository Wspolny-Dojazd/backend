namespace Application.Exceptions;

/// <summary>
/// Represents an exception thrown when a user is not a member of a group.
/// </summary>
/// <param name="groupId">The unique identifier of the group.</param>
/// <param name="userId">The unique identifier of the user.</param>
public class UserNotInGroupException(int groupId, int userId)
    : AppException(400, "USER_NOT_IN_GROUP", $"The user with ID {userId} is not a member of the group with ID {groupId}.");
