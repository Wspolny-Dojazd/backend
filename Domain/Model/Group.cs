using Domain.Enums;

namespace Domain.Model;

/// <summary>
/// Represents a group of people.
/// </summary>
public class Group
{
    /// <summary>
    /// Gets or sets the group's unique identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets group's joining code.
    /// </summary>
    public string JoiningCode { get; set; }

    /// <summary>
    /// Gets or sets group's starting status.
    /// </summary>
    public Status Status { get; set; }

    /// <summary>
    /// Gets or sets group's destination latitude.
    /// </summary>
    public int DestinationLat { get; set; }

    /// <summary>
    /// Gets or sets group's destination longitude.
    /// </summary>
    public int DestinationLon { get; set; }

    /// <summary>
    /// Gets or sets group's list of routes.
    /// </summary>
    public List<Route> Routes { get; set; }

    /// <summary>
    /// Gets or sets group's list of users' live locations.
    /// </summary>
    public List<Location> LiveLocations { get; set; }
}