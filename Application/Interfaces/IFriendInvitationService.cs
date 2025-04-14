using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.FriendInvitation;

namespace Application.Interfaces
{
    public interface IFriendInvitationService
    {
        Task<FriendInvitationDto> CreateInvitationAsync(Guid senderId, CreateFriendInvitationDto dto);
        Task<List<FriendInvitationDto>> GetSentInvitationsAsync(Guid userId);
        Task<List<FriendInvitationDto>> GetReceivedInvitationsAsync(Guid userId);
        Task<FriendInvitationDto> AcceptInvitationAsync(Guid userId, Guid invitationId);
        Task<FriendInvitationDto> DeclineInvitationAsync(Guid userId, Guid invitationId);
        Task<FriendInvitationDto> CancelInvitationAsync(Guid userId, Guid invitationId);
    }
}