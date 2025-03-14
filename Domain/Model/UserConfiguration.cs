using Domain.Enums;

namespace Domain.Model;

/// <summary>
/// UserConfiguration model class that defines its properties.
/// </summary>
public class UserConfiguration
{
    /// <summary>
    /// Gets or sets Configuration Id.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Gets or sets User Id.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets system's Language.
    /// </summary>
    public Language Language { get; set; }

    /// <summary>
    /// Gets or sets system's Time System.
    /// </summary>
    public TimeSystem TimeSystem { get; set; }

    /// <summary>
    /// Gets or sets system's Distance unit.
    /// </summary>
    public DistanceUnit DistanceUnit { get; set; }
}
