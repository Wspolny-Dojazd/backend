namespace Application.Exceptions;

/// <summary>
/// Represents an exception thrown when a user is not found.
/// </summary>
/// <param name="userId">The unique identifier of the user that was not found.</param>
public class GroupNotFoundException(int groupId)
    : AppException(404, "GROUP_NOT_FOUND", $"The group with ID {groupId} was not found.");
