namespace API.Models.Errors;

/// <summary>
/// Error codes for friend invitation operations that identify specific error conditions.
/// These codes are used in API responses when friend invitation operations fail.
/// </summary>
public enum FriendInvitationErrorCode
{
    /// <summary>
    /// Indicates that the specified invitation could not be found in the system.
    /// Typically returned when trying to accept, decline, or cancel a non-existent invitation.
    /// </summary>
    INVITATION_NOT_FOUND,

    /// <summary>
    /// Indicates that the specified user could not be found.
    /// Typically returned when trying to send an invitation to a non-existent user.
    /// </summary>
    USER_NOT_FOUND,

    /// <summary>
    /// Indicates that a user attempted to send a friend invitation to themselves,
    /// which is not allowed by the system.
    /// </summary>
    CANNOT_INVITE_SELF,

    /// <summary>
    /// Indicates that a user attempted to send a friend invitation to someone
    /// who is already their friend.
    /// </summary>
    ALREADY_FRIENDS,

    /// <summary>
    /// Indicates that an invitation already exists between the specified users.
    /// Prevents duplicate invitations from being created.
    /// </summary>
    INVITATION_ALREADY_EXISTS,

    /// <summary>
    /// Indicates that a user attempted to perform an operation on an invitation
    /// that they are not authorized to perform, such as accepting/declining an invitation
    /// they didn't receive, or canceling an invitation they didn't send.
    /// </summary>
    UNAUTHORIZED_ACTION,
}