namespace Application.Exceptions;

/// <summary>
/// Represents an exception thrown when the provided credentials are invalid.
/// </summary>
public class InvalidCredentialsException()
    : AppException(400, "INVALID_CREDENTIALS", "Invalid email or password.");
