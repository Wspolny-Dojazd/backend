using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PublicTransportService.Infrastructure.Data;

namespace PublicTransportService.Infrastructure.PathFinding.Raptor;

/// <summary>
/// Represents a cache for RAPTOR pathfinding data.
/// </summary>
/// <remarks>
/// This cache improves performance by avoiding repetitive database access.
/// </remarks>
/// <param name="scopeFactory">The scope factory used to create service scopes for data access.</param>
internal class RaptorDataCache(IServiceScopeFactory scopeFactory) : IRaptorDataCache
{
    private RaptorContext? context;

    /// <inheritdoc/>
    public RaptorContext GetContext()
    {
        return this.context ?? throw new InvalidOperationException(
            "Raptor data not initialized. Call InitializeAsync first.");
    }

    /// <inheritdoc/>
    public async Task InitializeAsync()
    {
        using var scope = scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PTSDbContext>();

        var stops = await dbContext.Stops
            .AsNoTracking()
            .Select(s => new PathFindingStop(s.Id, s.LogicalId, s.Latitude, s.Longitude))
            .ToDictionaryAsync(s => s.Id);

        var trips = await dbContext.Trips
            .AsNoTracking()
            .Where(t => !EF.Functions.Like(t.Id, "M%")) // Skip metro for now
            .ToDictionaryAsync(t => t.Id);

        var tripBaseDates = trips.ToDictionary(
            kvp => kvp.Key,
            kvp => kvp.Value.ServiceDate.ToDateTime(TimeOnly.MinValue));

        var stopTimes = await dbContext.StopTimes
            .AsNoTracking()
            .ToListAsync();

        var stopTimesByTrip = stopTimes
            .Where(st => trips.ContainsKey(st.TripId))
            .Select(st => new PathFindingStopTime(
                TripId: st.TripId,
                StopId: st.StopId,
                ArrivalTime: tripBaseDates[st.TripId].AddSeconds(st.ArrivalTime),
                DepartureTime: tripBaseDates[st.TripId].AddSeconds(st.DepartureTime),
                StopSequence: st.StopSequence))
            .GroupBy(st => st.TripId)
            .ToDictionary(
                g => g.Key,
                g => g.OrderBy(x => x.StopSequence).ToList());

        this.context = new RaptorContext(stopTimesByTrip, stops);
    }
}
