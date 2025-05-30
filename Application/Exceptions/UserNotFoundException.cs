using Shared.Enums.ErrorCodes.Auth;

namespace Application.Exceptions;

/// <summary>
/// Represents an exception thrown when a user is not found.
/// </summary>
public class UserNotFoundException : AppException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserNotFoundException"/> class.
    /// </summary>
    /// <param name="userId">The unique identifier of the user that was not found.</param>
    public UserNotFoundException(Guid userId)
        : this("ID", userId.ToString())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserNotFoundException"/> class.
    /// </summary>
    /// <param name="username">The username of the user that was not found.</param>
    public UserNotFoundException(string username)
        : this("username", username)
    {
    }

    private UserNotFoundException(string identifierType, string identifier)
        : base(404, AuthErrorCode.USER_NOT_FOUND, $"The user with {identifierType} '{identifier}' was not found.")
    {
    }
}
