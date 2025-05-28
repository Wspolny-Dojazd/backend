using API.Extensions;
using Application.DTOs;
using Application.DTOs.GroupInvitation;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Enums.ErrorCodes;

namespace API.Controllers;

/// <summary>
/// Provides API endpoints for managing group invitations.
/// </summary>
/// <param name="groupInvitationService">Service for managing group invitations.</param>
[Route("/api/groups")]
[ApiController]
public class GroupInvitationsController(IGroupInvitationService groupInvitationService)
    : ControllerBase
{
    /// <summary>
    /// Sends a new group invitation from the authenticated user to another user.
    /// </summary>
    /// <param name="dto">The group invitation request data.</param>
    /// <returns>The sent group invitation.</returns>
    /// <response code="200">Group invitation sent successfully.</response>
    /// <response code="400">
    /// Self-invitation, already sent, already in group, or reciprocal invitation exists.
    /// </response>
    /// <response code="404">The recipient user was not found.</response>
    [HttpPost("invitations")]
    [ProducesResponseType(typeof(GroupInvitationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse<GroupInvitationErrorCode>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse<GroupInvitationErrorCode>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GroupInvitationDto>> SendInvitation(
        [FromBody] GroupInvitationRequestDto dto)
    {
        var userId = this.User.GetUserId();
        var invitation = await groupInvitationService.SendAsync(userId, dto);
        return this.Ok(invitation);
    }

    /// <summary>
    /// Retrieves all invitations that were sent from the specific group.
    /// </summary>
    /// <param name="groupId">The unique identifier of the group.</param>
    /// <returns>The sent invitations.</returns>
    /// <response code="200">The sent invitations were retrieved successfully.</response>
    [HttpGet("{groupId}/invitations")]
    [ProducesResponseType(typeof(IEnumerable<GroupInvitationDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<GroupInvitationDto>>> GetSentInvitations(int groupId)
    {
        var invitations = await groupInvitationService.GetAllSentAsync(groupId);
        return this.Ok(invitations);
    }

    /// <summary>
    /// Retrieves all invitations received by the authenticated user.
    /// </summary>
    /// <returns>The received invitations.</returns>
    /// <response code="200">The received invitations were retrieved successfully.</response>
    [HttpGet("invitations/received")]
    [ProducesResponseType(typeof(IEnumerable<GroupInvitationDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<GroupInvitationDto>>> GetReceivedInvitations()
    {
        var userId = this.User.GetUserId();
        var invitations = await groupInvitationService.GetReceivedAsync(userId);
        return this.Ok(invitations);
    }

    /// <summary>
    /// Accepts a group invitation between the sender and receiver.
    /// </summary>
    /// <param name="id">The unique identifier of the invitation to accept.</param>
    /// <returns>The group the user was added to.</returns>
    /// <response code="200">Group invitation accepted successfully.</response>
    /// <response code="400">The user is already in the group.</response>
    /// <response code="403">The user is not authorized to accept the invitation.</response>
    /// <response code="404">The invitation was not found.</response>
    [HttpPost("invitations/{id}/accept")]
    [ProducesResponseType(typeof(GroupDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse<GroupInvitationErrorCode>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse<GroupInvitationErrorCode>), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse<GroupInvitationErrorCode>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GroupDto>> AcceptInvitation(Guid id)
    {
        var userId = this.User.GetUserId();
        var group = await groupInvitationService.AcceptAsync(userId, id);
        return this.Ok(group);
    }

    /// <summary>
    /// Deletes a group invitation.
    /// </summary>
    /// <param name="id">The unique identifier of the invitation to delete.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
    /// <response code="204">Group invitation deleted successfully.</response>
    /// <response code="403">The user is not authorized to delete the invitation.</response>
    /// <response code="404">The invitation was not found.</response>
    /// <remarks>
    /// This method executes a <c>cancel</c> operation if the user is the sender,
    /// or a <c>decline</c> operation if the user is the receiver.
    /// </remarks>
    [HttpDelete("invitations/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse<GroupInvitationErrorCode>), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse<GroupInvitationErrorCode>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteInvitation(Guid id)
    {
        var userId = this.User.GetUserId();
        await groupInvitationService.DeleteAsync(userId, id);
        return this.NoContent();
    }
}
