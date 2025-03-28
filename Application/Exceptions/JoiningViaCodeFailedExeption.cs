namespace Application.Exceptions;

/// <summary>
/// Represents an exception thrown when a user is not found.
/// </summary>
/// <param name="userId">The unique identifier of the user that was not found.</param>
public class JoiningViaCodeFailedExeption(int userId, string code)
    : AppException(404, "JOINING_VIA_CODE_FAILED", $"The user with Id {userId} could not be added to the group via code: {code}");
