namespace Application.Exceptions;

/// <summary>
/// Represents an exception thrown when a group searched by id is not found.
/// </summary>
/// <param name="userId">The unique identifier of the user that was not found.</param>
public class GroupByIdNotFoundException(int groupId)
    : AppException(404, "GROUP_NOT_FOUND", $"The group with ID {groupId} was not found.");
