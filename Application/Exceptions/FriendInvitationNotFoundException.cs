using System;

namespace Application.Exceptions
{
    public class FriendInvitationNotFoundException : AppException
    {
        public FriendInvitationNotFoundException() 
            : base(400, "TEST", "Friend invitation not found.")
        {
        }
    }
}