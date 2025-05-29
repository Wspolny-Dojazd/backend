namespace Domain.Models;

/// <summary>
/// Represents walking path information.
/// </summary>
/// <param name="Duration">The walking time in seconds.</param>
/// <param name="Distance">The distance in meters.</param>
/// <param name="Coordinates">The list of coordinates representing the walking path.</param>
public record WalkingPathInfo(
    int Duration,
    int Distance,
    List<(double Latitude, double Longitude)> Coordinates);
