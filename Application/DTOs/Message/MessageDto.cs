using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Message;

/// <summary>
/// Represents the data transfer object used for returning message data in API responses.
/// </summary>
/// <param name="Id">The unique identifier of the message.</param>
/// <param name="UserId">The unique identifier of the message author.</param>
/// <param name="Content">The content of the message.</param>
/// <param name="CreatedAt">The time the message was sent.</param>
public record class MessageDto(
    [property: Required] int Id,
    [property: Required] Guid UserId,
    [property: Required] string Content,
    [property: Required] DateTime CreatedAt);
