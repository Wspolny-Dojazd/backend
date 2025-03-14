namespace Domain.Model;

/// <summary>
/// Location model class that defines its properties.
/// </summary>
public class Location
{
    /// <summary>
    /// Gets or sets location's Id.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets User's Id.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets location's lattitude.
    /// </summary>
    public int Lat { get; set; }

    /// <summary>
    /// Gets or sets location's longitude.
    /// </summary>
    public int Lon { get; set; }

    /// <summary>
    /// Gets or sets user.
    /// </summary>
    public User User { get; set; }
}
