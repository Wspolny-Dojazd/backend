using PublicTransportService.Domain.Enums;

namespace PublicTransportService.Domain.Entities;

/// <summary>
/// Represents a route in the public transport system.
/// </summary>
public class Route
{
    /// <summary>
    /// Gets the unique identifier of the route.
    /// </summary>
    public required string Id { get; init; }

    /// <summary>
    /// Gets the identifier of the agency that operates the route.
    /// </summary>
    public required string AgencyId { get; init; }

    /// <summary>
    /// Gets the short name of the route (e.g. line number).
    /// </summary>
    public required string ShortName { get; init; }

    /// <summary>
    /// Gets the descriptive name of the route.
    /// </summary>
    public required string LongName { get; init; }

    /// <summary>
    /// Gets the transport type of the route.
    /// </summary>
    public RouteType Type { get; init; }

    /// <summary>
    /// Gets the color to be used on the route's background.
    /// </summary>
    /// <value>A hexadecimal color in the format RRGGBB.</value>
    public required string Color { get; init; }

    /// <summary>
    /// Gets the text color to be used on the route's background.
    /// </summary>
    /// <value>A hexadecimal color in the format RRGGBB.</value>
    public required string TextColor { get; init; }
}
