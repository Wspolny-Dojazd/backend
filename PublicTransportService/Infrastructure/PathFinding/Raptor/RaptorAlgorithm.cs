using PublicTransportService.Application.PathFinding;
using PublicTransportService.Domain.Interfaces;

namespace PublicTransportService.Infrastructure.PathFinding.Raptor;

/// <summary>
/// Represents the RAPTOR algorithm for pathfinding.
/// </summary>
/// <param name="context">The context containing the data needed for pathfinding.</param>
/// <param name="stopRepository">The repository for accessing stop data.</param>
internal class RaptorAlgorithm(RaptorContext context, IStopRepository stopRepository)
{
    private const int HourLimit = 3; // Temp solution: Time window for backward search from the arrival time
    private const int WalkTimeMin = 1; // Temp solution: Walking time in minutes between stops

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
        // PHASE 1: Create lookups for logical (grouped) stops and initialize "bestStates"
        //          for all stops as "unreachable", then set states for the target stops.

        // Group stops by their LogicalId.
        var stopsByLogicalId = context.Stops.Values
            .GroupBy(s => s.LogicalId)
            .ToDictionary(g => g.Key, g => g.Select(s => s.Id).ToHashSet());

        // Map each stop to its LogicalId so we can find its "siblings."
        var logicalIdByStop = context.Stops.Values
            .ToDictionary(s => s.Id, s => s.LogicalId);

        // For each stopId, store the best backward state found so far
        //    (departure time, transfers, lastTripId). Initialize them to the "worst" values.
        var bestStates = context.Stops.Keys.ToDictionary(
            id => id,
            _ => new BestState(DateTime.MinValue, int.MaxValue, null));

        // Reconstruct how we got to a particular stop in backward logic.
        var backPointers = new Dictionary<string, BackPointer>();

        // Retrieve the logicalId of the target location (closest group of stops),
        // then mark those stops as "destination stops," meaning we can "start" from them
        // backward at arrivalTime with 0 transfers.
        var targetLogicalId = await stopRepository.GetNearestLogicalIdStopAsync(destLat, destLon);
        var targetStops = stopsByLogicalId[targetLogicalId];

        // Use a queue (BFS-like) to process stops as we discover better backward states.
        var queue = new Queue<string>();

        // Mark each target stop as having an initial (arrivalTime, 0 transfers).
        foreach (var stopId in targetStops)
        {
            bestStates[stopId] = new BestState(arrivalTime, 0, null);
            backPointers[stopId] = new BackPointer(stopId, "init", arrivalTime, 0);
            queue.Enqueue(stopId);
        }

        // PHASE 2: Filter trip data to only those relevant in a certain time window
        //          (arrivals not too far before desired arrivalTime - temporary solution).
        var relevantTrips = context.StopTimesByTrip
            .Where(x => x.Value.Any(
                st => st.ArrivalTime >= arrivalTime.AddHours(-HourLimit) &&
                    st.DepartureTime <= arrivalTime))
            .ToDictionary(x => x.Key, x => x.Value);

        // For faster lookup, build a dictionary of tripIds by stop.
        var tripIdsByStop = new Dictionary<string, HashSet<string>>();
        foreach (var (tripId, stopTimes) in relevantTrips)
        {
            foreach (var st in stopTimes)
            {
                _ = tripIdsByStop.TryAdd(st.StopId, []);
                _ = tripIdsByStop[st.StopId].Add(tripId);
            }
        }

        // PHASE 3: Main backward search loop
        // Extract stops from 'queue' and see if we can improve earlier stops that lead to them.
        while (queue.Count > 0)
        {
            var currentStop = queue.Dequeue();
            var currentState = bestStates[currentStop];

            // 3a) For each trip passing through currentStop, see if we can get "earlier" stops in that trip
            //     (i.e., stops with a higher StopSequence in backward ordering).
            if (tripIdsByStop.TryGetValue(currentStop, out var tripIds))
            {
                foreach (var tripId in tripIds)
                {
                    if (!relevantTrips.TryGetValue(tripId, out var stopTimes))
                    {
                        continue;
                    }

                    var ordered = stopTimes.OrderByDescending(st => st.StopSequence).ToList();
                    var idxCurrent = ordered.FindIndex(st => st.StopId == currentStop);
                    if (idxCurrent < 0)
                    {
                        continue;
                    }

                    var stCurrent = ordered[idxCurrent];

                    // If we can't "alight" in backward sense (we want bestStates to have departureTime >= arrivalTime),
                    // skip this route.
                    if (currentState.DepartureTime < stCurrent.ArrivalTime)
                    {
                        continue;
                    }

                    // Determine transfers: if changing tripId vs. LastTripId, increment.
                    var transfers = currentState.LastTripId == tripId
                        ? currentState.Transfers
                        : currentState.Transfers + 1;

                    var limitTime = stCurrent.ArrivalTime;

                    // For each earlier stop in the trip (in real forward direction),
                    // check if we can improve its backward departure time.
                    for (int i = idxCurrent + 1; i < ordered.Count; i++)
                    {
                        var stPrev = ordered[i];
                        if (stPrev.DepartureTime > limitTime)
                        {
                            break;
                        }

                        var candidateState = new BestState(
                            stPrev.DepartureTime,
                            transfers,
                            tripId);

                        // If this candidate improves on the old best state, update it and queue the stop.
                        if (IsStopBetter(candidateState, bestStates[stPrev.StopId]))
                        {
                            bestStates[stPrev.StopId] = candidateState;
                            queue.Enqueue(stPrev.StopId);

                            backPointers[stPrev.StopId] = new BackPointer(
                                currentStop, tripId, stPrev.DepartureTime, transfers);
                        }
                    }
                }
            }

            // 3b) Handle walking within a group of physical stops (same LogicalId),
            //     to allow easy on-foot transfers between sibling stops in the same location.
            if (logicalIdByStop.TryGetValue(currentStop, out var logicalId) &&
                stopsByLogicalId.TryGetValue(logicalId, out var siblingStops))
            {
                foreach (var siblingId in siblingStops)
                {
                    if (siblingId == currentStop)
                    {
                        continue;
                    }

                    // Subtract walking time from currentStop's best departure time
                    var walkTime = currentState.DepartureTime.AddMinutes(-WalkTimeMin);

                    // Skip if result underflows below valid DateTime range
                    // (protects against DateTime.MinValue errors)
                    if (walkTime < DateTime.MinValue.AddDays(1))
                    {
                        continue;
                    }

                    var candidateState = new BestState(
                        walkTime,
                        currentState.Transfers,
                        currentState.LastTripId);

                    // If this candidate improves on the old best state, update it and queue the stop.
                    if (IsStopBetter(candidateState, bestStates[siblingId]))
                    {
                        bestStates[siblingId] = candidateState;
                        queue.Enqueue(siblingId);

                        backPointers[siblingId] = new BackPointer(
                            currentStop, null, walkTime, candidateState.Transfers);
                    }
                }
            }
        }

        // PHASE 4: For each user, pick the best stop near their start location
        //          and reconstruct the backward path in forward order.
        var results = new Dictionary<Guid, PathResult>();
        foreach (var (userId, lat, lon) in userStartLocations)
        {
            // Identify the logicalId of the user's start location
            var logicalId = await stopRepository.GetNearestLogicalIdStopAsync(lat, lon);
            var candidateStops = stopsByLogicalId[logicalId];

            // Find the best physically close stop for this user
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

            // If no route found, produce empty result
            if (bestStop == null || best.DepartureTime == DateTime.MinValue)
            {
                results[userId] = new PathResult(DateTime.MinValue, []);
                continue;
            }

            // Otherwise, reconstruct path from that stop
            var path = ReconstructPath(context, bestStop, backPointers, bestStates);
            results[userId] = new PathResult(best.DepartureTime, path);
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

    private static List<PathSegment> ReconstructPath(
        RaptorContext context,
        string startStopId,
        Dictionary<string, BackPointer> backPointers,
        Dictionary<string, BestState> bestStates)
    {
        var path = new List<PathSegment>();
        var visited = new HashSet<string>();
        var currentStop = startStopId;

        while (backPointers.TryGetValue(currentStop, out var bp))
        {
            // If the pointer references itself, or we've seen it before, we're done
            if (bp.PreviousStopId == currentStop || !visited.Add(bp.PreviousStopId))
            {
                break;
            }

            var nextStop = bp.PreviousStopId;

            // The bestStates entry for currentStop tells us the departure time from 'currentStop' in forward sense.
            var departureTime = bestStates[currentStop].DepartureTime;

            // The backPointer uses arrivalTime to indicate when we get to nextStop in forward sense.
            var arrivalTime = bp.ArrivalTime;

            // If TripId is null, it's a walking link.
            if (bp.TripId is null)
            {
                path.Add(new PathSegment(currentStop, nextStop, null, departureTime, arrivalTime));
            }
            else if (context.StopTimesByTrip.TryGetValue(bp.TripId, out var stopTimes))
            {
                // Expand the route by listing all intermediate stops.
                var ordered = stopTimes.OrderBy(x => x.StopSequence).ToList();
                int idxA = ordered.FindIndex(st => st.StopId == currentStop);
                int idxB = ordered.FindIndex(st => st.StopId == nextStop);

                if (idxA > idxB)
                {
                    (idxA, idxB) = (idxB, idxA);
                }

                if (idxA >= 0 && idxB > idxA)
                {
                    // Build small segments for each consecutive pair of stops in the trip.
                    for (int i = idxA; i < idxB; i++)
                    {
                        var stFrom = ordered[i];
                        var stTo = ordered[i + 1];

                        var segDepTime = (i == idxA) ? departureTime : stFrom.DepartureTime;
                        var segArrTime = (i + 1 == idxB) ? arrivalTime : stTo.ArrivalTime;

                        path.Add(new PathSegment(stFrom.StopId, stTo.StopId, bp.TripId, segDepTime, segArrTime));
                    }
                }
                else
                {
                    // Fallback if we can't expand the full list of intermediate stops
                    path.Add(new PathSegment(currentStop, nextStop, bp.TripId, departureTime, arrivalTime));
                }
            }
            else
            {
                // Trip is missing from the data => fallback to one big segment
                path.Add(new PathSegment(currentStop, nextStop, bp.TripId, departureTime, arrivalTime));
            }

            currentStop = nextStop;
        }

        return path;
    }
}
