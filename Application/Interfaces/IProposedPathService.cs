using Application.DTOs.Path;

namespace Application.Interfaces;

/// <summary>
/// Defines operations for generating and managing proposed paths.
/// </summary>
public interface IProposedPathService
{
    /// <summary>
    /// Generates multiple proposed paths for users in the specified group.
    /// </summary>
    /// <param name="groupId">The identifier of the group for which the paths are generated.</param>
    /// <param name="request">The request containing destination and user locations.</param>
    /// <returns>The proposed paths for the group.</returns>
    Task<IEnumerable<ProposedPathDto>> GenerateAsync(int groupId, PathRequestDto request);

    /// <summary>
    /// Retrieves all proposed paths for the specified group.
    /// </summary>
    /// <param name="groupId">The identifier of the group.</param>
    /// <returns>The proposed paths for the group.</returns>
    Task<IEnumerable<ProposedPathDto>> GetRangeAsync(int groupId);

    /// <summary>
    /// Rejects and removes all proposed paths for the specified group.
    /// </summary>
    /// <param name="groupId">The identifier of the group.</param>
    /// <returns>A <see langword="Task"/> representing the asynchronous operation.</returns>
    /// <remarks>
    /// This operation removes all proposed paths
    /// from the database for the specified group.
    /// </remarks>
    Task RejectAllAsync(int groupId);
}
