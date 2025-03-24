using Application.Interfaces;
using Domain.Model;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <summary>
/// Represents controller that defines api group endpoints.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class GroupController : ControllerBase
{
    private readonly IGroupService groupService;

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupController"/> class.
    /// </summary>
    /// <param name="groupService">Group service that defines group data methods.</param>
    public GroupController(IGroupService groupService)
    {
        this.groupService = groupService;
    }

    /// <summary>
    /// Method that defines get group by id endpoint.
    /// </summary>
    /// <param name="id">Unique group's identifier.</param>
    /// <returns>Returns action result object.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<Group>> GetGroupById(int id)
    {
        return this.Ok(await this.groupService.GetGroupByIdAsync(id));
    }

    [HttpPost]
    public async Task<ActionResult<Group>> CreateGroupAsync()
    {
        return this.Ok(await this.groupService.CreateGroupAsync());
    }
    public class JoinGroupRequest
    {
        public int UserId { get; set; }
    }

    [HttpPost("join-via-code/{code}")]
    public async Task<ActionResult<Group>> AddUserViaCodeAsync(string code, [FromBody] JoinGroupRequest request)
    {
        return this.Ok(await this.groupService.AddUserViaCodeAsync(code, request.UserId));
    }

    [HttpPost("{id}/leave")]
    public async Task<ActionResult<Group>> UserLeaveAsync(int id, [FromBody] JoinGroupRequest request)
    {
        return this.Ok(await this.groupService.RemoveUserFromGroupAsync(id, request.UserId));
    }

    [HttpPost("{id}/kick-user/{userId}")]
    public async Task<ActionResult<Group>> UserLeaveAsync(int id, int userId)
    {
        return this.Ok(await this.groupService.RemoveUserFromGroupAsync(id, userId));
    }

}
