namespace Application.Exceptions;

/// <summary>
/// Represents an exception thrown when a user attempts to send a friend invitation to someone they are already friends with.
/// </summary>
public class AlreadyFriendsException()
    : AppException(400, "ALREADY_FRIENDS", "You are already friends with this user.");
