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
        var basePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "../API"));

        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: true)
            .Build();

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
