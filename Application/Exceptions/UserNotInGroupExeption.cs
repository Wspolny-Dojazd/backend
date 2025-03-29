namespace Application.Exceptions;

/// <summary>
/// Represents an exception thrown when a user is not found.
/// </summary>
/// <param name="userId">The unique identifier of the user that was not found.</param>
public class UserNotInGroupExeption(int groupId, int userId)
    : AppException(404, "USER_NOT_IN_GROUP", $"The user with ID {userId} is not a member of the group with ID {groupId}.");
