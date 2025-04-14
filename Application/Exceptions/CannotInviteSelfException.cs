using System;

namespace Application.Exceptions
{
    public class CannotInviteSelfException : AppException
    {
        public CannotInviteSelfException() 
            : base(400, "TEST", "You cannot send a friend invitation to yourself.")
        {
        }
    }
}