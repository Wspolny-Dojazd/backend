using System.Runtime.InteropServices.JavaScript;
using Application.DTOs.FriendInvitation;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using Domain.Model;

namespace Application.Services;


/// <summary>
/// Service that implements friend invitation functionality including creating, accepting,
/// declining, and retrieving invitations between users.
/// </summary>
public class FriendInvitationService : IFriendInvitationService
{
    private readonly IFriendInvitationRepository invitationRepository;
    private readonly IUserRepository userRepository;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="FriendInvitationService"/> class.
    /// </summary>
    /// <param name="invitationRepository">Repository for accessing friend invitation data.</param>
    /// <param name="userRepository">Repository for accessing user data.</param>
    /// <param name="mapper">AutoMapper instance for object mapping.</param>
    public FriendInvitationService(
        IFriendInvitationRepository invitationRepository,
        IUserRepository userRepository,
        IMapper mapper)
    {
        this.invitationRepository = invitationRepository;
        this.userRepository = userRepository;
        this.mapper = mapper;
    }

    public async Task<FriendInvitationDto> CreateInvitationAsync(Guid senderId, CreateFriendInvitationDto dto)
    {
        var receiver = await this.userRepository.GetByIdAsync(dto.UserId);

        if (receiver == null)
        {
            throw new UserNotFoundException(string.Empty);
        }

        if (senderId == dto.UserId)
        {
            throw new CannotInviteSelfException();
        }

        var sender = await this.userRepository.GetByIdAsync(senderId);
        if (sender == null)
        {
            throw new UserNotFoundException(string.Empty);
        }

        if (sender.Friends != null && sender.Friends.Any(f => f.Id == dto.UserId))
        {
            throw new AlreadyFriendsException();
        }


        // Check if invitation already exists
        if (await this.invitationRepository.ExistsAsync(senderId, dto.UserId))
            throw new FriendInvitationExistsException();

        var invitation = new FriendInvitation
        {
            InvitationId = Guid.NewGuid(),
            SenderId = senderId,
            ReceiverId = dto.UserId,
            CreatedAt = DateTime.UtcNow,
            Sender = sender,
            Receiver = receiver
        };

        await this.invitationRepository.CreateAsync(invitation);

        return this.mapper.Map<FriendInvitationDto>(invitation);
    }

    public async Task<List<FriendInvitationDto>> GetSentInvitationsAsync(Guid userId)
    {
        var invitations = await this.invitationRepository.GetSentInvitationsAsync(userId);
        return this.mapper.Map<List<FriendInvitationDto>>(invitations);
    }

    public async Task<List<FriendInvitationDto>> GetReceivedInvitationsAsync(Guid userId)
    {
        var invitations = await this.invitationRepository.GetReceivedInvitationsAsync(userId);
        return this.mapper.Map<List<FriendInvitationDto>>(invitations);
    }

    public async Task<FriendInvitationDto> AcceptInvitationAsync(Guid userId, Guid invitationId)
    {
        var invitation = await this.invitationRepository.GetByIdAsync(invitationId);
        if (invitation == null)
            throw new FriendInvitationNotFoundException();

        if (invitation.ReceiverId != userId)
            throw new UnauthorizedInvitationActionException();

        // Add to friends list
        await this.userRepository.AddFriendAsync(invitation.SenderId, invitation.ReceiverId);

        // Store the invitation data for returning it later
        var invitationDto = this.mapper.Map<FriendInvitationDto>(invitation);

        // Delete the current invitation
        await this.invitationRepository.DeleteAsync(invitationId);

        // Check and delete any reciprocal invitation
        // (where the receiver of the current invitation is the sender of another invitation to the current sender)
        var reciprocalInvitations = await this.invitationRepository.GetSentInvitationsAsync(invitation.ReceiverId);
        var reciprocalInvitation = reciprocalInvitations.FirstOrDefault(i => i.ReceiverId == invitation.SenderId);

        if (reciprocalInvitation != null)
        {
            await this.invitationRepository.DeleteAsync(reciprocalInvitation.InvitationId);
        }

        return invitationDto;
    }

    public async Task<FriendInvitationDto> DeclineInvitationAsync(Guid userId, Guid invitationId)
    {
        var invitation = await this.invitationRepository.GetByIdAsync(invitationId);
        if (invitation == null)
            throw new FriendInvitationNotFoundException();

        if (invitation.ReceiverId != userId)
            throw new UnauthorizedInvitationActionException();

        var invitationDto = this.mapper.Map<FriendInvitationDto>(invitation);

        await this.invitationRepository.DeleteAsync(invitationId);

        var reciprocalInvitations = await this.invitationRepository.GetSentInvitationsAsync(invitation.ReceiverId);

        var reciprocalInvitation = reciprocalInvitations.FirstOrDefault(i => i.ReceiverId == invitation.SenderId);

        if (reciprocalInvitation != null)
        {
            await this.invitationRepository.DeleteAsync(reciprocalInvitation.InvitationId);
        }

        return invitationDto;
    }

    public async Task<FriendInvitationDto> CancelInvitationAsync(Guid userId, Guid invitationId)
    {
        var invitation = await this.invitationRepository.GetByIdAsync(invitationId);
        if (invitation == null)
            throw new FriendInvitationNotFoundException();

        if (invitation.SenderId != userId)
            throw new UnauthorizedInvitationActionException();

        await this.invitationRepository.DeleteAsync(invitationId);

        return this.mapper.Map<FriendInvitationDto>(invitation);
    }
}
