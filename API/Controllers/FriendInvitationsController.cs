using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Extensions;
using API.Models.Errors;
using Application.DTOs.FriendInvitation;
using Application.Exceptions;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
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
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(FriendInvitationDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        public async Task<ActionResult<FriendInvitationDto>> CreateInvitation([FromBody] CreateFriendInvitationDto dto)
        {
            try
            {
                var userId = User.GetUserId();
                var invitation = await _friendInvitationService.CreateInvitationAsync(userId, dto);
                return CreatedAtAction(nameof(CreateInvitation), invitation);
            }
            catch (UserNotFoundException)
            {
                return BadRequest(new ErrorResponse(FriendInvitationErrorCode.USER_NOT_FOUND));
            }
            catch (CannotInviteSelfException)
            {
                return BadRequest(new ErrorResponse(FriendInvitationErrorCode.CANNOT_INVITE_SELF));
            }
            catch (FriendInvitationExistsException)
            {
                return BadRequest(new ErrorResponse(FriendInvitationErrorCode.INVITATION_ALREADY_EXISTS));
            }
            catch (AlreadyFriendsException)
            {
                return BadRequest(new ErrorResponse(FriendInvitationErrorCode.ALREADY_FRIENDS));
            }
        }
        
        [HttpGet("sent")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<FriendInvitationDto>))]
        public async Task<ActionResult<List<FriendInvitationDto>>> GetSentInvitations()
        {
            var userId = User.GetUserId();
            var invitations = await _friendInvitationService.GetSentInvitationsAsync(userId);
            return Ok(invitations);
        }
        
        [HttpGet("received")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<FriendInvitationDto>))]
        public async Task<ActionResult<List<FriendInvitationDto>>> GetReceivedInvitations()
        {
            var userId = User.GetUserId();
            var invitations = await _friendInvitationService.GetReceivedInvitationsAsync(userId);
            return Ok(invitations);
        }
        
        [HttpPost("{id}/accept")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FriendInvitationDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        public async Task<ActionResult<FriendInvitationDto>> AcceptInvitation(Guid id)
        {
            try
            {
                var userId = User.GetUserId();
                var invitation = await _friendInvitationService.AcceptInvitationAsync(userId, id);
                return Ok(invitation);
            }
            catch (FriendInvitationNotFoundException)
            {
                return NotFound(new ErrorResponse(FriendInvitationErrorCode.INVITATION_NOT_FOUND));
            }
            catch (UnauthorizedInvitationActionException)
            {
                return BadRequest(new ErrorResponse(FriendInvitationErrorCode.UNAUTHORIZED_ACTION));
            }
        }
        
        [HttpPost("{id}/decline")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FriendInvitationDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        public async Task<ActionResult<FriendInvitationDto>> DeclineInvitation(Guid id)
        {
            try
            {
                var userId = User.GetUserId();
                var invitation = await _friendInvitationService.DeclineInvitationAsync(userId, id);
                return Ok(invitation);
            }
            catch (FriendInvitationNotFoundException)
            {
                return NotFound(new ErrorResponse(FriendInvitationErrorCode.INVITATION_NOT_FOUND));
            }
            catch (UnauthorizedInvitationActionException)
            {
                return BadRequest(new ErrorResponse(FriendInvitationErrorCode.UNAUTHORIZED_ACTION));
            }
        }
        
        [HttpPost("{id}/cancel")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FriendInvitationDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        public async Task<ActionResult<FriendInvitationDto>> CancelInvitation(Guid id)
        {
            try
            {
                var userId = User.GetUserId();
                var invitation = await _friendInvitationService.CancelInvitationAsync(userId, id);
                return Ok(invitation);
            }
            catch (FriendInvitationNotFoundException)
            {
                return NotFound(new ErrorResponse(FriendInvitationErrorCode.INVITATION_NOT_FOUND));
            }
            catch (UnauthorizedInvitationActionException)
            {
                return BadRequest(new ErrorResponse(FriendInvitationErrorCode.UNAUTHORIZED_ACTION));
            }
        }
    }
}