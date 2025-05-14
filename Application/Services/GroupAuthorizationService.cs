using Application.Exceptions;
using Application.Interfaces;
using Domain.Interfaces;
using Shared.Enums.ErrorCodes;

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
            throw new AppException(
                403,
                GroupErrorCode.ACCESS_DENIED,
                $"The user with ID '{userId}' does not belong to the group with ID '{groupId}'.");
        }
    }
}
