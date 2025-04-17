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
    /// Gets or sets the unique identifier of the group creator.
    /// </summary>
    public required Guid CreatorId { get; set; }

    /// <summary>
    /// Gets or sets the current status of the group.
    /// </summary>
    public Status Status { get; set; }

    /// <summary>
    /// Gets or sets the group creator.
    /// </summary>
    public User Creator { get; set; } = default!;

    /// <summary>
    /// Gets or sets the accepted travel path of the group, if any.
    /// </summary>
    public GroupPath? CurrentPath { get; set; }

    /// <summary>
    /// Gets or sets the proposed paths of the group, if any.
    /// </summary>
    public List<ProposedPath> ProposedPaths { get; set; } = [];

    /// <summary>
    /// Gets or sets the users who are members of the group.
    /// </summary>
    public required List<User> GroupMembers { get; set; }
}
