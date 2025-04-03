namespace Domain.Model;

/// <summary>
/// Represents a user's geographical location.
/// </summary>
public class UserLocation
{
    /// <summary>
    /// Gets or sets the unique identifier of the location.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the user ID associated with this location.
    /// This also serves as the primary key.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the latitude coordinate.
    /// </summary>
    public double? Latitude { get; set; }

    /// <summary>
    /// Gets or sets the longitude coordinate.
    /// </summary>
    public double? Longitude { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when this location was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}
