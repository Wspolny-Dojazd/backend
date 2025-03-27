namespace PublicTransportService.Domain.Entities;

/// <summary>
/// Represents a coordinate point used to define a route shape.
/// </summary>
public class Shape
{
    /// <summary>
    /// Gets the unique identifier of the shape point.
    /// </summary>
    public required string Id { get; init; }

    /// <summary>
    /// Gets the order of the point within the shape.
    /// </summary>
    public required int PtSequence { get; init; }

    /// <summary>
    /// Gets the latitude of the shape point.
    /// </summary>
    public required double PtLatitude { get; init; }

    /// <summary>
    /// Gets the longitude of the shape point.
    /// </summary>
    public required double PtLongitude { get; init; }
}
