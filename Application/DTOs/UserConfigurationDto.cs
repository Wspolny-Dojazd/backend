using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Application.DTOs;

/// <summary>
/// Represents the data transfer object used for returning user configuration data in API responses.
/// </summary>
public class UserConfigurationDto
{
    /// <summary>
    /// Gets the time system preference of the user.
    /// </summary>
    [Required]
    public TimeSystem TimeSystem { get; init; }

    /// <summary>
    /// Gets the unit of distance preferred by the user.
    /// </summary>
    [Required]
    public DistanceUnit DistanceUnit { get; init; }

    /// <summary>
    /// Gets the language preference of the user.
    /// </summary>
    [Required]
    public Language Language { get; init; }

    /// <summary>
    /// Gets the theme preference of the user.
    /// </summary>
    [Required]
    public Theme Theme { get; init; }
}
