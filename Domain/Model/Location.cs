namespace Domain.Model;

/// <summary>
/// Represents a user's current geographic location.
/// </summary>
public class Location
{
    /// <summary>
    /// Gets or sets the unique identifier of the location.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user associated with the location.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets the latitude coordinate.
    /// </summary>
    public int Lat { get; set; }

    /// <summary>
    /// Gets or sets the longitude coordinate.
    /// </summary>
    public int Lon { get; set; }

    /// <summary>
    /// Gets or sets the user associated with the location.
    /// </summary>
    public required User User { get; set; }
}
