namespace Domain.Model;

/// <summary>
/// Route model class that defines its properties.
/// </summary>
public class Route
{
    /// <summary>
    /// Gets or sets Route Id.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets Tip.
    /// </summary>
    public string Tip { get; set; }

    /// <summary>
    /// Gets or sets Latitude.
    /// </summary>
    public int Lat { get; set; }

    /// <summary>
    /// Gets or sets Longitude.
    /// </summary>
    public int Lon { get; set; }
}
