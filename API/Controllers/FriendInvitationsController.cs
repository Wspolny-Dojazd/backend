using API.Extensions;
using API.Models.Errors;
using Application.DTOs.FriendInvitation;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <summary>
/// Provides API endpoints for managing friend invitations.
/// </summary>
/// <param name="friendInvitationService">Service for managing friend invitations.</param>
[Route("/api/friend-invitations")]
[ApiController]
public class FriendInvitationsController(IFriendInvitationService friendInvitationService)
    : ControllerBase
{
    /// <summary>
    /// Creates a new friend invitation from the authenticated user to another user.
    /// </summary>
    /// <param name="dto">The friend invitation request data.</param>
    /// <returns>The created friend invitation.</returns>
    /// <response code="200">Friend invitation created successfully.</response>
    /// <response code="400">Self-invitation, already sent, already friends, or reciprocal invitation exists.</response>
    /// <response code="404">The recipient user was not found.</response>
    [HttpPost]
    [ProducesResponseType(typeof(FriendInvitationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse<FriendInvitationErrorCode>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse<FriendInvitationErrorCode>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FriendInvitationDto>> SendInvitation([FromBody] FriendInvitationRequestDto dto)
    {
        var userId = this.User.GetUserId();
        var invitation = await friendInvitationService.SendAsync(userId, dto);
        return this.Ok(invitation);
    }

    /// <summary>
    /// Retrieves all invitations sent by the authenticated user.
    /// </summary>
    /// <returns>The sent invitations.</returns>
    /// <response code="200">The sent invitations were retrieved successfully.</response>
    [HttpGet("sent")]
    [ProducesResponseType(typeof(IEnumerable<FriendInvitationDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<FriendInvitationDto>>> GetSentInvitations()
    {
        var userId = this.User.GetUserId();
        var invitations = await friendInvitationService.GetSentAsync(userId);
        return this.Ok(invitations);
    }

    /// <summary>
    /// Retrieves all invitations received by the authenticated user.
    /// </summary>
    /// <returns>The received invitations.</returns>
    /// <response code="200">The received invitations were retrieved successfully.</response>
    [HttpGet("received")]
    [ProducesResponseType(typeof(IEnumerable<FriendInvitationDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<FriendInvitationDto>>> GetReceivedInvitations()
    {
        var userId = this.User.GetUserId();
        var invitations = await friendInvitationService.GetReceivedAsync(userId);
        return this.Ok(invitations);
    }

    /// <summary>
    /// Accepts a friend invitation and creates a friendship between the sender and receiver.
    /// </summary>
    /// <param name="id">The unique identifier of the invitation to accept.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
    /// <response code="204">Friend invitation accepted successfully.</response>
    /// <response code="403">The user is not authorized to accept the invitation.</response>
    /// <response code="404">The invitation was not found.</response>
    [HttpPost("{id}/accept")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse<FriendInvitationErrorCode>), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse<FriendInvitationErrorCode>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AcceptInvitation(Guid id)
    {
        var userId = this.User.GetUserId();
        await friendInvitationService.AcceptAsync(userId, id);
        return this.NoContent();
    }

    /// <summary>
    /// Deletes a friend invitation.
    /// </summary>
    /// <param name="id">The unique identifier of the invitation to delete.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
    /// <response code="204">Friend invitation deleted successfully.</response>
    /// <response code="403">The user is not authorized to delete the invitation.</response>
    /// <response code="404">The invitation was not found.</response>
    /// <remarks>
    /// This method executes a <c>cancel</c> operation if the user is the sender,
    /// or a <c>decline</c> operation if the user is the receiver.
    /// </remarks>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse<FriendInvitationErrorCode>), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse<FriendInvitationErrorCode>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteInvitation(Guid id)
    {
        var userId = this.User.GetUserId();
        await friendInvitationService.DeleteAsync(userId, id);
        return this.NoContent();
    }
}
