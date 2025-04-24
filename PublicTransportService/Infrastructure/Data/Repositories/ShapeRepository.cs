using Microsoft.EntityFrameworkCore;
using PublicTransportService.Application.PathFinding;
using PublicTransportService.Domain.Entities;
using PublicTransportService.Domain.Interfaces;

namespace PublicTransportService.Infrastructure.Data.Repositories;

/// <summary>
/// Provides data access operations for <see cref="Shape"/> entities.
/// </summary>
/// <param name="dbContext">The database context for accessing the database.</param>
internal class ShapeRepository(PTSDbContext dbContext)
    : IShapeRepository
{
    /// <inheritdoc/>
    public async Task<List<Shape>> GetSegmentShapesAsync(PathSegment segment)
    {
        var trip = await dbContext.Trips
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == segment.TripId)
            ?? throw new InvalidOperationException($"Trip with ID '{segment.TripId}' not found.");

        var stopIds = new HashSet<string> { segment.FromStopId, segment.ToStopId };
        var stops = await dbContext.Stops
            .AsNoTracking()
            .Where(s => stopIds.Contains(s.Id))
            .ToDictionaryAsync(s => s.Id);

        if (!stops.TryGetValue(segment.FromStopId, out var stop1))
        {
            throw new InvalidOperationException($"Stop with ID '{segment.FromStopId}' not found.");
        }

        if (!stops.TryGetValue(segment.ToStopId, out var stop2))
        {
            throw new InvalidOperationException($"Stop with ID '{segment.ToStopId}' not found.");
        }

        var shapes = await dbContext.Shapes
            .AsNoTracking()
            .Where(s => s.Id == trip.ShapeId)
            .OrderBy(s => s.PtSequence)
            .ToListAsync();

        if (shapes.Count == 0)
        {
            return [];
        }

        static double Distance(Shape shape, Stop stop)
            => Math.Sqrt(
                Math.Pow(shape.PtLatitude - stop.Latitude, 2) +
                Math.Pow(shape.PtLongitude - stop.Longitude, 2));

        var nearestToStop1 = shapes.MinBy(s => Distance(s, stop1))
                             ?? throw new InvalidOperationException("Missing shape point for stop1.");
        var nearestToStop2 = shapes.MinBy(s => Distance(s, stop2))
                             ?? throw new InvalidOperationException("Missing shape point for stop2.");

        int minSeq = Math.Min(nearestToStop1.PtSequence, nearestToStop2.PtSequence);
        int maxSeq = Math.Max(nearestToStop1.PtSequence, nearestToStop2.PtSequence);

        return [..shapes.Where(s => s.PtSequence >= minSeq && s.PtSequence <= maxSeq)];
    }
}
