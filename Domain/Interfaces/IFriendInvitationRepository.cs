using Domain.Model;

namespace Domain.Interfaces;

public interface IFriendInvitationRepository
{
    Task<FriendInvitation> CreateAsync(FriendInvitation invitation);

    Task<FriendInvitation> GetByIdAsync(Guid invitationId);

    Task<List<FriendInvitation>> GetSentInvitationsAsync(Guid userId);

    Task<List<FriendInvitation>> GetReceivedInvitationsAsync(Guid userId);

    Task DeleteAsync(Guid invitationId);

    Task<bool> ExistsAsync(Guid senderId, Guid receiverId);
}
