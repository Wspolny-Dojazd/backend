namespace PublicTransportService.Application.PathFinding;

/// <summary>
/// Provides utility methods for parsing and handling
/// trip identifiers used in GTFS-based pathfinding.
/// </summary>
public static class TripIdUtils
{
    /// <summary>
    /// Extracts the base trip ID by removing any custom suffix
    /// appended after the specified separator.
    /// </summary>
    /// <param name="tripId">The full trip ID (may include a suffix).</param>
    /// <param name="separator">
    /// The separator character used to delimit the base trip ID.
    /// </param>
    /// <returns>The base trip ID before the separator.</returns>
    public static string GetBaseTripId(string tripId, char separator)
    {
        if (string.IsNullOrEmpty(tripId))
        {
            throw new ArgumentException("Trip ID cannot be null or empty.", nameof(tripId));
        }

        var index = tripId.IndexOf(separator);
        return index > 0 ? tripId[..index] : tripId;
    }

    /// <summary>
    /// Determines whether the trip ID starts with
    /// a valid service date in the format <c>yyyy-MM-dd</c>.
    /// </summary>
    /// <param name="tripId">The GTFS trip ID to evaluate.</param>
    /// <returns>
    /// <see langword="true"/> if the trip ID begins with a valid date;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static bool IsTripIdWithDate(string tripId)
    {
        return tripId.Length >= 10 &&
               char.IsDigit(tripId[0]) &&
               DateOnly.TryParseExact(tripId[..10], "yyyy-MM-dd", out _);
    }

    /// <summary>
    /// Extracts the service date embedded in the trip ID,
    /// assuming it starts with a <c>yyyy-MM-dd</c> date.
    /// </summary>
    /// <param name="tripId">The full trip ID containing a date prefix.</param>
    /// <param name="separator">
    /// The separator character after the date part. Default is <c>':'</c>.
    /// </param>
    /// <returns>
    /// The parsed <see cref="DateTime"/> value representing
    /// the service date with a time of 00:00.
    /// </returns>
    /// <exception cref="FormatException">
    /// Thrown if the trip ID does not start with a valid date.
    /// </exception>
    public static DateTime GetServiceDate(string tripId, char separator = ':')
    {
        var baseId = GetBaseTripId(tripId, separator);
        var datePart = baseId[..10];

        return DateOnly.ParseExact(datePart, "yyyy-MM-dd").ToDateTime(TimeOnly.MinValue);
    }

    /// <summary>
    /// Attempts to extract the applicable days of the week from a trip ID suffix pattern.
    /// </summary>
    /// <param name="tripId">
    /// The trip ID from GTFS data, expected to contain a tag indicating service pattern,
    /// such as <see langword="NdM"/> (Sunday), <see langword="SbM"/> (Saturday),
    /// <see langword="PtM"/> (Friday), or <see langword="PcM"/> (Monday–Thursday).
    /// </param>
    /// <param name="daysOfWeek">
    /// When this method returns, contains the collection of <see cref="DayOfWeek"/> values
    /// for which the trip is valid, if recognized; otherwise, an empty collection.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if a known pattern was found and parsed;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static bool TryGetDaysOfWeek(string tripId, out IEnumerable<DayOfWeek> daysOfWeek)
    {
        var tag = tripId.Split(':').ElementAtOrDefault(1);

        daysOfWeek = tag switch
        {
            "NdM" => [DayOfWeek.Sunday],
            "SbM" => [DayOfWeek.Saturday],
            "PtM" => [DayOfWeek.Friday],
            "PcM" => [
                DayOfWeek.Monday,
                DayOfWeek.Tuesday,
                DayOfWeek.Wednesday,
                DayOfWeek.Thursday],
            _ => [],
        };

        return daysOfWeek.Any();
    }
}
