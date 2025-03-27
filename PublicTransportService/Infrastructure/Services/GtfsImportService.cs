using System.Diagnostics;
using System.Globalization;
using System.IO.Compression;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PublicTransportService.Application.Interfaces;
using PublicTransportService.Domain.Entities;
using PublicTransportService.Domain.Enums;
using PublicTransportService.Infrastructure.Data;
using PublicTransportService.Infrastructure.Services.GtfsImport.CsvModels;

namespace PublicTransportService.Infrastructure.Services;

/// <summary>
/// Represents a service for importing GTFS data into the database.
/// </summary>
/// <param name="context">The database context.</param>
/// <param name="logger">The logger.</param>
internal class GtfsImportService(PTSDbContext context, ILogger<GtfsImportService> logger)
    : IGtfsImportService
{
    private const string GtfsUrl = "https://mkuran.pl/gtfs/warsaw.zip";

    /// <inheritdoc/>
    public async Task ImportAsync(string? localZipPath = null, int chunkSize = 10000)
    {
        var stopwatch = Stopwatch.StartNew();

        var originalAutoDetect = context.ChangeTracker.AutoDetectChangesEnabled;
        context.ChangeTracker.AutoDetectChangesEnabled = false;

        var originalTracking = context.ChangeTracker.QueryTrackingBehavior;
        context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

        var tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        var tempDir = Directory.CreateDirectory(tempPath);

        logger.LogInformation("Starting GTFS import from {Url}", GtfsUrl);
        logger.LogDebug("Temporary directory created at {Path}", tempPath);

        await using var transaction = await context.Database.BeginTransactionAsync();

        try
        {
            var zipPath = Path.Combine(tempPath, "gtfs.zip");

            if (!string.IsNullOrWhiteSpace(localZipPath))
            {
                if (!File.Exists(localZipPath))
                {
                    throw new FileNotFoundException("Local GTFS file not found.", localZipPath);
                }

                File.Copy(localZipPath, zipPath, overwrite: true);
                logger.LogInformation("Using local GTFS file at {Path}", localZipPath);
            }
            else
            {
                var downloadStopwatch = Stopwatch.StartNew();

                using var httpClient = new HttpClient();
                await this.DownloadGtfsWithProgressAsync(httpClient, zipPath);

                var downloadTime = downloadStopwatch.Elapsed.TotalSeconds;
                logger.LogInformation(
                    "Downloaded GTFS zip from {Url} in {Time:N2} seconds",
                    GtfsUrl,
                    downloadTime);
            }

            ZipFile.ExtractToDirectory(zipPath, tempPath);
            logger.LogDebug("Extracted GTFS files to {Path}", tempPath);

            logger.LogInformation("Clearing existing GTFS data...");
            _ = await context.StopTimes.ExecuteDeleteAsync();
            _ = await context.Stops.ExecuteDeleteAsync();
            _ = await context.Shapes.ExecuteDeleteAsync();
            _ = await context.Routes.ExecuteDeleteAsync();
            logger.LogInformation("Existing GTFS data cleared.");

            await this.ImportRoutesAsync(Path.Combine(tempPath, "routes.txt"), chunkSize);
            await this.ImportShapesAsync(Path.Combine(tempPath, "shapes.txt"), chunkSize);
            await this.ImportStopsAsync(Path.Combine(tempPath, "stops.txt"), chunkSize);
            await this.ImportStopTimesAsync(Path.Combine(tempPath, "stop_times.txt"), chunkSize);

            _ = await context.SaveChangesAsync();
            await transaction.CommitAsync();

            logger.LogInformation("GTFS import completed successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "GTFS import failed: {Message}", ex.Message);
            await transaction.RollbackAsync();
            throw;
        }
        finally
        {
            stopwatch.Stop();
            logger.LogInformation("GTFS import took {Time} seconds", stopwatch.Elapsed.TotalSeconds);

            try
            {
                tempDir.Delete(true);
                logger.LogDebug("Deleted temporary directory at {Path}", tempPath);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Could not delete temp folder: {Message}", ex.Message);
            }

            context.ChangeTracker.AutoDetectChangesEnabled = originalAutoDetect;
            context.ChangeTracker.QueryTrackingBehavior = originalTracking;

            logger.LogInformation("Restored original change tracking settings");
        }
    }

    private static DateTime TimeSpanToDateTime(string time)
    {
        // GTFS allows extended hour format like 25:10:00, which represents 1:10 AM on the next day.
        // This method parses such values safely and returns a proper DateTime.
        var parts = time.Split(':');

        if (parts.Length != 3
            || !int.TryParse(parts[0], out var hours)
            || !int.TryParse(parts[1], out var minutes)
            || !int.TryParse(parts[2], out var seconds))
        {
            throw new FormatException($"Invalid GTFS time format: '{time}'");
        }

        var daysToAdd = hours / 24;
        var normalizedHours = hours % 24;

        var timeSpan = new TimeSpan(normalizedHours, minutes, seconds);
        return DateTime.Today.AddDays(daysToAdd).Add(timeSpan);
    }

    private async Task ImportRoutesAsync(string path, int chunkSize)
    {
        await this.ImportInBatchesAsync<RouteCsv, Route>(
            path,
            record => new Route
            {
                Id = record.route_id,
                AgencyId = record.agency_id,
                ShortName = record.route_short_name,
                LongName = record.route_long_name,
                Type = (RouteType)int.Parse(record.route_type),
                Color = record.route_color,
                TextColor = record.route_text_color,
            },
            context.Routes,
            "routes",
            chunkSize);
    }

    private async Task ImportShapesAsync(string path, int chunkSize)
    {
        await this.ImportInBatchesAsync<ShapeCsv, Shape>(
            path,
            record => new Shape
            {
                Id = record.shape_id,
                PtSequence = int.Parse(record.shape_pt_sequence),
                PtLatitude = double.Parse(record.shape_pt_lat, CultureInfo.InvariantCulture),
                PtLongitude = double.Parse(record.shape_pt_lon, CultureInfo.InvariantCulture),
            },
            context.Shapes,
            "shapes",
            chunkSize);
    }

    private async Task ImportStopsAsync(string path, int chunkSize)
    {
        await this.ImportInBatchesAsync<StopCsv, Stop>(
            path,
            record => new Stop
            {
                Id = record.stop_id,
                Name = record.stop_name,
                Code = record.stop_code,
                PlatformCode = record.platform_code,
                Latitude = double.Parse(record.stop_lat, CultureInfo.InvariantCulture),
                Longitude = double.Parse(record.stop_lon, CultureInfo.InvariantCulture),
                LocationType = (LocationType)int.Parse(record.location_type),
                ParentStationId = record.parent_station,
                WheelchairBoarding = record.wheelchair_boarding == "1",
                NameStem = record.stop_name_stem,
                TownName = record.town_name,
                StreetName = record.street_name,
            },
            context.Stops,
            "stops",
            chunkSize);
    }

    private async Task ImportStopTimesAsync(string path, int chunkSize)
    {
        var currentId = 1;
        await this.ImportInBatchesAsync<StopTimeCsv, StopTime>(
            path,
            record => new StopTime
            {
                Id = currentId++,
                TripId = record.trip_id,
                StopId = record.stop_id,
                StopSequence = int.Parse(record.stop_sequence),
                ArrivalTime = TimeSpanToDateTime(record.arrival_time),
                DepartureTime = TimeSpanToDateTime(record.departure_time),
                PickupType = (PickupType)int.Parse(record.pickup_type),
                DropOffType = (DropOffType)int.Parse(record.drop_off_type),
            },
            context.StopTimes,
            "stop times",
            chunkSize);
    }

    private async Task ImportInBatchesAsync<TCsv, TEntity>(
        string path,
        Func<TCsv, TEntity> map,
        DbSet<TEntity> dbSet,
        string entityLabel,
        int batchSize)
        where TCsv : class
        where TEntity : class
    {
        logger.LogInformation("Importing {Entity} from {Path}", entityLabel, path);

        using var reader = new StreamReader(path);
        using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HeaderValidated = null,
            MissingFieldFound = null,
        });

        var batch = new List<TEntity>(batchSize);
        var count = 0;

        var allRecords = new List<TEntity>();
        await foreach (var record in csv.GetRecordsAsync<TCsv>())
        {
            allRecords.Add(map(record));
        }

        var totalChunks = (int)Math.Ceiling(allRecords.Count / (double)batchSize);

        logger.LogInformation(
            "Total records of {Entity}: {Count}. Processing in {Chunks} chunks of {BatchSize}",
            entityLabel,
            allRecords.Count,
            totalChunks,
            batchSize);

        var globalStopwatch = Stopwatch.StartNew();
        var chunkTimes = new List<TimeSpan>();

        for (int i = 0; i < totalChunks; i++)
        {
            var chunkStopwatch = Stopwatch.StartNew();
            var chunk = allRecords.Skip(i * batchSize).Take(batchSize);

            const int RecentChunkPercentage = 10;
            var recentCount = (int)Math.Ceiling(totalChunks * RecentChunkPercentage / 100.0);
            var recentChunkTimes = chunkTimes.Count <= recentCount
                ? chunkTimes
                : chunkTimes.TakeLast(recentCount);

            var averageChunkTime = recentChunkTimes.Any()
                ? TimeSpan.FromMilliseconds(recentChunkTimes.Average(t => t.TotalMilliseconds))
                : TimeSpan.Zero;

            var percent = (i + 1) * 100.0 / totalChunks;
            var remainingChunks = totalChunks - (i + 1);
            var eta = TimeSpan.FromMilliseconds(averageChunkTime.TotalMilliseconds * remainingChunks);

            var message = $"\rSaving chunk {i + 1}/{totalChunks} ({percent:N1}%)... ETA: {eta:hh\\:mm\\:ss}";
            Console.Write(message.PadRight(100));

            await dbSet.AddRangeAsync(chunk);
            _ = await context.SaveChangesAsync();
            context.ChangeTracker.Clear();

            chunkStopwatch.Stop();
            chunkTimes.Add(chunkStopwatch.Elapsed);

            logger.LogDebug("Chunk {Index}/{Total} saved.", i + 1, totalChunks);
            count += chunk.Count();
        }

        Console.Write("\r");
        logger.LogInformation(
            "Imported {Count} {Entity} in {Time:N2} seconds",
            count,
            entityLabel,
            globalStopwatch.Elapsed.TotalSeconds);
    }

    private async Task DownloadGtfsWithProgressAsync(HttpClient httpClient, string destinationPath)
    {
        using var response = await httpClient.SendAsync(
            new HttpRequestMessage(HttpMethod.Get, GtfsUrl),
            HttpCompletionOption.ResponseHeadersRead);

        _ = response.EnsureSuccessStatusCode();

        var contentLength = response.Content.Headers.ContentLength;

        if (contentLength is null)
        {
            logger.LogWarning("Cannot determine file size. Downloading without progress...");
            await using var stream = await response.Content.ReadAsStreamAsync();
            await using var fileStream = File.Create(destinationPath);
            await stream.CopyToAsync(fileStream);
            return;
        }

        const int DownloadBufferSize = 8_192;
        var buffer = new byte[DownloadBufferSize];

        const double Megabyte = 1_000_000.0;
        var totalBytes = contentLength.Value;
        long totalRead = 0;
        int read;

        using var contentStream = await response.Content.ReadAsStreamAsync();
        await using var fileContentStream = File.Create(destinationPath);

        var lastProgress = -1;
        var stopwatch = Stopwatch.StartNew();

        while ((read = await contentStream.ReadAsync(buffer)) > 0)
        {
            await fileContentStream.WriteAsync(buffer.AsMemory(0, read));
            totalRead += read;

            var progress = (int)(totalRead * 100 / totalBytes);
            if (progress != lastProgress)
            {
                var elapsedSeconds = stopwatch.Elapsed.TotalSeconds;
                var speed = elapsedSeconds > 0 ? totalRead / elapsedSeconds : 0;
                var speedMBs = speed / Megabyte;
                var remainingBytes = totalBytes - totalRead;
                var etaSeconds = speed > 0 ? remainingBytes / speed : 0;

                var message =
                    $"\rDownloading GTFS... {totalRead / Megabyte:N1} MB / {totalBytes / Megabyte:N1} " +
                    $"MB ({progress}%) - {speedMBs:N2} MB/s, ETA: {TimeSpan.FromSeconds(etaSeconds):hh\\:mm\\:ss}";

                Console.Write(message.PadRight(100));

                lastProgress = progress;
            }
        }

        Console.Write("\r");
    }
}
