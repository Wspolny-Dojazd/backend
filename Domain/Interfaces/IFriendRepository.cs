namespace Domain.Interfaces;

using Domain.Model;

public interface IFriendRepository
{
    Task<FriendInvitation> CreateInvitationAsync(FriendInvitation invitation);

    Task<List<FriendInvitation>> GetSentInvitationsAsync(Guid userId);

    Task<List<FriendInvitation>> GetReceivedInvitationsAsync(Guid userId);

    Task<FriendInvitation> GetInvitationByIdAsync(Guid invitationId);

    Task<bool> InvitationExistsAsync(Guid senderId, Guid receiverId);

    Task DeleteInvitationAsync(FriendInvitation invitation);

    Task<bool> AreFriendsAsync(Guid userId1, Guid userId2);

    Task AddFriendAsync(Guid userId1, Guid userId2);
}
