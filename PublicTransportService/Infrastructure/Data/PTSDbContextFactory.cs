using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace PublicTransportService.Infrastructure.Data;

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
        var connectionString = configuration.GetConnectionString("AppDbConnectionString");

        var optionsBuilder = new DbContextOptionsBuilder<PTSDbContext>()
            .UseMySql(
                connectionString,
                ServerVersion.AutoDetect(connectionString),
                mySqlOptions => mySqlOptions.MigrationsHistoryTable("__EFMigrationsHistory_PTS"))
            .UseSnakeCaseNamingConvention();

        return new PTSDbContext(optionsBuilder.Options);
    }
}
