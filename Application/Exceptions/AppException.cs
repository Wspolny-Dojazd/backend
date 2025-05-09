namespace Application.Exceptions;

/// <summary>
/// Represents the base application-level exception with a custom error code and HTTP status code.
/// </summary>
/// <param name="statusCode">The associated HTTP status code.</param>
/// <param name="code">The machine-readable error code.</param>
/// <param name="message">The error message associated with the exception.</param>
public class AppException(int statusCode, Enum code, string message = "")
    : Exception(message)
{
    /// <summary>
    /// Gets the associated HTTP status code for the exception.
    /// </summary>
    public int StatusCode { get; } = statusCode;

    /// <summary>
    /// Gets the machine-readable error code for the exception.
    /// </summary>
    public Enum Code { get; } = code;
}
