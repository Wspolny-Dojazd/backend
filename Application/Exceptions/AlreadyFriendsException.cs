namespace Application.Exceptions;

/// <summary>
/// Exception thrown when a user attempts to send a friend invitation to someone
/// who is already their friend.
/// </summary>
/// <remarks>
/// This exception prevents duplicate friendship relationships from being created
/// and ensures that the friend invitation system is only used for creating new friendships.
/// </remarks>
public class AlreadyFriendsException : AppException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AlreadyFriendsException"/> class.
    /// </summary>
    public AlreadyFriendsException()
        : base(400, "ALREADY_FRIENDS", "You are already friends with this user.")
    {
    }
}
