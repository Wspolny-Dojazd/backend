using Application.DTOs.Path;

namespace Application.Interfaces;

/// <summary>
/// Defines a contract for managing the accepted travel path of a group.
/// </summary>
public interface IGroupPathService
{
    /// <summary>
    /// Confirms a selected proposed path for the specified group by creating
    /// a new accepted group path and removing all other proposals.
    /// </summary>
    /// <param name="groupId">The identifier of the group.</param>
    /// <param name="proposalId">The identifier of the proposed group path to confirm.</param>
    /// <returns>The confirmed group path.</returns>
    Task<GroupPathDto> ConfirmFromProposalAsync(int groupId, Guid proposalId);

    /// <summary>
    /// Retrieves the accepted path for the specified group.
    /// </summary>
    /// <param name="groupId">The identifier of the group.</param>
    /// <returns>The accepted group path.</returns>
    Task<GroupPathDto> GetByGroupIdAsync(int groupId);
}
