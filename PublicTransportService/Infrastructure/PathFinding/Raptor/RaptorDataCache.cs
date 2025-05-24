using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PublicTransportService.Application.PathFinding;
using PublicTransportService.Infrastructure.Data;

namespace PublicTransportService.Infrastructure.PathFinding.Raptor;

/// <summary>
/// Represents a cache for RAPTOR pathfinding data.
/// </summary>
/// <param name="scopeFactory">The scope factory used to create service scopes for data access.</param>
/// <remarks>
/// This cache improves performance by avoiding repetitive database access.
/// </remarks>
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
            .Select(s => new PathFindingStop(s.Id, s.LogicalId))
            .ToDictionaryAsync(s => s.Id);

        var trips = await dbContext.Trips
            .AsNoTracking()
            .ToDictionaryAsync(t => t.Id);

        var stopTimes = await dbContext.StopTimes
            .AsNoTracking()
            .ToListAsync();

        var frequencies = await dbContext.Frequencies
            .AsNoTracking()
            .ToListAsync();

        var stopTimesByTrip = stopTimes
            .GroupBy(st => st.TripId)
            .ToDictionary(g => g.Key, g => g.ToList());

        var datedTrips = trips
            .Where(kvp => TripIdUtils.IsTripIdWithDate(kvp.Key))
            .ToDictionary(
                kvp => kvp.Key,
                kvp => TripIdUtils.GetServiceDate(kvp.Key));

        var minDate = datedTrips.Values.Min();
        var maxDate = datedTrips.Values.Max();

        var pathFindingStopTimes = new List<PathFindingStopTime>();

        foreach (var st in stopTimes)
        {
            if (!datedTrips.TryGetValue(st.TripId, out var baseDate))
            {
                continue;
            }

            pathFindingStopTimes.Add(new PathFindingStopTime(
                TripId: st.TripId,
                StopId: st.StopId,
                ArrivalTime: baseDate.AddSeconds(st.ArrivalTime),
                DepartureTime: baseDate.AddSeconds(st.DepartureTime),
                StopSequence: st.StopSequence));
        }

        var generatedTrips = FrequencyTripExpander.GenerateTrips(
            frequencies,
            stopTimesByTrip,
            minDate,
            maxDate);

        pathFindingStopTimes.AddRange(generatedTrips);

        var final = pathFindingStopTimes
            .GroupBy(st => st.TripId)
            .ToDictionary(g => g.Key, g => g.OrderBy(x => x.StopSequence).ToList());

        this.context = new RaptorContext(final, stops);
    }
}
