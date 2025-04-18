namespace Domain.Model;

/// <summary>
/// Represents the travel path that has been accepted by the group.
/// </summary>
public class GroupPath
{
    /// <summary>
    /// Gets or sets the unique identifier of the group path.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the group for which the path was accepted.
    /// </summary>
    public int GroupId { get; set; }

    /// <summary>
    /// Gets or sets the UTC date and time when the path was accepted.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the serialized JSON representation of the group's accepted path.
    /// </summary>
    public required string SerializedDto { get; set; }

    /// <summary>
    /// Gets or sets the group associated with this path.
    /// </summary>
    public Group Group { get; set; } = default!;
}
