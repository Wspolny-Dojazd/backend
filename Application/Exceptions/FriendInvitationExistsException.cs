namespace Application.Exceptions;

/// <summary>
/// Exception thrown when attempting to create a friend invitation that already exists.
/// </summary>
/// <remarks>
/// This exception is thrown when a user tries to send a friend invitation to another user
/// when there's already a pending invitation between them.
/// </remarks>
public class FriendInvitationExistsException : AppException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FriendInvitationExistsException"/> class.
    /// </summary>
    public FriendInvitationExistsException()
        : base(400, "INVITATION_ALREADY_EXISTS", "Friend invitation already exists.")
    {
    }
}
