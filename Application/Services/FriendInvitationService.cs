using Application.DTOs.FriendInvitation;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using Domain.Model;
using Shared.Enums.ErrorCodes;

namespace Application.Services;

/// <summary>
/// Provides operations for managing friend invitations.
/// </summary>
/// <param name="invitationRepository">The repository for accessing friend invitation data.</param>
/// <param name="friendService">The service for managing user friendships.</param>
/// <param name="userRepository">The repository for accessing user data.</param>
/// <param name="userService">The service for managing user data.</param>
/// <param name="mapper">The object mapper.</param>
public class FriendInvitationService(
    IFriendInvitationRepository invitationRepository,
    IFriendService friendService,
    IUserRepository userRepository,
    IUserService userService,
    IMapper mapper)
    : IFriendInvitationService
{
    /// <inheritdoc/>
    public async Task<FriendInvitationDto> SendAsync(Guid senderId, FriendInvitationRequestDto dto)
    {
        if (senderId == dto.UserId)
        {
            throw new AppException(400, FriendInvitationErrorCode.SELF_INVITATION);
        }

        var receiver = await userRepository.GetByIdAsync(dto.UserId)
            ?? throw new AppException(404, FriendInvitationErrorCode.RECIPIENT_NOT_FOUND);

        var sender = await userService.GetEntityByIdAsync(senderId);
        if (sender.Friends.Any(f => f.Id == dto.UserId))
        {
            throw new AppException(400, FriendInvitationErrorCode.ALREADY_FRIEND);
        }

        if (await invitationRepository.ExistsAsync(dto.UserId, senderId))
        {
            throw new AppException(400, FriendInvitationErrorCode.RECIPROCAL_EXISTS);
        }

        if (await invitationRepository.ExistsAsync(senderId, dto.UserId))
        {
            throw new AppException(400, FriendInvitationErrorCode.ALREADY_SENT);
        }

        var invitation = new FriendInvitation
        {
            Id = Guid.NewGuid(),
            SenderId = senderId,
            ReceiverId = dto.UserId,
            CreatedAt = DateTime.UtcNow,
        };

        await invitationRepository.AddAsync(invitation);
        return mapper.Map<FriendInvitationDto>(invitation);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<FriendInvitationDto>> GetSentAsync(Guid userId)
    {
        var invitations = await invitationRepository.GetSentAsync(userId);
        return mapper.Map<IEnumerable<FriendInvitationDto>>(invitations);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<FriendInvitationDto>> GetReceivedAsync(Guid userId)
    {
        var invitations = await invitationRepository.GetReceivedAsync(userId);
        return mapper.Map<IEnumerable<FriendInvitationDto>>(invitations);
    }

    /// <inheritdoc/>
    public async Task AcceptAsync(Guid userId, Guid invitationId)
    {
        var invitation = await invitationRepository.GetByIdAsync(invitationId)
            ?? throw new AppException(404, FriendInvitationErrorCode.INVITATION_NOT_FOUND);

        if (invitation.ReceiverId != userId)
        {
            throw new AppException(403, FriendInvitationErrorCode.ACCESS_DENIED);
        }

        if (!await friendService.AreFriendsAsync(invitation.SenderId, userId))
        {
            await friendService.CreateFriendshipAsync(invitation.SenderId, userId);
        }

        await invitationRepository.DeleteAsync(invitation);
        await this.RemoveReciprocalInvitationIfExistsAsync(invitation.SenderId, userId);
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(Guid userId, Guid invitationId)
    {
        var invitation = await invitationRepository.GetByIdAsync(invitationId)
            ?? throw new AppException(404, FriendInvitationErrorCode.INVITATION_NOT_FOUND);

        if (invitation.SenderId == userId)
        {
            // Canceling an invitation
            await invitationRepository.DeleteAsync(invitation);
        }
        else if (invitation.ReceiverId == userId)
        {
            // Declining an invitation
            await invitationRepository.DeleteAsync(invitation);
            await this.RemoveReciprocalInvitationIfExistsAsync(invitation.SenderId, invitation.ReceiverId);
        }
        else
        {
            throw new AppException(403, FriendInvitationErrorCode.ACCESS_DENIED);
        }
    }

    private async Task RemoveReciprocalInvitationIfExistsAsync(Guid senderId, Guid receiverId)
    {
        var invitation = await invitationRepository.GetByUsersAsync(receiverId, senderId);
        if (invitation is not null)
        {
            await invitationRepository.DeleteAsync(invitation);
        }
    }
}
