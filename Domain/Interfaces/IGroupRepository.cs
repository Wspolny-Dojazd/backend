using Domain.Model;

namespace Domain.Interfaces;

/// <summary>
/// Defines a contract for <see cref="Group"/> data access operations.
/// </summary>
public interface IGroupRepository
{
    /// <summary>
    /// Retrieves a group by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the group.</param>
    /// <returns>The group if found; otherwise, <see langword="null"/>.</returns>
    Task<Group?> GetGroupByIdAsync(int id);

    /// <summary>
    /// Retrieves a group by its unique identifier.
    /// </summary>
    /// <param name="code">The unique joining code of the group.</param>
    /// <returns>The group if found; otherwise, <see langword="null"/>.</returns>
    Task<Group?> GetGroupByCodeAsync(string code);

    /// <summary>
    /// Creates new group.
    /// </summary>
    /// <returns>The group if created; otherwise, <see langword="null"/>.</returns>
    Task<Group> CreateGroupAsync();

    /// <summary>
    /// Saves changes to database.
    /// </summary>
    void SaveAsync();
}
