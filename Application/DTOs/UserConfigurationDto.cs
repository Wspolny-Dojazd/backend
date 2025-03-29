using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Application.DTOs;

/// <summary>
/// Represents the user configuration settings, including time system, distance unit, language, and theme preferences.
/// This class is used for managing and updating user-specific settings in the system.
/// </summary>
public class UserConfigurationDto
{
    /// <summary>
    /// Gets or sets the time system preference of the user.
    /// </summary>
    [Required]
    public required TimeSystem TimeSystem { get; set; }

    /// <summary>
    /// Gets or sets the unit of distance preferred by the user.
    /// </summary>
    [Required]
    public required DistanceUnit DistanceUnit { get; set; }

    /// <summary>
    /// Gets or sets the language preference of the user.
    /// </summary>
    [Required]
    public required Language Language { get; set; }

    /// <summary>
    /// Gets or sets the theme preference of the user.
    /// </summary>
    [Required]
    public required Theme Theme { get; set; }
}
