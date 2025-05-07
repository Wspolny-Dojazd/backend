using System.ComponentModel.DataAnnotations;
using Application.DTOs.UserLocation;

namespace Application.DTOs.Path;

/// <summary>
/// Represents a data transfer object used for path requests.
/// </summary>
public class PathRequestDto
{
    /// <summary>
    /// Gets the latitude of the shared destination.
    /// </summary>
    [Required]
    public double DestinationLatitude { get; init; }

    /// <summary>
    /// Gets the longitude of the shared destination.
    /// </summary>
    [Required]
    public double DestinationLongitude { get; init; }

    /// <summary>
    /// Gets the desired arrival time at the destination.
    /// </summary>
    [Required]
    public required DateTime ArrivalTime { get; init; }

    /// <summary>
    /// Gets the list of locations of group members.
    /// </summary>
    [Required]
    public required List<MemberLocationRequestDto> UserLocations { get; init; }
}
