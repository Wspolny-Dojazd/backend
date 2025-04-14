// Application/Exceptions/AlreadyFriendsException.cs
using System;

namespace Application.Exceptions
{
    public class AlreadyFriendsException : AppException
    {
        public AlreadyFriendsException() 
            : base(400, "TEST", "You are already friends with this user.")
        {
        }
    }
}