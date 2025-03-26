namespace PublicTransportService.Infrastructure.Services.GtfsImport.CsvModels;

#pragma warning disable IDE1006, SA1300

/// <summary>
/// Maps GTFS shape data from shapes.txt.
/// </summary>
internal record class ShapeCsv(
    string shape_id,
    string shape_pt_lat,
    string shape_pt_lon,
    string shape_pt_sequence);
