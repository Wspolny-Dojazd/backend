namespace Domain.Model;

public class FriendInvitation
{
    public Guid InvitationId { get; set; }

    public Guid SenderId { get; set; }

    public Guid ReceiverId { get; set; }

    public DateTime CreatedAt { get; set; }

    public User Sender { get; set; }

    public User Receiver { get; set; }
}
