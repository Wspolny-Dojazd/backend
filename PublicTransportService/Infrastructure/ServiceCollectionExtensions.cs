using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using PublicTransportService.Application.Interfaces;
using PublicTransportService.Domain.Interfaces;
using PublicTransportService.Infrastructure.Data;
using PublicTransportService.Infrastructure.Data.Repositories;
using PublicTransportService.Infrastructure.Importing;
using PublicTransportService.Infrastructure.PathFinding.Raptor;
using PublicTransportService.Infrastructure.Services;

namespace PublicTransportService.Infrastructure;

/// <summary>
/// Extension methods for registering infrastructure services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers the public transport service infrastructure layer dependencies (e.g., DbContext, services).
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The modified service collection.</returns>
    public static IServiceCollection AddPTSInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("AppDbConnectionString");

        _ = services.AddDbContext<PTSDbContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                .UseSnakeCaseNamingConvention());

        _ = services.AddHttpClient();

        _ = services
            .AddScoped<IGtfsImportService, GtfsImportService>()
            .AddScoped<IPathPlanningService, PathPlanningService>()
            .AddScoped<IRouteRepository, RouteRepository>()
            .AddScoped<IShapeRepository, ShapeRepository>()
            .AddScoped<IStopRepository, StopRepository>()
            .AddScoped<ITripRepository, TripRepository>()
            .AddScoped<IGtfsImportStrategy>(provider =>
            {
                var builder = new MySqlConnectionStringBuilder(connectionString!);
                var context = provider.GetRequiredService<PTSDbContext>();

                if (builder.AllowLoadLocalInfile)
                {
                    var logger = provider.GetRequiredService<ILogger<BulkGtfsImportStrategy>>();
                    return new BulkGtfsImportStrategy(context, logger);
                }
                else
                {
                    var logger = provider.GetRequiredService<ILogger<EfCoreGtfsImportStrategy>>();

                    logger.LogWarning(
                        "Local file loading is disabled. " +
                        "Using EF insert instead of bulk insert. " +
                        "This may be slow. " +
                        "To enable it, set 'AllowLoadLocalInfile=true' in the connection string.");

                    return new EfCoreGtfsImportStrategy(context, logger);
                }
            })
            .AddScoped<IWalkingTimeEstimator>(provider =>
            {
                var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
                var client = httpClientFactory.CreateClient();

                string apiKey = configuration["OpenRouteService:ApiKey"]!;

                return new OpenRouteServiceWalkingTimeEstimator(client, apiKey);
            });

        _ = services.AddSingleton<IRaptorDataCache, RaptorDataCache>();

        _ = services.AddHostedService<RaptorDataCacheRefresher>();

        return services;
    }

    /// <summary>
    /// Initializes one-time infrastructure components, such as in-memory caches.
    /// Should be called once after application startup.
    /// </summary>
    /// <param name="services">The application's service provider.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public static async Task InitializePTSInfrastructureAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var cache = scope.ServiceProvider.GetRequiredService<IRaptorDataCache>();
        await cache.InitializeAsync();
    }
}
