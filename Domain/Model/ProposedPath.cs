namespace Domain.Model;

/// <summary>
/// Represents a proposed path for a group before it is accepted.
/// </summary>
public class ProposedPath
{
    /// <summary>
    /// Gets or sets the unique identifier of the proposed path.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the group to which the proposed path belongs.
    /// </summary>
    public int GroupId { get; set; }

    /// <summary>
    /// Gets or sets the UTC date and time when the proposed path was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the serialized JSON representation of the proposed path.
    /// </summary>
    public required string SerializedDto { get; set; }

    /// <summary>
    /// Gets or sets the group associated with this proposed path.
    /// </summary>
    public Group Group { get; set; } = default!;
}
