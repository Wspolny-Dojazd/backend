using Domain.Models;
using Newtonsoft.Json.Linq;
using PublicTransportService.Application.Interfaces;

namespace PublicTransportService.Infrastructure.ExternalApis;

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
        using var reader = new StreamReader(stream);
        var content = await reader.ReadToEndAsync();

        var json = JObject.Parse(content);

        var duration = (double)json["features"]![0]!["properties"]!["summary"]!["duration"]!;

        return (int)duration;
    }

    /// <inheritdoc/>
    public async Task<WalkingPathInfo> GetWalkingPathInfoAsync(double depLatitude, double depLongitude, double destLatitude, double destLongitude)
    {
        string url = $"https://api.openrouteservice.org/v2/directions/foot-walking?" +
                     $"api_key={apiKey}&start={depLongitude},{depLatitude}&end={destLongitude},{destLatitude}";

        var response = await httpClient.GetAsync(url);
        _ = response.EnsureSuccessStatusCode();

        using var stream = await response.Content.ReadAsStreamAsync();
        using var reader = new StreamReader(stream);
        var content = await reader.ReadToEndAsync();

        var json = JObject.Parse(content);

        var coordinatesToken = json["features"]![0]!["geometry"]!["coordinates"]!;
        var coordinates = coordinatesToken
            .Select(coord =>
            {
                var lon = (double)coord[0]!;
                var lat = (double)coord[1]!;
                return (lat, lon);
            })
            .ToList();

        var summaryToken = json["features"]![0]!["properties"]!["summary"]!;
        var duration = (int)MathF.Round(float.Parse(summaryToken["duration"]!.ToString()));
        var distance = (int)MathF.Round(float.Parse(summaryToken["distance"]!.ToString()));

        return new WalkingPathInfo(duration, distance, coordinates);
    }

    /// <inheritdoc/>
    public int GetWalkingTimeEstimate(double depLatitude, double depLongitude, double destLatitude, double destLongitude)
    {
        const double routeFactor = 1.3; // Adjusts for realistic paths
        double walkingSpeedKmh = 3.0;
        double distance = FlatDistance(depLatitude, depLongitude, destLatitude, destLongitude) * routeFactor;
        double timeSeconds = (distance / walkingSpeedKmh) * 60 * 60;

        return (int)Math.Ceiling(timeSeconds);
    }

    private static double FlatDistance(double lat1, double lon1, double lat2, double lon2)
    {
        const double R = 6371.0;
        double dLat = (lat2 - lat1) * Math.PI / 180.0;
        double dLon = (lon2 - lon1) * Math.PI / 180.0;
        double avgLat = (lat1 + lat2) / 2.0 * Math.PI / 180.0;

        double x = dLon * Math.Cos(avgLat);
        double y = dLat;

        return R * Math.Sqrt((x * x) + (y * y));
    }
}
