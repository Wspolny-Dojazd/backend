using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PublicTransportService.Application.Interfaces;
using PublicTransportService.Infrastructure.Data;
using PublicTransportService.Infrastructure.Services;

namespace PublicTransportService.Infrastructure;

/// <summary>
/// Extension methods for registering infrastructure services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers the infrastructure layer dependencies (e.g., DbContext, services).
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The modified service collection.</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("AppDbConnectionString");

        _ = services.AddDbContext<PTSDbContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
            .UseSnakeCaseNamingConvention());

        _ = services.AddScoped<IGtfsImportService, GtfsImportService>();

        return services;
    }
}
