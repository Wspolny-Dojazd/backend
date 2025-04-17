using Domain.Model;

namespace Domain.Interfaces;

/// <summary>
/// Defines a contract for accessing and managing proposed paths in the database.
/// </summary>
public interface IProposedPathRepository
{
    /// <summary>
    /// Adds multiple proposed paths to the database.
    /// </summary>
    /// <param name="paths">The proposed paths to add.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task AddRangeAsync(IEnumerable<ProposedPath> paths);

    /// <summary>
    /// Retrieves a proposed path by its unique identifier.
    /// </summary>
    /// <param name="id">The identifier of the proposed path.</param>
    /// <returns>The proposed path if found; otherwise, <see langword="null"/>.</returns>
    Task<ProposedPath?> GetByIdAsync(Guid id);

    /// <summary>
    /// Retrieves all proposed paths associated with the specified group.
    /// </summary>
    /// <param name="groupId">The identifier of the group.</param>
    /// <returns>The proposed paths associated with the group.</returns>
    Task<List<ProposedPath>> GetAllByGroupIdAsync(int groupId);

    /// <summary>
    /// Updates an existing proposed path in the database.
    /// </summary>
    /// <param name="proposedPath">The proposed path to update.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task UpdateAsync(ProposedPath proposedPath);

    /// <summary>
    /// Removes the specified proposed paths from the database.
    /// </summary>
    /// <param name="paths">The proposed paths to remove.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task RemoveRangeAsync(IEnumerable<ProposedPath> paths);
}
