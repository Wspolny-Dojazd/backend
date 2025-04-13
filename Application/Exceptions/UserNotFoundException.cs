namespace Application.Exceptions;

/// <summary>
/// Represents an exception thrown when a user is not found.
/// </summary>
/// <param name="userId">The unique identifier of the user that was not found.</param>
public class UserNotFoundException : AppException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserNotFoundException"/> class.
    /// </summary>
    /// <param name="userId">The unique identifier of the user that was not found.</param>
    public UserNotFoundException(Guid userId)
        : this("user unique identifier", userId.ToString())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserNotFoundException"/> class.
    /// </summary>
    /// <param name="userName">The user name of the user that was not found.</param>
    public UserNotFoundException(string userName)
        : this("user name", userName)
    {
    }

    private UserNotFoundException(string identifierType, string identifier)
        : base(404, "USER_NOT_FOUND", $"The user with  {identifierType} '{identifier}' was not found.")
    {
    }
}
