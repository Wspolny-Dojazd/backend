using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using Domain.Model;

namespace Application.Services
{
    public class FriendInvitationService : IFriendInvitationService
    {
        private readonly IFriendRepository _friendRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public FriendInvitationService(
            IFriendRepository friendRepository,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _friendRepository = friendRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<FriendInvitationDto> CreateInvitationAsync(Guid currentUserId, Guid receiverId)
        {
            // Check if receiver exists
            var receiver = await _userRepository.GetByIdAsync(receiverId);
            if (receiver == null)
            {
                throw new UserNotFoundException(receiverId);
            }

            // Check if user is trying to invite themselves
            if (currentUserId == receiverId)
            {
                throw new CannotInviteSelfException();
            }

            // Check if they are already friends
            var areFriends = await _friendRepository.AreFriendsAsync(currentUserId, receiverId);
            if (areFriends)
            {
                throw new AlreadyFriendsException();
            }

            // Check if invitation already exists
            var invitationExists = await _friendRepository.InvitationExistsAsync(currentUserId, receiverId);
            if (invitationExists)
            {
                throw new InvitationAlreadySentException();
            }

            // Create invitation
            var invitation = new FriendInvitation
            {
                InvitationId = Guid.NewGuid(),
                SenderId = currentUserId,
                ReceiverId = receiverId,
                CreatedAt = DateTime.UtcNow
            };

            await _friendRepository.CreateInvitationAsync(invitation);

            // Load sender and receiver for response
            invitation.Sender = await _userRepository.GetByIdAsync(currentUserId);
            invitation.Receiver = receiver;

            return _mapper.Map<FriendInvitationDto>(invitation);
        }

        public async Task<List<FriendInvitationDto>> GetSentInvitationsAsync(Guid userId)
        {
            var invitations = await _friendRepository.GetSentInvitationsAsync(userId);
            return _mapper.Map<List<FriendInvitationDto>>(invitations);
        }

        public async Task<List<FriendInvitationDto>> GetReceivedInvitationsAsync(Guid userId)
        {
            var invitations = await _friendRepository.GetReceivedInvitationsAsync(userId);
            return _mapper.Map<List<FriendInvitationDto>>(invitations);
        }

        public async Task<FriendInvitationDto> AcceptInvitationAsync(Guid currentUserId, Guid invitationId)
        {
            var invitation = await _friendRepository.GetInvitationByIdAsync(invitationId);

            if (invitation == null)
            {
                throw new InvitationNotFoundException();
            }

            if (invitation.ReceiverId != currentUserId)
            {
                throw new NotAuthorizedToAcceptInvitationException();
            }

            // Add users as friends
            await _friendRepository.AddFriendAsync(invitation.SenderId, invitation.ReceiverId);

            // Get a copy of the invitation for response
            var invitationDto = _mapper.Map<FriendInvitationDto>(invitation);

            // Delete the invitation
            await _friendRepository.DeleteInvitationAsync(invitation);

            return invitationDto;
        }

        public async Task<FriendInvitationDto> DeclineInvitationAsync(Guid currentUserId, Guid invitationId)
        {
            var invitation = await _friendRepository.GetInvitationByIdAsync(invitationId);

            if (invitation == null)
            {
                throw new InvitationNotFoundException();
            }

            if (invitation.ReceiverId != currentUserId)
            {
                throw new NotAuthorizedToDeclineInvitationException();
            }

            // Get a copy of the invitation for response
            var invitationDto = _mapper.Map<FriendInvitationDto>(invitation);

            // Delete the invitation
            await _friendRepository.DeleteInvitationAsync(invitation);

            return invitationDto;
        }

        public async Task<FriendInvitationDto> CancelInvitationAsync(Guid currentUserId, Guid invitationId)
        {
            var invitation = await _friendRepository.GetInvitationByIdAsync(invitationId);

            if (invitation == null)
            {
                throw new InvitationNotFoundException();
            }

            if (invitation.SenderId != currentUserId)
            {
                throw new NotAuthorizedToCancelInvitationException();
            }

            // Get a copy of the invitation for response
            var invitationDto = _mapper.Map<FriendInvitationDto>(invitation);

            // Delete the invitation
            await _friendRepository.DeleteInvitationAsync(invitation);

            return invitationDto;
        }
    }
}