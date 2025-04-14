namespace Application.Exceptions;

/// <summary>
/// Exception thrown when a user attempts to send a friend invitation to themselves.
/// </summary>
/// <remarks>
/// This exception enforces the business rule that users cannot send friend invitations
/// to their own account.
/// </remarks>
public class CannotInviteSelfException : AppException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CannotInviteSelfException"/> class.
    /// </summary>
    public CannotInviteSelfException()
        : base(400, "CANNOT_INVITE_SELF", "You cannot send a friend invitation to yourself.")
    {
    }
}
