namespace Domain.Model;

/// <summary>
/// Represents a member of a group.
/// </summary>
public class GroupMember
{
    /// <summary>
    /// Gets or sets the unique identifier of the group.
    /// </summary>
    public int GroupId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the user.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the user is the creator of the group.
    /// </summary>
    public bool IsCreator { get; set; }
}
