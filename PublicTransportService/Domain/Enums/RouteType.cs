namespace PublicTransportService.Domain.Enums;

/// <summary>
/// Specifies the type of transportation used on a route, as defined by the GTFS specification.
/// </summary>
/// <remarks>
/// As of now, Warsaw supports only <see cref="Tram"/>,
/// <see cref="Metro"/>, <see cref="Rail"/>, and <see cref="Bus"/>.
/// </remarks>
public enum RouteType : byte
{
    /// <summary>
    /// Tram, streetcar, or light rail lines.
    /// </summary>
    Tram = 0,

    /// <summary>
    /// Subway, metro, or underground rail services.
    /// </summary>
    Metro = 1,

    /// <summary>
    /// Suburban or regional rail services.
    /// </summary>
    Rail = 2,

    /// <summary>
    /// City, suburban, or long-distance buses.
    /// </summary>
    Bus = 3,

    /// <summary>
    /// Ferry or water-based transport.
    /// </summary>
    Ferry = 4,

    /// <summary>
    /// Street-level cable cars.
    /// </summary>
    CableCar = 5,

    /// <summary>
    /// Suspended aerial transport (e.g., gondolas).
    /// </summary>
    AerialLift = 6,

    /// <summary>
    /// Funicular rail systems on inclined tracks.
    /// </summary>
    Funicular = 7,

    /// <summary>
    /// Electric buses powered by overhead wires.
    /// </summary>
    Trolleybus = 11,

    /// <summary>
    /// Monorail or single-rail train systems.
    /// </summary>
    Monorail = 12,
}
