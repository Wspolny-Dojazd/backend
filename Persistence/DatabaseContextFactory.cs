using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Persistence;

/// <summary>
/// Provides a design-time factory for creating <see cref="DatabaseContext"/> instances during EF Core operations.
/// </summary>
public class DatabaseContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
{
    /// <inheritdoc/>
    public DatabaseContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("AppDbConnectionString")
            ?? throw new InvalidOperationException("Missing connection string 'AppDbConnectionString'.");

        var dbServerVersionRaw = Environment.GetEnvironmentVariable("DB_SERVER_VERSION");

        MySqlServerVersion serverVersion;
        if (!string.IsNullOrWhiteSpace(dbServerVersionRaw))
        {
            serverVersion = (MySqlServerVersion)ServerVersion.Parse(dbServerVersionRaw);
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[WARN] Environment variable 'DB_SERVER_VERSION' not found. Falling back to ServerVersion.AutoDetect().");
            Console.ResetColor();

            serverVersion = (MySqlServerVersion)ServerVersion.AutoDetect(connectionString);
        }

        var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>()
            .UseMySql(
                connectionString,
                serverVersion,
                mySqlOptions => mySqlOptions.MigrationsHistoryTable("__EFMigrationsHistory"))
            .UseSnakeCaseNamingConvention();

        return new DatabaseContext(optionsBuilder.Options);
    }
}
