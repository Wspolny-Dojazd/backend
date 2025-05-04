using System.Diagnostics;
using System.IO.Compression;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using PublicTransportService.Application.Interfaces;
using PublicTransportService.Domain.Entities;
using PublicTransportService.Infrastructure.Data;
using PublicTransportService.Infrastructure.Importing;
using PublicTransportService.Infrastructure.Mapping;
using PublicTransportService.Infrastructure.Models.GtfsCsv;

namespace PublicTransportService.Infrastructure.Services;

/// <summary>
/// Represents a service for importing GTFS data into the database.
/// </summary>
/// <param name="context">The database context.</param>
/// <param name="importStrategy">The import strategy.</param>
/// <param name="logger">The logger.</param>
internal class GtfsImportService(
    PTSDbContext context,
    IGtfsImportStrategy importStrategy,
    ILogger<GtfsImportService> logger)
    : IGtfsImportService
{
    private const string GtfsUrl = "https://mkuran.pl/gtfs/warsaw.zip";

    /// <inheritdoc/>
    public async Task ImportAsync(string? localZipPath, int chunkSize)
    {
        var stopwatch = Stopwatch.StartNew();

        var originalAutoDetect = context.ChangeTracker.AutoDetectChangesEnabled;
        context.ChangeTracker.AutoDetectChangesEnabled = false;

        var originalTracking = context.ChangeTracker.QueryTrackingBehavior;
        context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

        var tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        var tempDir = Directory.CreateDirectory(tempPath);

        logger.LogDebug("Temporary directory created at {Path}", tempPath);

        IDbContextTransaction? transaction = null;
        if (importStrategy.RequiresTransaction)
        {
            transaction = await context.Database.BeginTransactionAsync();
        }

        try
        {
            var zipPath = Path.Combine(tempPath, "gtfs.zip");

            if (!string.IsNullOrWhiteSpace(localZipPath))
            {
                File.Copy(localZipPath, zipPath, overwrite: true);
                logger.LogInformation("Using local GTFS file at {Path}", localZipPath);
            }
            else
            {
                using var httpClient = new HttpClient();
                await this.DownloadGtfsWithProgressAsync(httpClient, zipPath);
            }

            ZipFile.ExtractToDirectory(zipPath, tempPath);
            logger.LogDebug("Extracted GTFS files to {Path}", tempPath);

            await importStrategy.ImportAsync<StopCsv, Stop>(
                Path.Combine(tempPath, "stops.txt"),
                GtfsCsvMapper.ParseStops,
                chunkSize);

            await importStrategy.ImportAsync<RouteCsv, Route>(
                Path.Combine(tempPath, "routes.txt"),
                GtfsCsvMapper.ParseRoutes,
                chunkSize);

            await importStrategy.ImportAsync<ShapeCsv, Shape>(
                Path.Combine(tempPath, "shapes.txt"),
                GtfsCsvMapper.ParseShapes,
                chunkSize);

            await importStrategy.ImportAsync<TripCsv, Trip>(
                Path.Combine(tempPath, "trips.txt"),
                GtfsCsvMapper.ParseTrips,
                chunkSize);

            await importStrategy.ImportAsync<StopTimeCsv, StopTime>(
                Path.Combine(tempPath, "stop_times.txt"),
                GtfsCsvMapper.ParseStopTimes,
                chunkSize);

            await this.UpdateGtfsMetadataAsync();

            if (transaction is not null)
            {
                await transaction.CommitAsync();
                transaction.Dispose();
            }

            logger.LogInformation("GTFS import completed successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "GTFS import failed: {Message}", ex.Message);
            throw;
        }
        finally
        {
            stopwatch.Stop();
            context.ChangeTracker.AutoDetectChangesEnabled = originalAutoDetect;
            context.ChangeTracker.QueryTrackingBehavior = originalTracking;

            try
            {
                Directory.Delete(tempPath, recursive: true);
            }
            catch
            {
                logger.LogError("Failed to delete temporary directory {Path}", tempPath);
            }

            logger.LogInformation("GTFS import took {Time:N2} seconds", stopwatch.Elapsed.TotalSeconds);
        }
    }

    private async Task DownloadGtfsWithProgressAsync(HttpClient httpClient, string destinationFilePath)
    {
        logger.LogInformation("Downloading GTFS from {Url}", GtfsUrl);
        var downloadStopwatch = Stopwatch.StartNew();

        using var response = await httpClient.SendAsync(
            new HttpRequestMessage(HttpMethod.Get, GtfsUrl),
            HttpCompletionOption.ResponseHeadersRead);

        _ = response.EnsureSuccessStatusCode();
        var contentLength = response.Content.Headers.ContentLength
            ?? throw new InvalidOperationException("Unknown content length");

        await using var contentStream = await response.Content.ReadAsStreamAsync();
        await using var fileStream = File.Create(destinationFilePath);
        var buffer = new byte[8192];
        long totalBytesRead = 0;
        var progressStopwatch = Stopwatch.StartNew();

        int bytesRead;
        while ((bytesRead = await contentStream.ReadAsync(buffer)) > 0)
        {
            await fileStream.WriteAsync(buffer.AsMemory(0, bytesRead));
            totalBytesRead += bytesRead;
            var downloadSpeed = totalBytesRead / progressStopwatch.Elapsed.TotalSeconds / 1_000_000;
            var progressPercentage = (int)(totalBytesRead * 100 / contentLength);

            var estimatedTimeRemaining = TimeSpan.FromSeconds(
                (contentLength - totalBytesRead) / (downloadSpeed * 1_000_000));

            Console.Write(
                $"\rDownloading GTFS... {progressPercentage}% - {downloadSpeed:N2} MB/s, " +
                $"ETA: {estimatedTimeRemaining:hh\\:mm\\:ss}".PadRight(80));
        }

        Console.Write("\r");

        progressStopwatch.Stop();
        downloadStopwatch.Stop();

        logger.LogInformation(
            "Downloaded GTFS in {Time:N2} seconds.",
            downloadStopwatch.Elapsed.TotalSeconds);
    }

    private async Task UpdateGtfsMetadataAsync()
    {
        var metadata = await context.GtfsMetadata.SingleOrDefaultAsync();
        if (metadata is null)
        {
            logger.LogInformation("GTFS metadata not found — creating new entry");
            metadata = new GtfsMetadata { LastUpdated = DateTime.UtcNow };
            _ = await context.GtfsMetadata.AddAsync(metadata);
        }
        else
        {
            logger.LogInformation("Updating LastUpdated timestamp in GTFS metadata");
            metadata.LastUpdated = DateTime.UtcNow;
            _ = context.GtfsMetadata.Update(metadata);
        }

        _ = await context.SaveChangesAsync();
        logger.LogInformation("GTFS metadata updated with timestamp: {Timestamp}", metadata.LastUpdated);
    }
}
