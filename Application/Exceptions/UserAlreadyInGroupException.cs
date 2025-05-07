namespace Application.Exceptions;

/// <summary>
/// Represents an exception thrown when a user is already a member of a group.
/// </summary>
/// <param name="groupId">The unique identifier of the group.</param>
/// <param name="userId">The unique identifier of the user.</param>
public class UserAlreadyInGroupException(int groupId, Guid userId)
    : AppException(400, "USER_ALREADY_IN_GROUP", $"The user with ID {userId} is already a member of the group with ID {groupId}.");
