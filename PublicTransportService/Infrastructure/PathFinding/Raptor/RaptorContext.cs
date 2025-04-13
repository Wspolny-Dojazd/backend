namespace PublicTransportService.Infrastructure.PathFinding.Raptor;

/// <summary>
/// Represents the context for the Raptor algorithm.
/// </summary>
/// <param name="StopTimesByTrip">
/// A mapping of trip IDs to ordered lists of stop times used for route computation.
/// </param>
/// <param name="Stops">
/// A dictionary of stops used in the network, indexed by stop ID.
/// </param>
internal record RaptorContext(
    Dictionary<string, List<PathFindingStopTime>> StopTimesByTrip,
    Dictionary<string, PathFindingStop> Stops);
