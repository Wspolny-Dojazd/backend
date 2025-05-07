using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.UserLocation;

/// <summary>
/// Represents the data transfer object used for returning the user location in API responses.
/// </summary>
/// <param name="Latitude">The latitude of the user's location.</param>
/// <param name="Longitude">The longitude of the user's location.</param>
/// <param name="UpdatedAt">The timestamp of the last location update.</param>
public record UserLocationDto(
    [property: Required] double Latitude,
    [property: Required] double Longitude,
    [property: Required] DateTime UpdatedAt);
