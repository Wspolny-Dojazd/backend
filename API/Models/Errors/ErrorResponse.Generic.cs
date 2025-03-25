namespace API.Models.Errors;

/// <summary>
/// Represents a typed error response used for API documentation and Swagger schema generation.
/// </summary>
/// <typeparam name="TCode">The type of the machine-readable error code.</typeparam>
/// <param name="Code">The machine-readable error code.</param>
/// <param name="Message">The optional human-readable error message.</param>
public record class ErrorResponse<TCode>(TCode Code, string? Message = null)
    where TCode : Enum;
