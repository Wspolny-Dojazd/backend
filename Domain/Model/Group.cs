using Domain.Enums;

namespace Domain.Model;

/// <summary>
/// Group model class that defines its properties.
/// </summary>
public class Group
{
    /// <summary>
    /// Gets or sets group's Id.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets group's JoiningCode.
    /// </summary>
    public string JoiningCode { get; set; }

    /// <summary>
    /// Gets or sets group's Status.
    /// </summary>
    public Status Status { get; set; }

    /// <summary>
    /// Gets or sets group's Destination Latitude.
    /// </summary>
    public int DestinationLat { get; set; }

    /// <summary>
    /// Gets or sets group's Destination Longitude.
    /// </summary>
    public int DestinationLon { get; set; }

    /// <summary>
    /// Gets or sets group's list of Routes.
    /// </summary>
    public List<Route> Routes { get; set; }

    /// <summary>
    /// Gets or sets group's list of users' live locations.
    /// </summary>
    public List<Location> LiveLocations { get; set; }
}