using API.Extensions;
using API.Models.Errors;
using Application.DTOs.FriendInvitation;
using Application.Exceptions;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <summary>
/// API controller for managing friend invitations between users.
/// Provides endpoints for creating, retrieving, accepting, declining, and canceling invitations.
/// </summary>
[ApiController]
[Route("friend-invitations")]
[Authorize]
public class FriendInvitationsController : ControllerBase
{
    private readonly IFriendInvitationService friendInvitationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="FriendInvitationsController"/> class.
    /// </summary>
    /// <param name="friendInvitationService">Service for managing friend invitations.</param>
    public FriendInvitationsController(IFriendInvitationService friendInvitationService)
    {
        this.friendInvitationService = friendInvitationService;
    }

    /// <summary>
    /// Creates a new friend invitation from the authenticated user to another user.
    /// </summary>
    /// <param name="dto">Data transfer object containing the recipient user ID.</param>
    /// <returns>
    /// 201 Created with the invitation details if successful.
    /// 400 Bad Request if the recipient doesn't exist, is the same as sender, already has an invitation, or is already a friend.
    /// </returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(FriendInvitationDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
    public async Task<ActionResult<FriendInvitationDto>> CreateInvitation([FromBody] CreateFriendInvitationDto dto)
    {
        try
        {
            var userId = this.User.GetUserId();
            var invitation = await this.friendInvitationService.CreateInvitationAsync(userId, dto);

            return this.CreatedAtAction(nameof(this.CreateInvitation), invitation);
        }
        catch (UserNotFoundException)
        {
            return this.BadRequest(new ErrorResponse(FriendInvitationErrorCode.USER_NOT_FOUND));
        }
        catch (CannotInviteSelfException)
        {
            return this.BadRequest(new ErrorResponse(FriendInvitationErrorCode.CANNOT_INVITE_SELF));
        }
        catch (FriendInvitationExistsException)
        {
            return this.BadRequest(new ErrorResponse(FriendInvitationErrorCode.INVITATION_ALREADY_EXISTS));
        }
        catch (AlreadyFriendsException)
        {
            return this.BadRequest(new ErrorResponse(FriendInvitationErrorCode.ALREADY_FRIENDS));
        }
    }

    /// <summary>
    /// Retrieves all invitations sent by the authenticated user.
    /// </summary>
    /// <returns>200 OK with a list of sent invitations.</returns>
    [HttpGet("sent")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<FriendInvitationDto>))]
    public async Task<ActionResult<List<FriendInvitationDto>>> GetSentInvitations()
    {
        var userId = this.User.GetUserId();
        var invitations = await this.friendInvitationService.GetSentInvitationsAsync(userId);
        return this.Ok(invitations);
    }

    /// <summary>
    /// Retrieves all invitations received by the authenticated user.
    /// </summary>
    /// <returns>200 OK with a list of received invitations.</returns>
    [HttpGet("received")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<FriendInvitationDto>))]
    public async Task<ActionResult<List<FriendInvitationDto>>> GetReceivedInvitations()
    {
        var userId = this.User.GetUserId();
        var invitations = await this.friendInvitationService.GetReceivedInvitationsAsync(userId);
        return this.Ok(invitations);
    }

    /// <summary>
    /// Accepts a friend invitation received by the authenticated user.
    /// Also removes any reciprocal invitation if it exists.
    /// </summary>
    /// <param name="id">The ID of the invitation to accept.</param>
    /// <returns>
    /// 200 OK with the invitation details if accepted successfully.
    /// 404 Not Found if the invitation doesn't exist.
    /// 400 Bad Request if the user is not authorized to accept the invitation.
    /// </returns>
    [HttpPost("{id}/accept")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FriendInvitationDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
    public async Task<ActionResult<FriendInvitationDto>> AcceptInvitation(Guid id)
    {
        try
        {
            var userId = this.User.GetUserId();
            var invitation = await this.friendInvitationService.AcceptInvitationAsync(userId, id);
            return this.Ok(invitation);
        }
        catch (FriendInvitationNotFoundException)
        {
            return this.NotFound(new ErrorResponse(FriendInvitationErrorCode.INVITATION_NOT_FOUND));
        }
        catch (UnauthorizedInvitationActionException)
        {
            return this.BadRequest(new ErrorResponse(FriendInvitationErrorCode.UNAUTHORIZED_ACTION));
        }
    }

    /// <summary>
    /// Declines a friend invitation received by the authenticated user.
    /// Also removes any reciprocal invitation if it exists.
    /// </summary>
    /// <param name="id">The ID of the invitation to decline.</param>
    /// <returns>
    /// 200 OK with the invitation details if declined successfully.
    /// 404 Not Found if the invitation doesn't exist.
    /// 400 Bad Request if the user is not authorized to decline the invitation.
    /// </returns>
    [HttpPost("{id}/decline")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FriendInvitationDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
    public async Task<ActionResult<FriendInvitationDto>> DeclineInvitation(Guid id)
    {
        try
        {
            var userId = this.User.GetUserId();
            var invitation = await this.friendInvitationService.DeclineInvitationAsync(userId, id);
            return this.Ok(invitation);
        }
        catch (FriendInvitationNotFoundException)
        {
            return this.NotFound(new ErrorResponse(FriendInvitationErrorCode.INVITATION_NOT_FOUND));
        }
        catch (UnauthorizedInvitationActionException)
        {
            return this.BadRequest(new ErrorResponse(FriendInvitationErrorCode.UNAUTHORIZED_ACTION));
        }
    }

    /// <summary>
    /// Cancels a friend invitation previously sent by the authenticated user.
    /// </summary>
    /// <param name="id">The ID of the invitation to cancel.</param>
    /// <returns>
    /// 200 OK with the invitation details if canceled successfully.
    /// 404 Not Found if the invitation doesn't exist.
    /// 400 Bad Request if the user is not authorized to cancel the invitation.
    /// </returns>
    [HttpPost("{id}/cancel")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FriendInvitationDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
    public async Task<ActionResult<FriendInvitationDto>> CancelInvitation(Guid id)
    {
        try
        {
            var userId = this.User.GetUserId();
            var invitation = await this.friendInvitationService.CancelInvitationAsync(userId, id);
            return this.Ok(invitation);
        }
        catch (FriendInvitationNotFoundException)
        {
            return this.NotFound(new ErrorResponse(FriendInvitationErrorCode.INVITATION_NOT_FOUND));
        }
        catch (UnauthorizedInvitationActionException)
        {
            return this.BadRequest(new ErrorResponse(FriendInvitationErrorCode.UNAUTHORIZED_ACTION));
        }
    }
}
