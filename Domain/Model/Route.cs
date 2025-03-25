namespace Domain.Model;

/// <summary>
/// Represents a tip or instruction leading to a specific location.
/// </summary>
public class Route
{
    /// <summary>
    /// Gets or sets the unique identifier of the route.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the tip or hint for navigating to the destination.
    /// </summary>
    public required string Tip { get; set; }

    /// <summary>
    /// Gets or sets the latitude coordinate of the location.
    /// </summary>
    public int Lat { get; set; }

    /// <summary>
    /// Gets or sets the longitude coordinate of the location.
    /// </summary>
    public int Lon { get; set; }
}
