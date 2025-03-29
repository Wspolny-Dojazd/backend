namespace Application.Exceptions;

/// <summary>
/// Represents an exception thrown when a user is not found.
/// </summary>
/// <param name="userId">The unique identifier of the user that was not found.</param>
public class UserAlreadyInGroupExeption(int groupId, int userId)
    : AppException(404, "USER_ALREADY_IN_GROUP", $"The user with ID {userId} is already a member of the group with ID {groupId}.");
