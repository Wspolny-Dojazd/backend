using Domain.Interfaces;
using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

/// <summary>
/// Represents crud operations for user.
/// </summary>
/// <param name="databaseContext">The database context used to access user data.</param>
public class UserRepository(DatabaseContext databaseContext)
    : IUserRepository
{
    /// <inheritdoc/>
    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await databaseContext.Users
            .Include(u => u.Friends)
            .Where(u => u.Id == id)
            .FirstOrDefaultAsync();
    }

    /// <inheritdoc/>
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await databaseContext.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    /// <inheritdoc/>
    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await databaseContext.Users
            .FirstOrDefaultAsync(u => u.Username == username);
    }

    /// <inheritdoc/>
    public async Task AddAsync(User user)
    {
        _ = await databaseContext.Users.AddAsync(user);
        _ = await databaseContext.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(User user)
    {
        _ = databaseContext.Users.Update(user);
        _ = await databaseContext.SaveChangesAsync();
    }
    
    public async Task AddFriendAsync(Guid userId1, Guid userId2)
    {
        var user1 = await databaseContext.Users
            .Include(u => u.Friends)
            .FirstOrDefaultAsync(u => u.Id == userId1);
        
        var user2 = await databaseContext.Users
            .Include(u => u.Friends)
            .FirstOrDefaultAsync(u => u.Id == userId2);
        
        if (user1 == null || user2 == null)
            return;
        
        // Initialize if null
        if (user1.Friends == null)
            user1.Friends = new List<User>();
        
        if (user2.Friends == null)
            user2.Friends = new List<User>();
        
        // Add each other as friends
        user1.Friends.Add(user2);
        user2.Friends.Add(user1);
    
        await databaseContext.SaveChangesAsync();
    }
}
