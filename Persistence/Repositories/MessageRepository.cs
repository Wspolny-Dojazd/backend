using Domain.Interfaces;
using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

/// <summary>
/// Provides data access operations for <see cref="Message"/> entities.
/// </summary>
/// <param name="databaseContext">The database context used to access message data.</param>
public class MessageRepository(DatabaseContext databaseContext)
    : IMessageRepository
{
    /// <inheritdoc/>
    public async Task<List<Message>> GetAllByGroupIdAsync(int groupId)
    {
        return await databaseContext.Messages
            .Where(m => m.GroupId == groupId)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task AddAsync(Message message)
    {
        _ = await databaseContext.Messages.AddAsync(message);
        _ = await databaseContext.SaveChangesAsync();
    }
}
