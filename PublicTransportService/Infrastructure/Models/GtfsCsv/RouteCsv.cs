namespace PublicTransportService.Infrastructure.Models.GtfsCsv;

#pragma warning disable IDE1006, SA1300

/// <summary>
/// Maps GTFS route data from routes.txt.
/// </summary>
internal record class RouteCsv(
    string route_id,
    string agency_id,
    string route_short_name,
    string route_long_name,
    string route_type,
    string route_color,
    string route_text_color);
