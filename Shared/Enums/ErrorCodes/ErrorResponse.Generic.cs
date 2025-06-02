using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Shared.Enums.ErrorCodes;

/// <summary>
/// Represents a strongly-typed error response containing a machine-readable error code.
/// </summary>
/// <typeparam name="TCode">The type of the machine-readable error code.</typeparam>
/// <param name="code">The machine-readable error code.</param>
/// <param name="message">The optional human-readable error message.</param>
public class ErrorResponse<TCode>(TCode code, string? message = null)
    : ErrorResponse(code, message)
    where TCode : Enum
{
    /// <summary>
    /// Gets or sets the machine-readable error code.
    /// </summary>
    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public new TCode Code { get; set; } = code;
}
