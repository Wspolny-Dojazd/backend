using API.Attributes;
using Application.DTOs.Path;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Enums.ErrorCodes;

namespace API.Controllers;

/// <summary>
/// Provides API endpoints for managing proposed paths within groups.
/// </summary>
/// <param name="groupPathService">The service that handles logic related to confirmed group paths.</param>
/// <param name="proposedPathService">The service that handles logic related to proposed paths.</param>
[Route("api/groups/{groupId}/paths")]
[ApiController]
public class GroupPathsController(
    IGroupPathService groupPathService,
    IProposedPathService proposedPathService)
    : ControllerBase
{
    /// <summary>
    /// Generates multiple proposed path sets for all users in the group
    /// based on their locations and a desired destination.
    /// </summary>
    /// <param name="groupId">The identifier of the group.</param>
    /// <param name="request">The request containing destination and user locations.</param>
    /// <returns>The generated proposed paths.</returns>
    /// <response code="200">The paths were successfully generated.</response>
    /// <response code="400">Cannot generate paths because one has already been accepted.</response>
    /// <response code="403">The user is not the owner of the group.</response>
    /// <response code="404">The group was not found.</response>
    [HttpPost]
    [RequireGroupOwnership("groupId")]
    [ProducesResponseType(typeof(IEnumerable<ProposedPathDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse<GroupPathErrorCode>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse<GroupPathErrorCode>), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse<GroupPathErrorCode>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<ProposedPathDto>>> GeneratePaths(int groupId, [FromBody] PathRequestDto request)
    {
        var paths = await proposedPathService.GenerateAsync(groupId, request);
        return this.Ok(paths);
    }

    /// <summary>
    /// Retrieves all proposed paths for the specified group.
    /// </summary>
    /// <param name="groupId">The identifier of the group.</param>
    /// <returns>The proposed paths.</returns>
    /// <response code="200">The paths were successfully retrieved.</response>
    /// <response code="400">Cannot retrieve paths because one has already been accepted.</response>
    /// <response code="403">The user is not a member of the group.</response>
    /// <response code="404">The group was not found.</response>
    [HttpGet]
    [RequireGroupMembership("groupId")]
    [ProducesResponseType(typeof(IEnumerable<ProposedPathDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse<GroupPathErrorCode>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse<GroupPathErrorCode>), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse<GroupPathErrorCode>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<ProposedPathDto>>> GetPaths(int groupId)
    {
        var paths = await proposedPathService.GetRangeAsync(groupId);
        return this.Ok(paths);
    }

    /// <summary>
    /// Retrieves the accepted path for the specified group.
    /// </summary>
    /// <param name="groupId">The identifier of the group.</param>
    /// <returns>The accepted proposed path.</returns>
    /// <response code="200">The accepted path was successfully retrieved.</response>
    /// <response code="403">The user is not a member of the group.</response>
    /// <response code="404">The group or the path was not found.</response>
    [HttpGet("accepted")]
    [RequireGroupMembership("groupId")]
    [ProducesResponseType(typeof(ProposedPathDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse<GroupPathErrorCode>), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse<GroupPathErrorCode>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProposedPathDto>> GetAcceptedPath(int groupId)
    {
        var path = await groupPathService.GetByGroupIdAsync(groupId);
        return this.Ok(path);
    }

    /// <summary>
    /// Accepts a proposed path and removes all other proposed paths for the group.
    /// </summary>
    /// <param name="groupId">The identifier of the group.</param>
    /// <param name="pathId">The identifier of the path to accept.</param>
    /// <returns>The accepted path.</returns>
    /// <response code="200">The path was successfully accepted.</response>
    /// <response code="400">The path is already accepted.</response>
    /// <response code="403">The user is not the owner of the group.</response>
    /// <response code="404">The group was not found.</response>
    [HttpPost("{pathId}/accept")]
    [RequireGroupOwnership("groupId")]
    [ProducesResponseType(typeof(ProposedPathDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse<GroupPathErrorCode>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse<GroupPathErrorCode>), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse<GroupPathErrorCode>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProposedPathDto>> AcceptPath(int groupId, Guid pathId)
    {
        var path = await groupPathService.ConfirmFromProposalAsync(groupId, pathId);
        return this.Ok(path);
    }
}
