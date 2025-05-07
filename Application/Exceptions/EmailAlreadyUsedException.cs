namespace Application.Exceptions;

/// <summary>
/// Represents an exception thrown when the provided email is already in use.
/// </summary>
public class EmailAlreadyUsedException()
    : AppException(409, "EMAIL_ALREADY_USED", "Email is already registered.");
