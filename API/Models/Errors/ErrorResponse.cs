namespace API.Models.Errors;

/// <summary>
/// Represents a standardized error response returned by the API.
/// </summary>
/// <param name="Code">The machine-readable error code.</param>
/// <param name="Message">The optional human-readable error message.</param>
public class ErrorResponse
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorResponse"/> class.
    /// </summary>
    /// <param name="code">The machine-readable error code.</param>
    /// <param name="message">The optional human-readable error message.</param>
    public ErrorResponse(string code, string? message = null)
    {
        this.Code = code;
        this.Message = message;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorResponse"/> class.
    /// </summary>
    /// <param name="code">The machine-readable error code.</param>
    /// <param name="message">The optional human-readable error message.</param>
    public ErrorResponse(Enum code, string? message = null)
    {
        this.Code = code.ToString();
        this.Message = message;
    }

    /// <summary>
    /// Gets or sets the machine-readable error code.
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Gets or sets the human-readable error message.
    /// </summary>
    public string? Message { get; set; }
}
