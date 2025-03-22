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
    /// Gets or sets the group's joining code.
    /// </summary>
    public required string JoiningCode { get; set; }

    /// <summary>
    /// Gets or sets the group's starting status.
    /// </summary>
    public Status Status { get; set; }

    /// <summary>
    /// Gets or sets the group's destination latitude.
    /// </summary>
    public int DestinationLat { get; set; }

    /// <summary>
    /// Gets or sets the group's destination longitude.
    /// </summary>
    public int DestinationLon { get; set; }

    /// <summary>
    /// Gets or sets the group's list of routes.
    /// </summary>
    public required List<Route> Routes { get; set; }

    /// <summary>
    /// Gets or sets the group's list of users' live locations.
    /// </summary>
    public required List<Location> LiveLocations { get; set; }

    /// <summary>
    /// Gets or sets the group members.
    /// </summary>
    public required List<User> GroupMembers { get; set; }
}
