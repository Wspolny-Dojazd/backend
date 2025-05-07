using Domain.Model;

namespace Domain.Interfaces;

/// <summary>
/// Defines a contract for <see cref="Message"/> data access operations.
/// </summary>
public interface IMessageRepository
{
    /// <summary>
    /// Retrieves all messages for the specified group.
    /// </summary>
    /// <param name="groupId">The unique identifier of the group.</param>
    /// <returns>The messages for the group.</returns>
    Task<List<Message>> GetAllByGroupIdAsync(int groupId);

    /// <summary>
    /// Adds a new message to the database.
    /// </summary>
    /// <param name="message">The message to add.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task AddAsync(Message message);
}
