using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PublicTransportService.Application.Interfaces;
using PublicTransportService.Infrastructure;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(config =>
    {
        var configPath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        Console.WriteLine($"Loading config from: {configPath}");
        _ = config.AddJsonFile(configPath, optional: true)
            .AddEnvironmentVariables();
    })
    .ConfigureServices((context, services) =>
    {
        _ = services
            .AddLogging(logging =>
            {
                _ = logging
                    .ClearProviders()
                    .AddConsole(options => options.LogToStandardErrorThreshold = LogLevel.Warning)
                    .SetMinimumLevel(LogLevel.Warning)
                    .AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);
            })
            .AddInfrastructure(context.Configuration);
        })
    .Build();

using var scope = host.Services.CreateScope();

var chunkSizeArg = args.ElementAtOrDefault(0);
var chunkSize = int.TryParse(chunkSizeArg, out var parsedChunkSize) ? parsedChunkSize : 10_000;

var localZip = args.ElementAtOrDefault(1);
localZip = string.IsNullOrWhiteSpace(localZip) ? null : localZip;

Console.WriteLine($"Starting GTFS import...");
Console.WriteLine($"Chunk size: {chunkSize}");
if (localZip is not null)
{
    Console.WriteLine($"Using local zip: {localZip}");
}

var service = scope.ServiceProvider.GetRequiredService<IGtfsImportService>();
await service.ImportAsync(localZip, chunkSize);

Console.WriteLine("GTFS import finished.");
