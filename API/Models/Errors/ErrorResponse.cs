namespace API.Models.Errors;

/// <summary>
/// Represents a standardized error response returned by the API.
/// </summary>
/// <param name="Code">The machine-readable error code.</param>
/// <param name="Message">The optional human-readable error message.</param>
public record class ErrorResponse(string Code, string? Message = null);
