using API.Models.Errors;
using Application.DTOs;
using Application.Interfaces;
using Domain.Model;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <summary>
/// Represents controller that defines api group endpoints.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class GroupsController : ControllerBase
{
    private readonly IGroupService groupService;

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupController"/> class.
    /// </summary>
    /// <param name="groupService">Group service that defines group data methods.</param>
    public GroupsController(IGroupService groupService)
    {
        this.groupService = groupService;
    }

    /// <summary>
    /// Method that defines get group by id endpoint.
    /// </summary>
    /// <param name="id">Unique group's identifier.</param>
    /// <returns>Returns action result object.</returns>
    [ProducesResponseType(typeof(GroupDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse<GroupErrorCode>), StatusCodes.Status404NotFound)]
    [HttpGet("{id}")]
    public async Task<ActionResult<Group>> GetGroupById(int id)
    {
        var group = await this.groupService.GetGroupByIdAsync(id);
        return this.Ok(group);
    }

    /// <summary>
    /// Method that defines create group endpoint.
    /// </summary>
    /// <returns>Returns action result object.</returns>
    [ProducesResponseType(typeof(GroupDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse<GroupErrorCode>), StatusCodes.Status404NotFound)]
    [HttpPost]
    public async Task<ActionResult<Group>> CreateGroupAsync()
    {
        var group = await this.groupService.CreateGroupAsync();
        return this.Ok(group);
    }

    /// <summary>
    /// Method that defines add user via joining code endpoint.
    /// </summary>
    /// <param name="code">Unique group's joining code.</param>
    /// <param name="request">GroupRequest object.</param>
    /// <returns>Returns action result object.</returns>
    [ProducesResponseType(typeof(GroupDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse<GroupErrorCode>), StatusCodes.Status404NotFound)]
    [HttpPost("join-via-code/{code}")]
    public async Task<ActionResult<Group>> AddUserViaCodeAsync(string code, [FromBody] GroupRequest request)
    {
        var group = await this.groupService.AddUserViaCodeAsync(code, request.UserId);
        return this.Ok(group);
    }

    /// <summary>
    /// Method that defines user leaves the group endpoint.
    /// </summary>
    /// <param name="id">Unique user's identifier.</param>
    /// <param name="request">GroupREquest object.</param>
    /// <returns>Returns action result object.</returns>
    [ProducesResponseType(typeof(GroupDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse<GroupErrorCode>), StatusCodes.Status404NotFound)]
    [HttpPost("{id}/leave")]
    public async Task<ActionResult<Group>> UserLeaveAsync(int id, [FromBody] GroupRequest request)
    {
        var group = await this.groupService.RemoveUserFromGroupAsync(id, request.UserId);
        return this.Ok(group);
    }

    /// <summary>
    /// Method that defines kick user from the group endpoint.
    /// </summary>
    /// <param name="id">Unique group's identifier.</param>
    /// <param name="userId">Unique user's indentifier.</param>
    /// <returns>Returns action result object.</returns>
    [ProducesResponseType(typeof(GroupDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse<GroupErrorCode>), StatusCodes.Status404NotFound)]
    [HttpPost("{id}/kick-user/{userId}")]
    public async Task<ActionResult<Group>> KickUserAsync(int id, int userId)
    {
        var group = await this.groupService.RemoveUserFromGroupAsync(id, userId);
        return this.Ok(group);
    }
}
