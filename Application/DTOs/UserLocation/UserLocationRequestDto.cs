using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.UserLocation;

/// <summary>
/// Represents the data transfer object used for user location information.
/// </summary>
public class UserLocationRequestDto
{
    /// <summary>
    /// Gets the latitude of the user's location.
    /// </summary>
    /// <value>Between -90 and 90 degrees.</value>
    [Required]
    [Range(-90, 90)]
    public double Latitude { get; init; }

    /// <summary>
    /// Gets the longitude of the user's location.
    /// </summary>
    /// <value>Between -180 and 180 degrees.</value>
    [Required]
    [Range(-180, 180)]
    public double Longitude { get; init; }
}
