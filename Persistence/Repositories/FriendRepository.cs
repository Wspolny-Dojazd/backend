using Domain.Interfaces;
using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

/// <summary>
/// Represents crud operations for friendship relationships.
/// </summary>
/// <param name="databaseContext">The database context used to access group data.</param>
public class FriendRepository(DatabaseContext databaseContext)
    : IFriendRepository
{
    /// <inheritdoc/>
    public async Task<IEnumerable<User>> GetAllAsync(Guid userId)
    {
        var user = await databaseContext.Users
            .FirstOrDefaultAsync(u => u.Id == userId);

        return user?.Friends ?? new List<User>();
    }
}
