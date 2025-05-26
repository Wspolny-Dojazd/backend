namespace PublicTransportService.Application.Interfaces;

/// <summary>
/// Defines a contract for estimating walking time between two geographical points.
/// </summary>
public interface IWalkingTimeEstimator
{
    /// <summary>
    /// Calculates the walking time in seconds between a departure point and
    /// a destination point based on their geographic coordinates.
    /// </summary>
    /// <param name="depLatitude">The latitude of the departure point.</param>
    /// <param name="depLongitude">The longitude of the departure point.</param>
    /// <param name="destLatitude">The latitude of the destination point.</param>
    /// <param name="destLongitude">The longitude of the destination point.</param>
    /// <returns>
    /// The walking time between departure and destination point in seconds.
    /// </returns>
    Task<int> GetWalkingTimeAsync(double depLatitude, double depLongitude, double destLatitude, double destLongitude);

    /// <summary>
    /// Estimates the walking time in seconds between a departure point and
    /// a destination point based on their geographic coordinates.
    /// </summary>
    /// <param name="depLatitude">The latitude of the departure point.</param>
    /// <param name="depLongitude">The longitude of the departure point.</param>
    /// <param name="destLatitude">The latitude of the destination point.</param>
    /// <param name="destLongitude">The longitude of the destination point.</param>
    /// <returns>
    /// The walking time between departure and destination point in seconds.
    /// </returns>
    int GetWalkingTimeEstimate(double depLatitude, double depLongitude, double destLatitude, double destLongitude);
}
