namespace Application.Exceptions;

/// <summary>
/// Represents an exception thrown when a friend invitation cannot be found.
/// </summary>
public class InvitationNotFoundException()
    : AppException(404, "INVITATION_NOT_FOUND", "Friend invitation not found.");
