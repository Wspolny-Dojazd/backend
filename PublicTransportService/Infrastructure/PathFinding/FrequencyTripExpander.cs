using PublicTransportService.Application.PathFinding;
using PublicTransportService.Domain.Entities;

namespace PublicTransportService.Infrastructure.PathFinding;

/// <summary>
/// Generates synthetic trip instances from frequency definitions and base stop times.
/// </summary>
internal static class FrequencyTripExpander
{
    /// <summary>
    /// Generates all pathfinding stop times from frequency-based definitions.
    /// </summary>
    /// <param name="frequencies">The list of frequency-based trip definitions.</param>
    /// <param name="baseStopTimesByTrip">A dictionary of base stop times per trip ID.</param>
    /// <param name="minDate">The earliest date to consider for synthetic generation.</param>
    /// <param name="maxDate">The latest date to consider for synthetic generation.</param>
    /// <returns>A collection of fully resolved <see cref="PathFindingStopTime"/> entries.</returns>
    public static IEnumerable<PathFindingStopTime> GenerateTrips(
        IEnumerable<Frequency> frequencies,
        IReadOnlyDictionary<string, List<StopTime>> baseStopTimesByTrip,
        DateTime minDate,
        DateTime maxDate)
    {
        var result = new List<PathFindingStopTime>();

        foreach (var freq in frequencies)
        {
            if (!baseStopTimesByTrip.TryGetValue(freq.TripId, out var baseStops) || baseStops.Count == 0)
            {
                continue;
            }

            if (!TripIdUtils.TryGetDaysOfWeek(freq.TripId, out var targetDays))
            {
                continue;
            }

            var start = TimeSpan.FromSeconds(freq.StartTime);
            var end = TimeSpan.FromSeconds(freq.EndTime);

            for (var date = minDate.Date; date <= maxDate.Date; date = date.AddDays(1))
            {
                if (!targetDays.Contains(date.DayOfWeek))
                {
                    continue;
                }

                for (var t = start; t < end; t += TimeSpan.FromSeconds(freq.Headway))
                {
                    var tripStartTime = date + t;
                    var newTripId = $"{freq.TripId}@{date:yyyyMMdd}_{(int)t.TotalHours:00}{t.Minutes:00}";

                    var generated = baseStops.Select(st =>
                    {
                        var arrival = tripStartTime.AddSeconds(st.ArrivalTime);
                        var departure = tripStartTime.AddSeconds(st.DepartureTime);

                        return new PathFindingStopTime(
                            TripId: newTripId,
                            StopId: st.StopId,
                            ArrivalTime: arrival,
                            DepartureTime: departure,
                            StopSequence: st.StopSequence);
                    });

                    result.AddRange(generated);
                }
            }
        }

        return result;
    }
}
