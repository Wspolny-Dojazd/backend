using System;

namespace Application.Exceptions
{
    public class FriendInvitationExistsException : AppException
    {
        public FriendInvitationExistsException() 
            : base(400, "TEST", "Friend invitation already exists.")
        {
        }
    }
}