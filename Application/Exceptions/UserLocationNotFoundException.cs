namespace Application.Exceptions;

/// <summary>
/// Represents an exception thrown when a user location is not found.
/// </summary>
/// <param name="userId">The unique identifier of the user whose location was not found.</param>
public class UserLocationNotFoundException(Guid userId)
    : AppException(404, "LOCATION_NOT_FOUND", $"The location with ID {userId} was not found.");
