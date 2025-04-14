using System;

namespace Application.Exceptions
{
    public class UnauthorizedInvitationActionException : AppException
    {
        public UnauthorizedInvitationActionException() 
            : base(400, "TEST", "You are not authorized to perform this action on the invitation.")
        {
        }
    }
}