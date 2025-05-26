using System.Text.Json;
using PublicTransportService.Application.Interfaces;

/// <summary>
/// Estimates walking time between two geographical points using the OpenRouteService API.
/// </summary>
public class OpenRouteServiceWalkingTimeEstimator(HttpClient httpClient, string apiKey) : IWalkingTimeEstimator
{
    /// <inheritdoc/>
    public async Task<int> GetWalkingTimeAsync(double depLatitude, double depLongitude, double destLatitude, double destLongitude)
    {
        string url = $"https://api.openrouteservice.org/v2/directions/foot-walking?" +
                     $"api_key={apiKey}&start={depLongitude},{depLatitude}&end={destLongitude},{destLatitude}";

        var response = await httpClient.GetAsync(url);
        _ = response.EnsureSuccessStatusCode();

        using var stream = await response.Content.ReadAsStreamAsync();
        var json = await JsonDocument.ParseAsync(stream);

        var duration = json
            .RootElement
            .GetProperty("features")[0]
            .GetProperty("properties")
            .GetProperty("summary")
            .GetProperty("duration")
            .GetDouble();

        return (int)duration;
    }

    /// <inheritdoc/>
    public int GetWalkingTimeEstimate(double depLatitude, double depLongitude, double destLatitude, double destLongitude)
    {
        const double routeFactor = 1.3; // Adjusts for realistic paths
        double walkingSpeedKmh = 5.0; // Average walking speed
        double distance = HaversineDistance(depLatitude, depLongitude, destLatitude, destLongitude) * routeFactor;
        double timeSeconds = (distance / walkingSpeedKmh) * 60 * 60;

        return (int)Math.Round(timeSeconds);
    }

    private static double HaversineDistance(double lat1, double lon1, double lat2, double lon2)
    {
        const double R = 6371.0;

        double dLat = ToRadians(lat2 - lat1);
        double dLon = ToRadians(lon2 - lon1);

        double rLat1 = ToRadians(lat1);
        double rLat2 = ToRadians(lat2);

        double a = (Math.Sin(dLat / 2) * Math.Sin(dLat / 2)) +
                   (Math.Cos(rLat1) * Math.Cos(rLat2) *
                   Math.Sin(dLon / 2) * Math.Sin(dLon / 2));

        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        return R * c;
    }

    private static double ToRadians(double angle)
    {
        return angle * Math.PI / 180.0;
    }
}
