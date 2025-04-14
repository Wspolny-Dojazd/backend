namespace Application.Exceptions;

/// <summary>
/// Exception thrown when a user attempts to perform an action on a friend invitation
/// that they are not authorized to perform.
/// </summary>
/// <remarks>
/// This exception is typically thrown when:
/// - A user tries to accept or decline an invitation they didn't receive
/// - A user tries to cancel an invitation they didn't send.
/// </remarks>
public class UnauthorizedInvitationActionException : AppException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UnauthorizedInvitationActionException"/> class.
    /// </summary>
    public UnauthorizedInvitationActionException()
        : base(400, "UNAUTHORIZED_ACTION", "You are not authorized to perform this action on the invitation.")
    {
    }
}
