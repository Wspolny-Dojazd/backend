namespace Application.DTOs.Path;

/// <summary>
/// Represents the type of a travel segment.
/// </summary>
public enum SegmentType
{
    /// <summary>
    /// A segment representing public transport (e.g. bus, tram).
    /// </summary>
    Route,

    /// <summary>
    /// A segment representing walking.
    /// </summary>
    Walk,
}
