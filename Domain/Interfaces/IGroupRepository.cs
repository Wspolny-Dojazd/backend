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
    /// Creates new group.
    /// </summary>
    /// <returns>The group if created; otherwise, <see langword="null"/>.</returns>
    Task<Group> CreateGroupAsync();

    /// <summary>
    /// Adds the user to the group.
    /// </summary>
    /// <param name="code">The unique joining code to the group.</param>
    /// <param name="userId">The unique identifier of the group.</param>
    /// <returns>The group if user joined; otherwise, <see langword="null"/>.</returns>
    Task<Group?> AddUserViaCodeAsync(string code, int userId);

    /// <summary>
    /// Removes the user from the group.
    /// </summary>
    /// <param name="id">The unique identifier of the group.</param>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>The user if found; otherwise, <see langword="null"/>.</returns>
    Task<Group?> RemoveUserFromGroupAsync(int id, int userId);
}
