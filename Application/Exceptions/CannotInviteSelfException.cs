namespace Application.Exceptions;

/// <summary>
/// Represents an exception thrown when a user attempts to send a friend invitation to themselves.
/// </summary>
public class CannotInviteSelfException()
    : AppException(400, "CANNOT_INVITE_SELF", "You cannot send a friend invitation to yourself.");
