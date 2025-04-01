namespace Domain.Model;

/// <summary>
/// Represents the current location of a user.
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
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the latitude coordinate.
    /// </summary>
    public double Lat { get; set; }

    /// <summary>
    /// Gets or sets the longitude coordinate.
    /// </summary>
    public double Lon { get; set; }

    /// <summary>
    /// Gets or sets the user associated with the location.
    /// </summary>
    public required User User { get; set; }
}
