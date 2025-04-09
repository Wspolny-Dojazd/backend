namespace Application.Exceptions;

/// <summary>
/// Represents an exception thrown when the provided message is empty.
/// </summary>
/// <param name="userId">The unique identifier of the message author.</param>
public class EmptyMessageException(Guid userId)
    : AppException(400, "EMPTY_MESSAGE", $"The message from the user with ID {userId} is empty.");
