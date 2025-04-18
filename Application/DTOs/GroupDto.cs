using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

/// <summary>
/// Represents the data transfer object used for returning group data in API responses.
/// </summary>
/// <param name="Id">The unique identifier of the group.</param>
/// <param name="JoiningCode">The joining code of the group.</param>
/// <param name="GroupMembers">The members of the group.</param>
public record GroupDto(
    [property: Required] int Id,
    [property: Required] string JoiningCode,
    [property: Required] IEnumerable<GroupMemberDto> GroupMembers);
