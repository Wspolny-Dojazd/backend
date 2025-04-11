namespace Application.Exceptions;

/// <summary>
/// Represents an exception thrown when a user is not authorized to accept a friend invitation.
/// </summary>
public class NotAuthorizedToAcceptInvitationException()
    : AppException(403, "NOT_AUTHORIZED_TO_ACCEPT", "You are not authorized to accept this friend invitation.");
