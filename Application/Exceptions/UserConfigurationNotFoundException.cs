namespace Application.Exceptions;

/// <summary>
/// Represents an exception thrown when a user configuration is not found.
/// </summary>
/// <param name="userId">The unique identifier of the user whose configuration was not found.</param>
public class UserConfigurationNotFoundException(int userId)
    : AppException(404, "USER_CONFIGURATION_NOT_FOUND", $"The user's configuration data with user ID {userId} was not found.");
