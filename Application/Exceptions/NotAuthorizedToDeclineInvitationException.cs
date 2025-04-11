namespace Application.Exceptions;

/// <summary>
/// Represents an exception thrown when a user is not authorized to decline a friend invitation.
/// </summary>
public class NotAuthorizedToDeclineInvitationException()
    : AppException(403, "NOT_AUTHORIZED_TO_DECLINE", "You are not authorized to decline this friend invitation.");
