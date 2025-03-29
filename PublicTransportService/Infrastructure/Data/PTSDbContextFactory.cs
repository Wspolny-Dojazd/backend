using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using PublicTransportService.Infrastructure.Data;

/// <summary>
/// Represents a database context factory for the public transport system.
/// </summary>
public class PTSDbContextFactory : IDesignTimeDbContextFactory<PTSDbContext>
{
    /// <inheritdoc />
    public PTSDbContext CreateDbContext(string[] args)
    {
        var apiFolder = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "../API"));
        var builder = new ConfigurationBuilder();

        if (Directory.Exists(apiFolder))
        {
            _ = builder.SetBasePath(apiFolder)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);
        }

        var configuration = builder.AddEnvironmentVariables().Build();
        var connectionString = configuration.GetConnectionString("AppDbConnectionString")
            ?? throw new InvalidOperationException("Missing connection string 'AppDbConnectionString'.");

        var dbServerVersionRaw = Environment.GetEnvironmentVariable("DB_SERVER_VERSION");

        ServerVersion serverVersion;
        if (!string.IsNullOrWhiteSpace(dbServerVersionRaw))
        {
            serverVersion = ServerVersion.Parse(dbServerVersionRaw);
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[WARN] Environment variable 'DB_SERVER_VERSION' not found. Falling back to ServerVersion.AutoDetect().");
            Console.ResetColor();

            serverVersion = ServerVersion.AutoDetect(connectionString);
        }

        var optionsBuilder = new DbContextOptionsBuilder<PTSDbContext>()
            .UseMySql(
                connectionString,
                serverVersion,
                mySqlOptions => mySqlOptions.MigrationsHistoryTable("__EFMigrationsHistory_PTS"))
            .UseSnakeCaseNamingConvention();

        return new PTSDbContext(optionsBuilder.Options);
    }
}