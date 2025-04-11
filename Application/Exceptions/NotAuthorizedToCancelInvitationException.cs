namespace Application.Exceptions;

/// <summary>
/// Represents an exception thrown when a user is not authorized to cancel a friend invitation.
/// </summary>
public class NotAuthorizedToCancelInvitationException()
    : AppException(403, "NOT_AUTHORIZED_TO_CANCEL", "You are not authorized to cancel this friend invitation.");
