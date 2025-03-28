namespace Application.Exceptions;

/// <summary>
/// Represents an exception thrown when a user is not found.
/// </summary>
/// <param name="userId">The unique identifier of the user that was not found.</param>
public class RemovingUserFromGroupFailedExeption(int userID, int groupId)
    : AppException(404, "REMOVING_USER_FAILED", $"The user with ID {userID} could not be rmoved form group with ID {groupId}.");
