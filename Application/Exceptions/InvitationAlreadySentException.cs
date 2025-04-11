namespace Application.Exceptions;

/// <summary>
/// Represents an exception thrown when a friend invitation has already been sent between users.
/// </summary>
public class InvitationAlreadySentException()
    : AppException(400, "INVITATION_ALREADY_SENT", "A friend invitation has already been sent.");
