using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PublicTransportService.Infrastructure.Data;

namespace PublicTransportService.Infrastructure.PathFinding.Raptor;

/// <summary>
/// Represents a background service for refreshing the RAPTOR data cache.
/// </summary>
/// <param name="serviceProvider">The service provider.</param>
/// <param name="logger">The logger.</param>
internal class RaptorDataCacheRefresher(
    IServiceProvider serviceProvider,
    ILogger<RaptorDataCacheRefresher> logger)
    : BackgroundService
{
    private const int IntervalMinutes = 1;
    private DateTime? lastUpdateTimestamp;

    /// <inheritdoc/>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<PTSDbContext>();
                var currentTimestamp = await dbContext.GtfsMetadata
                    .Select(m => m.LastUpdated)
                    .OrderBy((_) => _) // avoids warning
                    .FirstAsync(stoppingToken);

                if (currentTimestamp != this.lastUpdateTimestamp)
                {
                    var cache = scope.ServiceProvider.GetRequiredService<IRaptorDataCache>();
                    await cache.InitializeAsync();
                    this.lastUpdateTimestamp = currentTimestamp;

                    logger.LogInformation("RAPTOR cache reloaded at {Time}", DateTime.Now);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to refresh RAPTOR cache");
            }

            await Task.Delay(TimeSpan.FromMinutes(IntervalMinutes), stoppingToken);
        }
    }
}
