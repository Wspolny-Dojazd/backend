namespace Domain.Model;

/// <summary>
/// Represents the route that leads to some location.
/// </summary>
public class Route
{
    /// <summary>
    /// Gets or sets the unique identifier of route.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets tip that helps to get to the location.
    /// </summary>
    public string Tip { get; set; }

    /// <summary>
    /// Gets or sets the location's latitude.
    /// </summary>
    public int Lat { get; set; }

    /// <summary>
    /// Gets or sets the location's longitude.
    /// </summary>
    public int Lon { get; set; }
}
