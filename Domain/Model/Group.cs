using Domain.Enums;

namespace Domain.Model;

/// <summary>
/// Represents a group of users traveling to a common destination.
/// </summary>
public class Group
{
    /// <summary>
    /// Gets or sets the unique identifier of the group.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the code used to join the group.
    /// </summary>
    public required string JoiningCode { get; set; }

    /// <summary>
    /// Gets or sets the current status of the group.
    /// </summary>
    public Status Status { get; set; }

    /// <summary>
    /// Gets or sets the destination latitude of the group.
    /// </summary>
    public int DestinationLat { get; set; }

    /// <summary>
    /// Gets or sets the destination longitude of the group.
    /// </summary>
    public int DestinationLon { get; set; }

    /// <summary>
    /// Gets or sets the routes associated with the group.
    /// </summary>
    public required List<Route> Routes { get; set; }

    /// <summary>
    /// Gets or sets the live locations of group members.
    /// </summary>
    public required List<Location> LiveLocations { get; set; }

    /// <summary>
    /// Gets or sets the users who are members of the group.
    /// </summary>
    public required List<User> GroupMembers { get; set; }
}
