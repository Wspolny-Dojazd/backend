namespace Application.Exceptions;

/// <summary>
/// Represents an exception thrown when there is a try to kick a group creator from their group.
/// </summary>
/// <param name="userId">The unique identifier of the user.</param>
/// <param name="groupId">The unique identifier of the group.</param>
public class KickGroupCreatorException(Guid userId, int groupId)
    : AppException(
        403,
        "ACCESS_DENIED",
        $"The user with ID {userId} is the creator of the group with ID {groupId} and cannot be kicked from it.");
