using Application.DTOs.Message;

namespace Application.Interfaces;

/// <summary>
/// Defines a contract for the message-related operations.
/// </summary>
public interface IMessageService
{
    /// <summary>
    /// Retrieves all messages for the specified group.
    /// </summary>
    /// <param name="userId">The unique identifier of the user trying to get the messages.</param>
    /// <param name="groupId">The unique identifier of the group.</param>
    /// <returns>The messages for the group with details.</returns>
    Task<IEnumerable<MessageDto>> GetAllByGroupIdAsync(Guid userId, int groupId);

    /// <summary>
    /// Sends a message to the specified group.
    /// </summary>
    /// <param name="userId">The unique identifier of the message author.</param>
    /// <param name="groupId">The unique identifier of the group.</param>
    /// <param name="content">The content of the message.</param>
    /// <returns>The sent message with details.</returns>
    Task<MessageDto> SendAsync(Guid userId, int groupId, string content);
}
