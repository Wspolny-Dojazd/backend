namespace Shared.Enums.ErrorCodes;

/// <summary>
/// Defines error codes for group invitation operations,
/// returned in API error responses.
/// </summary>
public enum GroupInvitationErrorCode
{
    /// <summary>
    /// The invitation was not found.
    /// </summary>
    INVITATION_NOT_FOUND,

    /// <summary>
    /// The group was not found.
    /// </summary>
    GROUP_NOT_FOUND,

    /// <summary>
    /// The recipient user was not found.
    /// </summary>
    RECIPIENT_NOT_FOUND,

    /// <summary>
    /// The user is already in the group.
    /// </summary>
    ALREADY_IN_GROUP,

    /// <summary>
    /// The invitation has already been sent.
    /// </summary>
    ALREADY_SENT,

    /// <summary>
    /// The recipient has already sent an invitation to the sender.
    /// </summary>
    RECIPROCAL_EXISTS,

    /// <summary>
    /// A user cannot invite themselves.
    /// </summary>
    SELF_INVITATION,

    /// <summary>
    /// The user is not authorized to access or modify the invitation.
    /// </summary>
    ACCESS_DENIED,
}