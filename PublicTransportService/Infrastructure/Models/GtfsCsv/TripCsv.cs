namespace PublicTransportService.Infrastructure.Models.GtfsCsv;

#pragma warning disable IDE1006, SA1300

/// <summary>
/// Maps GTFS route data from trips.txt.
/// </summary>
internal record class TripCsv(
    string trip_id,
    string route_id,
    string service_id,
    string shape_id,
    string trip_short_name,
    string trip_headsign,
    string direction_id,
    string wheelchair_accessible,
    string? hidden_block_id,
    string? brigade,
    string? fleet_type);
