using Application.Exceptions;
using Application.Interfaces;
using Domain.Interfaces;
using Shared.Enums.ErrorCodes;

namespace Application.Services;

/// <summary>
/// Provides authorization checks related to group membership.
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

    public async Task EnsureOwnershipAsync(int groupId, Guid userId)
    {
        _ = await groupRepository.GetByIdAsync(groupId)
            ?? throw new GroupNotFoundException(groupId);

        var isOwner = await groupRepository.IsOwnerAsync(groupId, userId);
        if (!isOwner)
        {
            throw new AppException(
                403,
                GroupErrorCode.ACCESS_DENIED,
                $"The user with ID '{userId}' is not an owner of the group with ID '{groupId}'.");
        }
    }
}
