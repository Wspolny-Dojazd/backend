using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Message;

/// <summary>
/// Represents the data transfer object used for sending message by users.
/// </summary>
public class MessagePayloadDto
{
    /// <summary>
    /// Gets the message content.
    /// </summary>
    [Required]
    public required string Content { get; init; }
}
