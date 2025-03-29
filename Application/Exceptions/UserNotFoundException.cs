namespace Application.Exceptions;

/// <summary>
/// Represents an exception thrown when a user is not found.
/// </summary>
/// <param name="userId">The unique identifier of the user that was not found.</param>
public class UserNotFoundException(int userId)
    : AppException(404, "USER_NOT_FOUND", $"The user with ID {userId} was not found.");
