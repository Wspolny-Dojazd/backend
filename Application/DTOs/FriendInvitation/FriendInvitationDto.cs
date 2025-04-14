using System;
using Application.DTOs;

namespace Application.DTOs.FriendInvitation
{
    public class FriendInvitationDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserDto Sender { get; set; }
        public UserDto Receiver { get; set; }
    }
}