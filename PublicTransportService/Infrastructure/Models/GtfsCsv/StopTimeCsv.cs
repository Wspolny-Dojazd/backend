namespace PublicTransportService.Infrastructure.Models.GtfsCsv;

#pragma warning disable IDE1006, SA1300

/// <summary>
/// Maps GTFS stop time data from stop_times.txt.
/// </summary>
internal record class StopTimeCsv(
    string trip_id,
    string arrival_time,
    string departure_time,
    string stop_id,
    string stop_sequence,
    string pickup_type,
    string drop_off_type);
