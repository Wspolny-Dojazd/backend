namespace PublicTransportService.Infrastructure.Models.GtfsCsv;

#pragma warning disable IDE1006, SA1300

/// <summary>
/// Maps GTFS stop data from stops.txt.
/// </summary>
internal record class StopCsv(
    string stop_id,
    string stop_name,
    string? stop_code,
    string? platform_code,
    string stop_lat,
    string stop_lon,
    string location_type,
    string? parent_station,
    string wheelchair_boarding,
    string? stop_name_stem,
    string? town_name,
    string? street_name);
