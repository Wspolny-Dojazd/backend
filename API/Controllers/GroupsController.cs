using API.Extensions;
using API.Models.Errors;
using Application.DTOs;
using Application.DTOs.Message;
using Application.Interfaces;
using Domain.Model;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <summary>
/// Provides API endpoints for managing groups and user membership within groups.
/// </summary>
/// <param name="groupService">The service that handles group-related logic.</param>
/// <param name="messageService">The service that handles message logic.</param>
[Route("api/[controller]")]
[ApiController]
public class GroupsController(IGroupService groupService, IMessageService messageService)
    : ControllerBase
{
    /// <summary>
    /// Retrieves a group by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the group.</param>
    /// <returns>The requested group details.</returns>
    /// <response code="200">The group was successfully retrieved.</response>
    /// <response code="404">The group was not found.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GroupDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse<GroupErrorCode>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GroupDto>> GetGroupById(int id)
    {
        var group = await groupService.GetByIdAsync(id);
        return this.Ok(group);
    }

    /// <summary>
    /// Creates a new group.
    /// </summary>
    /// <returns>The created group details.</returns>
    /// <response code="200">The group was successfully created.</response>
    [HttpPost]
    [ProducesResponseType(typeof(GroupDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<GroupDto>> Create()
    {
        var group = await groupService.CreateAsync();
        return this.Ok(group);
    }

    /// <summary>
    /// Adds the currently authenticated user to a group using a joining code.
    /// </summary>
    /// <param name="code">The unique joining code of the group.</param>
    /// <returns>The updated group details.</returns>
    /// <response code="200">The user was successfully added to the group.</response>
    /// <response code="404">The group was not found.</response>
    [HttpPost("join/code/{code}")]
    [ProducesResponseType(typeof(GroupDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse<GroupErrorCode>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GroupDto>> JoinByCode(string code)
    {
        var userId = this.User.GetUserId();
        var group = await groupService.AddUserByCodeAsync(code, userId);
        return this.Ok(group);
    }

    /// <summary>
    /// Removes the currently authenticated user from the specified group.
    /// </summary>
    /// <param name="id">The unique identifier of the group.</param>
    /// <returns>The updated group details.</returns>
    /// <response code="200">The user was successfully removed from the group.</response>
    /// <response code="404">The group was not found.</response>
    [HttpPost("{id}/leave")]
    [ProducesResponseType(typeof(GroupDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse<GroupErrorCode>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GroupDto>> Leave(int id)
    {
        var userId = this.User.GetUserId();
        var group = await groupService.RemoveUserAsync(id, userId);
        return this.Ok(group);
    }

    /// <summary>
    /// Removes a specific user from a group (kick user).
    /// </summary>
    /// <param name="id">The unique identifier of the group.</param>
    /// <param name="userId">The unique identifier of the user to be removed.</param>
    /// <returns>The updated group details.</returns>
    /// <response code="200">The user was successfully removed from the group.</response>
    /// <response code="400">The user is not a member of the group.</response>
    /// <response code="404">The group was not found.</response>
    [HttpPost("{id}/kick/{userId}")]
    [ProducesResponseType(typeof(GroupDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse<GroupErrorCode>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse<GroupErrorCode>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GroupDto>> Kick(int id, Guid userId)
    {
        var group = await groupService.RemoveUserAsync(id, userId);
        return this.Ok(group);
    }

    /// <summary>
    /// Retrieves all messages for a specified group.
    /// </summary>
    /// <param name="id">The unique identifier of the group.</param>
    /// <returns>The messages for the group with details.</returns>
    /// <response code="200">The messages for the group was successfully retrieved.</response>
    /// <response code="403">The user is not a member of the group.</response>
    /// <response code="404">The group or the user was not found.</response>
    [HttpGet("{id}/messages")]
    [ProducesResponseType(typeof(IEnumerable<MessageDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse<GroupErrorCode>), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse<GroupErrorCode>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessages(int id)
    {
        var userId = this.User.GetUserId();
        var messages = await messageService.GetAllByGroupIdAsync(userId, id);
        return this.Ok(messages);
    }

    /// <summary>
    /// Sends a message to a specified group.
    /// </summary>
    /// <param name="payload">The request payload that contains the message content.</param>
    /// <param name="id">The unique identifier of the group.</param>
    /// <returns>The data transfer object of the sent message with details.</returns>
    /// <response code="200">The message was successfully sent.</response>
    /// <response code="400">The message was empty.</response>
    /// <response code="403">The user is not a member of the group.</response>
    /// <response code="404">The group or the user was not found.</response>
    [HttpPost("{id}/messages")]
    [ProducesResponseType(typeof(MessageDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse<MessageErrorCode>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse<GroupErrorCode>), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse<GroupErrorCode>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MessageDto>> SendMessage([FromBody] MessagePayloadDto payload, int id)
    {
        var userId = this.User.GetUserId();
        var message = await messageService.SendAsync(userId, id, payload.Content);
        return this.Ok(message);
    }
}
