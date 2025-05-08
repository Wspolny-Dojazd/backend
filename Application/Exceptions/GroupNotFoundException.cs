using Shared.Enums.ErrorCodes;

namespace Application.Exceptions;

/// <summary>
/// Represents an exception thrown when a group is not found.
/// </summary>
public class GroupNotFoundException : AppException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GroupNotFoundException"/> class.
    /// </summary>
    /// <param name="id">The unique identifier of the group that was not found.</param>
    public GroupNotFoundException(int id)
        : this("ID", id.ToString())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupNotFoundException"/> class.
    /// </summary>
    /// <param name="joiningCode">The joining code of the group that was not found.</param>
    public GroupNotFoundException(string joiningCode)
        : this("joining code", joiningCode)
    {
    }

    private GroupNotFoundException(string identifierType, string identifier)
    : base(404, GroupErrorCode.GROUP_NOT_FOUND)
    {
    }
}
