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

    /// <summary>
    /// Creates a new friend invitation from one user to another.
    /// </summary>
    /// <param name="senderId">The ID of the user sending the invitation.</param>
    /// <param name="dto">Data transfer object containing the recipient user ID.</param>
    /// <returns>A data transfer object representing the created invitation.</returns>
    /// <exception cref="UserNotFoundException">Thrown when the recipient user doesn't exist.</exception>
    /// <exception cref="CannotInviteSelfException">Thrown when a user attempts to invite themselves.</exception>
    /// <exception cref="AlreadyFriendsException">Thrown when users are already friends.</exception>
    /// <exception cref="FriendInvitationExistsException">Thrown when an invitation already exists between the users.</exception>
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

        if (await this.invitationRepository.ExistsAsync(senderId, dto.UserId))
        {
            throw new FriendInvitationExistsException();
        }

        var invitation = new FriendInvitation
        {
            InvitationId = Guid.NewGuid(),
            SenderId = senderId,
            ReceiverId = dto.UserId,
            CreatedAt = DateTime.UtcNow,
            Sender = sender,
            Receiver = receiver,
        };

        await this.invitationRepository.CreateAsync(invitation);

        return this.mapper.Map<FriendInvitationDto>(invitation);
    }

    /// <summary>
    /// Retrieves all invitations sent by a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user who sent the invitations.</param>
    /// <returns>A list of data transfer objects representing the sent invitations.</returns>
    public async Task<List<FriendInvitationDto>> GetSentInvitationsAsync(Guid userId)
    {
        var invitations = await this.invitationRepository.GetSentInvitationsAsync(userId);
        return this.mapper.Map<List<FriendInvitationDto>>(invitations);
    }

    /// <summary>
    /// Retrieves all invitations received by a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user who received the invitations.</param>
    /// <returns>A list of data transfer objects representing the received invitations.</returns>
    public async Task<List<FriendInvitationDto>> GetReceivedInvitationsAsync(Guid userId)
    {
        var invitations = await this.invitationRepository.GetReceivedInvitationsAsync(userId);
        return this.mapper.Map<List<FriendInvitationDto>>(invitations);
    }

    /// <summary>
    /// Accepts a friend invitation, creating a friendship between the users and removing the invitation.
    /// Also removes any reciprocal invitation if it exists.
    /// </summary>
    /// <param name="userId">The ID of the user accepting the invitation (must be the receiver).</param>
    /// <param name="invitationId">The ID of the invitation to accept.</param>
    /// <returns>A data transfer object representing the accepted invitation.</returns>
    /// <exception cref="FriendInvitationNotFoundException">Thrown when the invitation doesn't exist.</exception>
    /// <exception cref="UnauthorizedInvitationActionException">Thrown when the user is not authorized to accept the invitation.</exception>
    public async Task<FriendInvitationDto> AcceptInvitationAsync(Guid userId, Guid invitationId)
    {
        var invitation = await this.invitationRepository.GetByIdAsync(invitationId);
        if (invitation == null)
        {
            throw new FriendInvitationNotFoundException();
        }

        if (invitation.ReceiverId != userId)
        {
            throw new UnauthorizedInvitationActionException();
        }

        await this.userRepository.AddFriendAsync(invitation.SenderId, invitation.ReceiverId);

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

    /// <summary>
    /// Declines a friend invitation, removing it from the system.
    /// Also removes any reciprocal invitation if it exists.
    /// </summary>
    /// <param name="userId">The ID of the user declining the invitation (must be the receiver).</param>
    /// <param name="invitationId">The ID of the invitation to decline.</param>
    /// <returns>A data transfer object representing the declined invitation.</returns>
    /// <exception cref="FriendInvitationNotFoundException">Thrown when the invitation doesn't exist.</exception>
    /// <exception cref="UnauthorizedInvitationActionException">Thrown when the user is not authorized to decline the invitation.</exception>
    public async Task<FriendInvitationDto> DeclineInvitationAsync(Guid userId, Guid invitationId)
    {
        var invitation = await this.invitationRepository.GetByIdAsync(invitationId);
        if (invitation == null)
        {
            throw new FriendInvitationNotFoundException();
        }

        if (invitation.ReceiverId != userId)
        {
            throw new UnauthorizedInvitationActionException();
        }

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

    /// <summary>
    /// Cancels a friend invitation that was previously sent.
    /// </summary>
    /// <param name="userId">The ID of the user canceling the invitation (must be the sender).</param>
    /// <param name="invitationId">The ID of the invitation to cancel.</param>
    /// <returns>A data transfer object representing the canceled invitation.</returns>
    /// <exception cref="FriendInvitationNotFoundException">Thrown when the invitation doesn't exist.</exception>
    /// <exception cref="UnauthorizedInvitationActionException">Thrown when the user is not authorized to cancel the invitation.</exception>
    public async Task<FriendInvitationDto> CancelInvitationAsync(Guid userId, Guid invitationId)
    {
        var invitation = await this.invitationRepository.GetByIdAsync(invitationId);
        if (invitation == null)
        {
            throw new FriendInvitationNotFoundException();
        }

        if (invitation.SenderId != userId)
        {
            throw new UnauthorizedInvitationActionException();
        }

        await this.invitationRepository.DeleteAsync(invitationId);

        return this.mapper.Map<FriendInvitationDto>(invitation);
    }
}
