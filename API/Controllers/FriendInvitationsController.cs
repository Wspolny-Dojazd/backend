using System.Security.Claims;
using API.Models;
using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("friend-invitations")]
[Authorize]
public class FriendInvitationsController : ControllerBase
{
    private readonly IFriendInvitationService _friendInvitationService;

    public FriendInvitationsController(IFriendInvitationService friendInvitationService)
    {
        _friendInvitationService = friendInvitationService;
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            throw new UnauthorizedAccessException("User ID not found in claims");
        }
        return userId;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(FriendInvitationDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateInvitation([FromBody] CreateFriendInvitationRequest request)
    {
        var currentUserId = GetCurrentUserId();
        var result = await _friendInvitationService.CreateInvitationAsync(currentUserId, request.UserId);
        return CreatedAtAction(nameof(GetSentInvitations), null, result);
    }

    [HttpGet("sent")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<FriendInvitationDto>))]
    public async Task<IActionResult> GetSentInvitations()
    {
        var currentUserId = GetCurrentUserId();
        var invitations = await _friendInvitationService.GetSentInvitationsAsync(currentUserId);
        return Ok(invitations);
    }

    [HttpGet("received")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<FriendInvitationDto>))]
    public async Task<IActionResult> GetReceivedInvitations()
    {
        var currentUserId = GetCurrentUserId();
        var invitations = await _friendInvitationService.GetReceivedInvitationsAsync(currentUserId);
        return Ok(invitations);
    }

    [HttpPost("{id}/accept")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FriendInvitationDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AcceptInvitation([FromRoute] Guid id)
    {
        var currentUserId = GetCurrentUserId();
        var result = await _friendInvitationService.AcceptInvitationAsync(currentUserId, id);
        return Ok(result);
    }

    [HttpPost("{id}/decline")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FriendInvitationDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeclineInvitation([FromRoute] Guid id)
    {
        var currentUserId = GetCurrentUserId();
        var result = await _friendInvitationService.DeclineInvitationAsync(currentUserId, id);
        return Ok(result);
    }

    [HttpPost("{id}/cancel")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FriendInvitationDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelInvitation([FromRoute] Guid id)
    {
        var currentUserId = GetCurrentUserId();
        var result = await _friendInvitationService.CancelInvitationAsync(currentUserId, id);
        return Ok(result);
    }
}
