using PublicTransportService.Application.PathFinding;
using PublicTransportService.Domain.Entities;

namespace PublicTransportService.Domain.Interfaces;

/// <summary>
/// Defines a contract for a repository that manages route shapes.
/// </summary>
public interface IShapeRepository
{
    /// <summary>
    /// Retrieves a list of shapes associated with a specific path segment.
    /// </summary>
    /// <param name="segment">The path segment for which to retrieve shapes.</param>
    /// <returns>The shapes associated with the specified path segment.</returns>
    Task<List<Shape>> GetSegmentShapesAsync(PathSegment segment);
}
