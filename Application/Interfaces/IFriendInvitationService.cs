using Application.DTOs;

namespace Application.Interfaces;

public interface IFriendInvitationService
{
    Task<FriendInvitationDto> CreateInvitationAsync(Guid currentUserId, Guid receiverId);

    Task<List<FriendInvitationDto>> GetSentInvitationsAsync(Guid userId);

    Task<List<FriendInvitationDto>> GetReceivedInvitationsAsync(Guid userId);

    Task<FriendInvitationDto> AcceptInvitationAsync(Guid currentUserId, Guid invitationId);

    Task<FriendInvitationDto> DeclineInvitationAsync(Guid currentUserId, Guid invitationId);

    Task<FriendInvitationDto> CancelInvitationAsync(Guid currentUserId, Guid invitationId);
}
