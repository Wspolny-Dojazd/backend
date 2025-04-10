using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

/// <summary>
/// Represents the data transfer object used for returning group member data in API responses.
/// </summary>
/// <param name="Id">The unique identifier of the group member.</param>
/// <param name="Nickname">The nickname of the group member.</param>
/// /// <param name="IsCreator">Whether the user is the creator of the group.</param>
public record GroupMemberDto(
    [property: Required] Guid Id,
    [property: Required] string Nickname,
    [property: Required] bool IsCreator);
