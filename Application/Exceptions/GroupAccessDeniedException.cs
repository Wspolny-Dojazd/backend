namespace Application.Exceptions;

/// <summary>
/// Represents an exception thrown when access to a specified group by a specified user is denied.
/// </summary>
/// <param name="userId">The unique identifier of the user.</param>
/// <param name="groupId">The unique identifier of the group.</param>
public class GroupAccessDeniedException(Guid userId, int groupId)
    : AppException(
        403,
        "ACCESS_DENIED",
        $"The user with ID {userId} does not have access to group with ID {groupId}.");
