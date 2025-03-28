using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Application.DTOs;

/// <summary>
/// Represents the data transfer object used for returning user data in API responses.
/// </summary>
/// <param name="Id">The unique identifier of the user.</param>
/// <param name="Nickname">The nickname of the user.</param>
/// <param name="Email">The email address of the user.</param>
public record UserConfigurationDto(
    [property: Required] TimeSystem TimeSystem,
    [property: Required] DistanceUnit DistanceUnit,
    [property: Required] Language Language,
    [property: Required] Theme Theme);
