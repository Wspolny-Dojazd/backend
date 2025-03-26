namespace Application.Exceptions;

/// <summary>
/// Represents the base application-level exception with a custom error code and HTTP status code.
/// </summary>
/// <param name="code">The machine-readable error code.</param>
/// <param name="message">The optional error message associated with the exception.</param>
/// <param name="statusCode">The associated HTTP status code.</param>
public abstract class AppException(string code, string? message, int statusCode)
    : Exception(message)
{
    /// <summary>
    /// Gets the machine-readable error code for the exception.
    /// </summary>
    public string Code { get; } = code;

    /// <summary>
    /// Gets the associated HTTP status code for the exception.
    /// </summary>
    public int StatusCode { get; } = statusCode;
}
