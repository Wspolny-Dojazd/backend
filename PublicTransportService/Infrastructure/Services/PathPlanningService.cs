using PublicTransportService.Application.Interfaces;
using PublicTransportService.Application.PathFinding;
using PublicTransportService.Domain.Interfaces;
using PublicTransportService.Infrastructure.PathFinding.Raptor;

namespace PublicTransportService.Infrastructure.Services;

/// <summary>
/// Represents a service for path planning using the Raptor algorithm.
/// </summary>
/// <param name="dataCache">The data cache for Raptor algorithm context.</param>
/// <param name="stopRepository">The repository for accessing stop data.</param>
/// <param name="walkingTimeEstimator">The repository foasdfr accessing stop data.</param>
internal class PathPlanningService(IRaptorDataCache dataCache, IStopRepository stopRepository, IWalkingTimeEstimator walkingTimeEstimator)
    : IPathPlanningService
{
    /// <inheritdoc/>
    public async Task<IEnumerable<Dictionary<Guid, PathResult>>> ComputeGroupPathsAsync(
        double destLatitude,
        double destLongitude,
        DateTime arrivalTime,
        IEnumerable<(Guid, double, double)> userLocations)
    {
        var context = dataCache.GetContext();
        var raptor = new RaptorAlgorithm(context, stopRepository, walkingTimeEstimator);

        var paths = await raptor.FindPathsForUsersAsync(destLatitude, destLongitude, arrivalTime, userLocations);

        // Currently returns only one path set.
        // Multiple alternatives may be supported in the future to allow user choice.
        return [paths];
    }
}
