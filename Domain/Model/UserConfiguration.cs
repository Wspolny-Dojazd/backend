using Domain.Enums;

namespace Domain.Model;

/// <summary>
/// UserConfiguration model class that defines its properties.
/// </summary>
public class UserConfiguration
{
    /// <summary>
    /// Gets or sets the unique configuration identifier.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Gets or sets user's identifier.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets user's system's language.
    /// </summary>
    public Language Language { get; set; }

    /// <summary>
    /// Gets or sets user's system's time system.
    /// </summary>
    public TimeSystem TimeSystem { get; set; }

    /// <summary>
    /// Gets or sets user's system's distance unit.
    /// </summary>
    public DistanceUnit DistanceUnit { get; set; }
}
