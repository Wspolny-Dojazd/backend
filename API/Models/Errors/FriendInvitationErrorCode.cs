// API/Models/Errors/FriendInvitationErrorCode.cs
namespace API.Models.Errors
{
    public enum FriendInvitationErrorCode
    {
        INVITATION_NOT_FOUND,
        USER_NOT_FOUND,
        CANNOT_INVITE_SELF,
        ALREADY_FRIENDS,
        INVITATION_ALREADY_EXISTS,
        UNAUTHORIZED_ACTION,
    }
}