using Application.Exceptions;
using Application.Interfaces;
using Domain.Interfaces;

namespace Application.Services;

/// <summary>
/// Provides services to ensure that a user is a member of a specific group.
/// </summary>
/// <param name="groupRepository">The repository for accessing group data.</param>
public class GroupAuthorizationService(IGroupRepository groupRepository) : IGroupAuthorizationService
{
    /// <inheritdoc/>
    public async Task EnsureMembershipAsync(int groupId, Guid userId)
    {
        _ = await groupRepository.GetByIdAsync(groupId)
            ?? throw new GroupNotFoundException(groupId);

        var isMember = await groupRepository.HasMemberAsync(groupId, userId);
        if (!isMember)
        {
            throw new UserNotInGroupException(userId, groupId);
        }
    }
}
