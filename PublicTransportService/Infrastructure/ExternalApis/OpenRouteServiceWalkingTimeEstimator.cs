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
}
