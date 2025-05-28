using System.Globalization;
using PublicTransportService.Domain.Entities;
using PublicTransportService.Domain.Enums;
using PublicTransportService.Infrastructure.Models.GtfsCsv;

namespace PublicTransportService.Infrastructure.Mapping;

/// <summary>
/// Provides asynchronous mapping functions to convert GTFS CSV records into domain entities.
/// </summary>
internal static class GtfsCsvMapper
{
    /// <summary>
    /// Maps a stream of <see cref="StopCsv"/> records to <see cref="Stop"/> entities.
    /// </summary>
    /// <param name="csvs">An async enumerable of CSV records.</param>
    /// <returns>Async enumerable of mapped <see cref="Stop"/> entities.</returns>
    public static async IAsyncEnumerable<Stop> ParseStops(IAsyncEnumerable<StopCsv> csvs)
    {
        await foreach (var csv in csvs)
        {
            yield return new Stop
            {
                Id = csv.stop_id,
                Name = csv.stop_name,
                Code = csv.stop_code,
                PlatformCode = csv.platform_code,
                Latitude = double.Parse(csv.stop_lat, CultureInfo.InvariantCulture),
                Longitude = double.Parse(csv.stop_lon, CultureInfo.InvariantCulture),
                LocationType = (LocationType)int.Parse(csv.location_type),
                ParentStation = csv.parent_station,
                WheelchairBoarding = csv.wheelchair_boarding == "1",
                NameStem = csv.stop_name_stem,
                TownName = csv.town_name,
                StreetName = csv.street_name,
            };
        }
    }

    /// <summary>
    /// Maps a stream of <see cref="RouteCsv"/> records to <see cref="Route"/> entities.
    /// </summary>
    /// <param name="csvs">An async enumerable of CSV records.</param>
    /// <returns>Async enumerable of mapped <see cref="Route"/> entities.</returns>
    public static async IAsyncEnumerable<Route> ParseRoutes(IAsyncEnumerable<RouteCsv> csvs)
    {
        await foreach (var csv in csvs)
        {
            yield return new Route
            {
                Id = csv.route_id,
                AgencyId = csv.agency_id,
                ShortName = csv.route_short_name,
                LongName = csv.route_long_name,
                Type = (RouteType)int.Parse(csv.route_type),
                Color = csv.route_color,
                TextColor = csv.route_text_color,
            };
        }
    }

    /// <summary>
    /// Maps a stream of <see cref="ShapeCsv"/> records to <see cref="Shape"/> entities.
    /// </summary>
    /// <param name="csvs">An async enumerable of CSV records.</param>
    /// <returns>Async enumerable of mapped <see cref="Shape"/> entities.</returns>
    public static async IAsyncEnumerable<Shape> ParseShapes(IAsyncEnumerable<ShapeCsv> csvs)
    {
        await foreach (var csv in csvs)
        {
            yield return new Shape
            {
                Id = csv.shape_id,
                PtSequence = int.Parse(csv.shape_pt_sequence),
                PtLatitude = double.Parse(csv.shape_pt_lat, CultureInfo.InvariantCulture),
                PtLongitude = double.Parse(csv.shape_pt_lon, CultureInfo.InvariantCulture),
            };
        }
    }

    /// <summary>
    /// Maps a stream of <see cref="TripCsv"/> records to <see cref="Trip"/> entities.
    /// </summary>
    /// <param name="csvs">An async enumerable of CSV records.</param>
    /// <returns>Async enumerable of mapped <see cref="Trip"/> entities.</returns>
    public static async IAsyncEnumerable<Trip> ParseTrips(IAsyncEnumerable<TripCsv> csvs)
    {
        await foreach (var csv in csvs)
        {
            yield return new Trip
            {
                Id = csv.trip_id,
                RouteId = csv.route_id,
                ServiceId = csv.service_id,
                HeadSign = csv.trip_headsign,
                ShortName = csv.trip_short_name,
                ShapeId = csv.shape_id,
                DirectionId = int.Parse(csv.direction_id),
                WheelchairAccessible = csv.wheelchair_accessible == "1",
                HiddenBlockId = csv.hidden_block_id,
                Brigade = csv.brigade,
                FleetType = csv.fleet_type,
            };
        }
    }

    /// <summary>
    /// Maps a stream of <see cref="StopTimeCsv"/> records to <see cref="StopTime"/> entities.
    /// </summary>
    /// <param name="csvs">An async enumerable of CSV records.</param>
    /// <returns>Async enumerable of mapped <see cref="StopTime"/> entities with auto-incremented IDs.</returns>
    public static async IAsyncEnumerable<StopTime> ParseStopTimes(IAsyncEnumerable<StopTimeCsv> csvs)
    {
        var currentId = 1;
        await foreach (var csv in csvs)
        {
            yield return new StopTime
            {
                Id = currentId++,
                TripId = csv.trip_id,
                StopId = csv.stop_id,
                StopSequence = int.Parse(csv.stop_sequence),
                ArrivalTime = ParseTimeToSeconds(csv.arrival_time),
                DepartureTime = ParseTimeToSeconds(csv.departure_time),
                PickupType = (PickupType)int.Parse(csv.pickup_type),
                DropOffType = (DropOffType)int.Parse(csv.drop_off_type),
            };
        }
    }

    /// <summary>
    /// Maps a stream of <see cref="FrequencyCsv"/> records to <see cref="Frequency"/> entities.
    /// </summary>
    /// <param name="csvs">An async enumerable of CSV records.</param>
    /// <returns>Async enumerable of mapped <see cref="Frequency"/> entities with auto-incremented IDs.</returns>
    public static async IAsyncEnumerable<Frequency> ParseFrequencies(IAsyncEnumerable<FrequencyCsv> csvs)
    {
        var currentId = 1;
        await foreach (var csv in csvs)
        {
            yield return new Frequency
            {
                Id = currentId++,
                TripId = csv.trip_id,
                StartTime = ParseTimeToSeconds(csv.start_time),
                EndTime = ParseTimeToSeconds(csv.end_time),
                Headway = int.Parse(csv.headway_secs),
                ExactTimes = int.Parse(csv.exact_times),
            };
        }
    }

    private static int ParseTimeToSeconds(string time)
    {
        var parts = time.Split(':');
        return parts.Length != 3
            || !int.TryParse(parts[0], out var h)
            || !int.TryParse(parts[1], out var m)
            || !int.TryParse(parts[2], out var s)
            ? throw new FormatException($"Invalid GTFS time format: '{time}'")
            : (h * 3600) + (m * 60) + s;
    }
}
