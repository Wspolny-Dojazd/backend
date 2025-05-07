using Domain.Enums;

namespace Domain.Model;

/// <summary>
/// Represents user-specific application configuration settings.
/// </summary>
public class UserConfiguration
{
    /// <summary>
    /// Gets or sets the unique identifier of the configuration.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user to whom the configuration belongs.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the preferred application language.
    /// </summary>
    public Language Language { get; set; }

    /// <summary>
    /// Gets or sets the preferred time display format.
    /// </summary>
    public TimeSystem TimeSystem { get; set; }

    /// <summary>
    /// Gets or sets the preferred distance unit.
    /// </summary>
    public DistanceUnit DistanceUnit { get; set; }

    /// <summary>
    /// Gets or sets the preferred display theme.
    /// </summary>
    public Theme Theme { get; set; }
}
