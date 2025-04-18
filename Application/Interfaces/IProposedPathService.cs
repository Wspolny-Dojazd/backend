using Application.DTOs.Path;

namespace Application.Interfaces;

/// <summary>
/// Defines a contract for generating and managing proposed paths for users in a group.
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
    /// Retrieves all currently generated proposed paths for the specified group.
    /// </summary>
    /// <param name="groupId">The identifier of the group.</param>
    /// <returns>The proposed paths for the group.</returns>
    Task<IEnumerable<ProposedPathDto>> GetRangeAsync(int groupId);

    /// <summary>
    /// Removes all proposed paths associated with the specified group.
    /// </summary>
    /// <param name="groupId">The identifier of the group.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task ResetAllForGroupAsync(int groupId);
}
