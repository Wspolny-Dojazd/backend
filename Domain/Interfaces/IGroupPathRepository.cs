using Domain.Model;

namespace Domain.Interfaces;

/// <summary>
/// Defines a contract for <see cref="GroupPath"/> data access operations.
/// </summary>
public interface IGroupPathRepository
{
    /// <summary>
    /// Adds a new accepted group path to the database.
    /// </summary>
    /// <param name="groupPath">The group path to add.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task AddAsync(GroupPath groupPath);

    /// <summary>
    /// Retrieves the accepted path for the specified group.
    /// </summary>
    /// <param name="groupId">The identifier of the group.</param>
    /// <returns>The accepted group path if found; otherwise, <see langword="null"/>.</returns>
    Task<GroupPath?> GetByGroupIdAsync(int groupId);
}
