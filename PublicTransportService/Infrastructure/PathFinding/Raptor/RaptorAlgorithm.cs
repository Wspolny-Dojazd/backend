using PublicTransportService.Application.Interfaces;
using PublicTransportService.Application.PathFinding;
using PublicTransportService.Domain.Interfaces;

namespace PublicTransportService.Infrastructure.PathFinding.Raptor;

/// <summary>
/// Represents the RAPTOR algorithm for pathfinding.
/// </summary>
/// <param name="context">The context containing the data needed for pathfinding.</param>
/// <param name="stopRepository">The repository for accessing stop data.</param>
internal class RaptorAlgorithm(RaptorContext context, IStopRepository stopRepository, IWalkingTimeEstimator walkingTimeEstimator)
{
    private const int HourLimit = 3; // Temp solution: Time window for backward search from the arrival time
    private const int WalkTimeMin = 1; // Temp solution: Walking time in minutes between stops

    // Maintain pre-sorted stop times for each trip (for forward and backward traversal).
    private readonly Dictionary<string, List<PathFindingStopTime>> ascendingStopTimesByTrip = [];
    private readonly Dictionary<string, List<PathFindingStopTime>> descendingStopTimesByTrip = [];

    /// <summary>
    /// Calculates optimal paths for each user to a shared destination, working backwards from the arrival time.
    /// </summary>
    /// <param name="destLat">The latitude of the destination.</param>
    /// <param name="destLon">The longitude of the destination.</param>
    /// <param name="arrivalTime">The desired arrival time at the destination.</param>
    /// <param name="userStartLocations">The list of user locations with their unique identifiers and coordinates.</param>
    /// <returns>A dictionary mapping each user to their computed path result.</returns>
    public async Task<Dictionary<Guid, PathResult>> FindPathsForUsersAsync(
        double destLat,
        double destLon,
        DateTime arrivalTime,
        IEnumerable<(Guid UserId, double Lat, double Lon)> userStartLocations)
    {
        /* === PHASE 1: Group stops by their logical ID, and initialize "bestStates" === */

        var stopsByLogicalId = context.Stops.Values
            .GroupBy(s => s.LogicalId)
            .ToDictionary(g => g.Key, g => g.Select(s => s.Id).ToHashSet());

        var logicalIdByStop = context.Stops.Values
            .ToDictionary(s => s.Id, s => s.LogicalId);

        // bestStates[stopId] holds the best backward state found so far.
        // Initialize each stop as unreachable.
        var bestStates = context.Stops.Keys.ToDictionary(
            stopId => stopId,
            _ => new BestState(DateTime.MinValue, int.MaxValue, null));

        // backPointers will help us reconstruct paths in a forward direction later.
        var backPointers = new Dictionary<string, BackPointer>();

        // Identify the logical ID of the target location (closest group of stops),
        // then mark all those stops as starting points (in backward logic) with 0 transfers.
        var targetLogicalId = await stopRepository.GetNearestLogicalIdStopAsync(destLat, destLon);
        var targetStops = stopsByLogicalId[targetLogicalId];

        // A queue for BFS-like processing
        var queue = new Queue<string>();

        // Mark each target stop as the initial backward state.
        foreach (var stopId in targetStops)
        {
            bestStates[stopId] = new BestState(arrivalTime, 0, null);
            backPointers[stopId] = new BackPointer(stopId, "init", arrivalTime, 0);
            queue.Enqueue(stopId);
        }

        /* === PHASE 2: Filter out only the relevant trips within a X-hour window before arrivalTime === */

        var minTime = arrivalTime.AddHours(-HourLimit);
        var relevantTrips = context.StopTimesByTrip
            .Where(x => x.Value.Any(st =>
                st.ArrivalTime >= minTime &&
                st.DepartureTime <= arrivalTime))
            .ToDictionary(x => x.Key, x => x.Value);

        // Pre-sort stop times for each relevant trip (ascending and descending).
        foreach (var (tripId, stopTimes) in relevantTrips)
        {
            var asc = stopTimes.OrderBy(st => st.StopSequence).ToList();
            var desc = stopTimes.OrderByDescending(st => st.StopSequence).ToList();
            this.ascendingStopTimesByTrip[tripId] = asc;
            this.descendingStopTimesByTrip[tripId] = desc;
        }

        // For faster backward lookup from a stop to the trips serving it:
        var tripIdsByStop = new Dictionary<string, HashSet<string>>();
        foreach (var (tripId, stopTimes) in relevantTrips)
        {
            foreach (var st in stopTimes)
            {
                if (!tripIdsByStop.TryGetValue(st.StopId, out var set))
                {
                    set = [];
                    tripIdsByStop[st.StopId] = set;
                }

                _ = set.Add(tripId);
            }
        }

        /* === PHASE 3: Main BFS loop (working backward from the desired arrival time) === */

        while (queue.Count > 0)
        {
            var currentStop = queue.Dequeue();
            var currentState = bestStates[currentStop];

            // 3a) For each trip that goes through currentStop, check earlier stops in that trip (in backward sense).
            if (tripIdsByStop.TryGetValue(currentStop, out var tripIds))
            {
                foreach (var tripId in tripIds)
                {
                    if (!this.descendingStopTimesByTrip.TryGetValue(tripId, out var descStopTimes))
                    {
                        continue;
                    }

                    var idxCurrent = descStopTimes.FindIndex(st => st.StopId == currentStop);
                    if (idxCurrent < 0)
                    {
                        continue;
                    }

                    var stCurrent = descStopTimes[idxCurrent];

                    // We can only continue backward if the currentStop's best departure time
                    // is >= the arrivalTime recorded at stCurrent.
                    if (currentState.DepartureTime < stCurrent.ArrivalTime)
                    {
                        continue;
                    }

                    // Determine if changing trips increases transfers count.
                    var transfers = (currentState.LastTripId == tripId)
                        ? currentState.Transfers
                        : currentState.Transfers + 1;

                    // limitTime is the earliest time we can still board this trip when going backward.
                    var limitTime = stCurrent.ArrivalTime;

                    // Look for earlier (in forward sense) stops in descending order.
                    for (int i = idxCurrent + 1; i < descStopTimes.Count; i++)
                    {
                        var stPrev = descStopTimes[i];

                        // If stPrev departs after limitTime, no need to check further.
                        if (stPrev.DepartureTime > limitTime)
                        {
                            break;
                        }

                        var candidateState = new BestState(
                            stPrev.DepartureTime,
                            transfers,
                            tripId);

                        if (IsStopBetter(candidateState, bestStates[stPrev.StopId]))
                        {
                            bestStates[stPrev.StopId] = candidateState;
                            queue.Enqueue(stPrev.StopId);

                            backPointers[stPrev.StopId] = new BackPointer(
                                currentStop,
                                tripId,
                                stPrev.DepartureTime,
                                transfers);
                        }
                    }
                }
            }

            // 3b) Handle walking transfers between sibling stops of the same LogicalId.
            if (logicalIdByStop.TryGetValue(currentStop, out var logicalId) &&
                stopsByLogicalId.TryGetValue(logicalId, out var siblingStops))
            {
                foreach (var siblingId in siblingStops)
                {
                    if (siblingId == currentStop)
                    {
                        continue;
                    }

                    var depStop = await stopRepository.GetByIdAsync(currentStop);
                    var destStop = await stopRepository.GetByIdAsync(siblingId);

                    var walkTime = currentState.DepartureTime.AddMinutes(-WalkTimeMin);
                    // var walkTime = currentState.DepartureTime.AddSeconds(
                    //     -walkingTimeEstimator.GetWalkingTimeEstimate(
                    //         depStop.Latitude, depStop.Longitude, destStop.Latitude, destStop.Longitude));

                    // Avoid going below the min DateTime range.
                    if (walkTime < DateTime.MinValue.AddDays(1))
                    {
                        continue;
                    }

                    var candidate = new BestState(
                        walkTime,
                        currentState.Transfers,
                        currentState.LastTripId);

                    if (IsStopBetter(candidate, bestStates[siblingId]))
                    {
                        bestStates[siblingId] = candidate;
                        queue.Enqueue(siblingId);

                        backPointers[siblingId] = new BackPointer(
                            currentStop,
                            null,
                            walkTime,
                            candidate.Transfers);
                    }
                }
            }
        }

        /* === PHASE 4: For each user, find the best reachable stop near
         * their start location and reconstruct the path ===
         */

        var results = new Dictionary<Guid, PathResult>();

        foreach (var (userId, lat, lon) in userStartLocations)
        {
            // Identify the logicalId for the user's start location.
            var userLogicalId = await stopRepository.GetNearestLogicalIdStopAsync(lat, lon);
            var candidateStops = stopsByLogicalId[userLogicalId];

            // Among the user's nearby stops, find the best (greatest departure time).
            string? bestStop = null;
            var best = new BestState(DateTime.MinValue, int.MaxValue, null);

            foreach (var stopId in candidateStops)
            {
                if (IsStopBetter(bestStates[stopId], best))
                {
                    best = bestStates[stopId];
                    bestStop = stopId;
                }
            }

            // If no route was found, provide an empty result.
            if (bestStop == null || best.DepartureTime == DateTime.MinValue)
            {
                results[userId] = new PathResult(DateTime.MinValue, []);
                continue;
            }

            // Otherwise, reconstruct the path in forward (chronological) order.
            var pathSegments = this.ReconstructPath(bestStop, backPointers, bestStates);
            results[userId] = new PathResult(best.DepartureTime, pathSegments);
        }

        return results;
    }

    private static bool IsStopBetter(BestState candidate, BestState current)
    {
        // Currently, this comparison is based only on DepartureTime.
        // In the future, we will consider more complex criteria such as
        // fewer transfers, penalizing on-demand rides, etc.
        // Note: more transfers may actually help the frontend test diverse UI scenarios.
        return candidate.DepartureTime > current.DepartureTime;
    }

    private List<PathSegment> ReconstructPath(
        string startStopId,
        Dictionary<string, BackPointer> backPointers,
        Dictionary<string, BestState> bestStates)
    {
        var path = new List<PathSegment>();
        var visited = new HashSet<string>();
        var currentStop = startStopId;

        while (backPointers.TryGetValue(currentStop, out var bp))
        {
            // If the pointer references itself or we've already
            // visited the previous stop, break to avoid loops.
            if (bp.PreviousStopId == currentStop || !visited.Add(bp.PreviousStopId))
            {
                break;
            }

            var nextStop = bp.PreviousStopId;

            // 'departureTime' is when we leave 'currentStop' in forward logic.
            var departureTime = bestStates[currentStop].DepartureTime;

            // 'arrivalTime' in the backPointer is when we arrive at 'nextStop' in forward logic.
            var arrivalTime = bp.ArrivalTime;

            // If TripId is null, it's a walking link.
            if (bp.TripId == null)
            {
                path.Add(new PathSegment(currentStop, nextStop, null, departureTime, arrivalTime));
            }

            // Otherwise, it's a trip-based segment, and we can expand intermediate stops if we have them.
            else if (this.ascendingStopTimesByTrip.TryGetValue(bp.TripId, out var ascStopTimes))
            {
                var idxA = ascStopTimes.FindIndex(st => st.StopId == currentStop);
                var idxB = ascStopTimes.FindIndex(st => st.StopId == nextStop);

                if (idxA > idxB)
                {
                    (idxA, idxB) = (idxB, idxA);
                }

                // If both stops exist in the trip in correct order,
                // expand the route with intermediate stops.
                if (idxA >= 0 && idxB > idxA)
                {
                    for (int i = idxA; i < idxB; i++)
                    {
                        var stFrom = ascStopTimes[i];
                        var stTo = ascStopTimes[i + 1];

                        var segDepTime = (i == idxA) ? departureTime : stFrom.DepartureTime;
                        var segArrTime = stTo.ArrivalTime;

                        path.Add(new PathSegment(
                            stFrom.StopId,
                            stTo.StopId,
                            bp.TripId,
                            segDepTime,
                            segArrTime));
                    }
                }
                else
                {
                    // Fallback: just make a single segment if indexing doesn't line up.
                    path.Add(new PathSegment(currentStop, nextStop, bp.TripId, departureTime, arrivalTime));
                }
            }
            else
            {
                // Trip data is missing in ascendingStopTimes => fallback to a single segment.
                path.Add(new PathSegment(currentStop, nextStop, bp.TripId, departureTime, arrivalTime));
            }

            currentStop = nextStop;
        }

        return path;
    }
}
