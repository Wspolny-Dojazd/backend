namespace PublicTransportService.Infrastructure.PathFinding.Raptor;

/// <summary>
/// Represents the best known state of a path at a given stop, used to track progress in the RAPTOR algorithm.
/// </summary>
/// <param name="DepartureTime">The departure time from the origin stop.</param>
/// <param name="Transfers">The number of transfers made to reach this stop.</param>
/// <param name="LastTripId">The ID of the most recent trip taken, or <see langword="null"/> if not applicable.</param>
internal record BestState(DateTime DepartureTime, int Transfers, string? LastTripId);
