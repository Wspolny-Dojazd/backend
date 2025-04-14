namespace Application.Exceptions;

/// <summary>
/// Exception thrown when a friend invitation could not be found in the system.
/// </summary>
/// <remarks>
/// This exception is typically thrown when attempting to access, accept, decline,
/// or cancel an invitation that doesn't exist or has already been processed.
/// </remarks>
public class FriendInvitationNotFoundException : AppException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FriendInvitationNotFoundException"/> class.
    /// </summary>
    public FriendInvitationNotFoundException()
        : base(400, "INVITATION_NOT_FOUND", "Friend invitation not found.")
    {
    }
}
