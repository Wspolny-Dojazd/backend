using Domain.Model;

namespace Domain.Interfaces;

/// <summary>
/// Defines a contract for <see cref="User"/> Friends data access operations.
/// </summary>
public interface IFriendRepository
{
    /// <summary>
    /// Retrieves a list of friends by user's unique identifier.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>The friends of the specified user.</returns>
    Task<IEnumerable<User>> GetAllAsync(Guid userId);
}
