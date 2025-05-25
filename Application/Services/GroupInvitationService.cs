using Application.DTOs;
using Application.DTOs.GroupInvitation;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using Domain.Model;
using Shared.Enums.ErrorCodes;

namespace Application.Services;

/// <summary>
/// Provides operations for managing group invitations.
/// </summary>
/// <param name="invitationRepository">The repository for accessing group invitation data.</param>
/// <param name="groupService">The service for managing user groups.</param>
/// <param name="userRepository">The repository for accessing user data.</param>
/// <param name="userService">The service for managing user data.</param>
/// <param name="mapper">The object mapper.</param>
public class GroupInvitationService(
    IGroupInvitationRepository invitationRepository,
    IGroupService groupService,
    IUserRepository userRepository,
    IUserService userService,
    IMapper mapper)
    : IGroupInvitationService
{
    /// <inheritdoc/>
    public async Task<GroupInvitationDto> SendAsync(Guid senderId, GroupInvitationRequestDto dto)
    {
        if (senderId == dto.UserId)
        {
            throw new AppException(400, GroupInvitationErrorCode.SELF_INVITATION);
        }

        var receiver = await userRepository.GetByIdAsync(dto.UserId)
            ?? throw new AppException(404, GroupInvitationErrorCode.RECIPIENT_NOT_FOUND);

        var sender = await userService.GetEntityByIdAsync(senderId);

        var group = await groupService.GetByIdAsync(dto.GroupId)
            ?? throw new AppException(404, GroupInvitationErrorCode.GROUP_NOT_FOUND);

        if (group.GroupMembers.Any(f => f.Id == dto.UserId))
        {
            throw new AppException(400, GroupInvitationErrorCode.ALREADY_IN_GROUP);
        }

        if (await invitationRepository.ExistsAsync(dto.UserId, senderId, dto.GroupId))
        {
            throw new AppException(400, GroupInvitationErrorCode.RECIPROCAL_EXISTS);
        }

        if (await invitationRepository.ExistsAsync(senderId, dto.UserId, dto.GroupId))
        {
            throw new AppException(400, GroupInvitationErrorCode.ALREADY_SENT);
        }

        var invitation = new GroupInvitation
        {
            Id = Guid.NewGuid(),
            SenderId = senderId,
            ReceiverId = dto.UserId,
            GroupId = dto.GroupId,
            CreatedAt = DateTime.UtcNow,
        };

        await invitationRepository.AddAsync(invitation);
        return mapper.Map<GroupInvitationDto>(invitation);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<GroupInvitationDto>> GetSentAsync(Guid userId)
    {
        var invitations = await invitationRepository.GetSentAsync(userId);
        return mapper.Map<IEnumerable<GroupInvitationDto>>(invitations);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<GroupInvitationDto>> GetReceivedAsync(Guid userId)
    {
        var invitations = await invitationRepository.GetReceivedAsync(userId);
        return mapper.Map<IEnumerable<GroupInvitationDto>>(invitations);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<GroupInvitationDto>> GetAllSentAsync(int groupId)
    {
        var invitations = await invitationRepository.GetAllAsync(groupId);
        return mapper.Map<IEnumerable<GroupInvitationDto>>(invitations);
    }

    /// <inheritdoc/>
    public async Task<GroupDto> AcceptAsync(Guid userId, Guid invitationId)
    {
        var invitation = await invitationRepository.GetByIdAsync(invitationId)
            ?? throw new AppException(404, GroupInvitationErrorCode.INVITATION_NOT_FOUND);

        var group = await groupService.GetByIdAsync(invitation.GroupId)
            ?? throw new AppException(404, GroupInvitationErrorCode.GROUP_NOT_FOUND);

        if (invitation.ReceiverId != userId)
        {
            throw new AppException(403, GroupInvitationErrorCode.ACCESS_DENIED);
        }

        if (!await groupService.IsInGroup(invitation.GroupId, userId))
        {
            await groupService.AddUserByInvitationAsync(invitation.GroupId, userId);
        }

        await invitationRepository.DeleteAsync(invitation);
        await this.RemoveReciprocalInvitationIfExistsAsync(invitation.SenderId, userId, invitation.GroupId);

        return mapper.Map<GroupDto>(group);
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(Guid userId, Guid invitationId)
    {
        var invitation = await invitationRepository.GetByIdAsync(invitationId)
           ?? throw new AppException(404, GroupInvitationErrorCode.INVITATION_NOT_FOUND);

        if (invitation.SenderId == userId)
        {
            // Canceling an invitation
            await invitationRepository.DeleteAsync(invitation);
        }
        else if (invitation.ReceiverId == userId)
        {
            // Declining an invitation
            await invitationRepository.DeleteAsync(invitation);
            await this.RemoveReciprocalInvitationIfExistsAsync(invitation.SenderId, userId, invitation.GroupId);
        }
        else
        {
            throw new AppException(403, GroupInvitationErrorCode.ACCESS_DENIED);
        }
    }

    private async Task RemoveReciprocalInvitationIfExistsAsync(Guid senderId, Guid receiverId, int groupId)
    {
        var invitation = await invitationRepository.GetByUsersAsync(receiverId, senderId, groupId);
        if (invitation is not null)
        {
            await invitationRepository.DeleteAsync(invitation);
        }
    }
}
