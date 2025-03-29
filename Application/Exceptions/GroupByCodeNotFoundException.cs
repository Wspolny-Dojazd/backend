namespace Application.Exceptions;

/// <summary>
/// Represents an exception thrown when a group searched by code is not found.
/// </summary>
/// <param name="code">The unique identifier of the user that was not found.</param>
public class GroupByCodeNotFoundException(string code)
    : AppException(404, "GROUP_NOT_FOUND", $"The group with ID {code} was not found.");
