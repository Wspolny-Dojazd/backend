namespace PublicTransportService.Infrastructure.Models.GtfsCsv;

#pragma warning disable IDE1006, SA1300

/// <summary>
/// Maps GTFS frequency data from frequencies.txt.
/// </summary>
internal record class FrequencyCsv(
    string trip_id,
    string start_time,
    string end_time,
    string headway_secs,
    string exact_times);
