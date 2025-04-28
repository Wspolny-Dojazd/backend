using Microsoft.EntityFrameworkCore;
using PublicTransportService.Domain.Entities;

namespace PublicTransportService.Infrastructure.Data;

/// <summary>
/// Represents the Entity Framework Core database context for the public transport system.
/// </summary>
/// <param name="options">The options to configure the database context.</param>
public class PTSDbContext(DbContextOptions<PTSDbContext> options)
    : DbContext(options)
{
    /// <summary>
    /// Gets or sets the <see cref="DbSet{TEntity}"/> representing GTFS metadata.
    /// </summary>
    public DbSet<GtfsMetadata> GtfsMetadata { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="DbSet{TEntity}"/> representing GTFS routes.
    /// </summary>
    public DbSet<Route> Routes { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="DbSet{TEntity}"/> representing GTFS shapes.
    /// </summary>
    public DbSet<Shape> Shapes { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="DbSet{TEntity}"/> representing GTFS stops.
    /// </summary>
    public DbSet<Stop> Stops { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="DbSet{TEntity}"/> representing GTFS stop times.
    /// </summary>
    public DbSet<StopTime> StopTimes { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="DbSet{TEntity}"/> representing GTFS trips.
    /// </summary>
    public DbSet<Trip> Trips { get; set; }

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        _ = modelBuilder.Entity<Route>()
            .ToTable("pts_routes");

        _ = modelBuilder.Entity<Shape>()
            .ToTable("pts_shapes")
            .HasKey(s => new { s.Id, s.PtSequence });

        _ = modelBuilder.Entity<StopTime>()
            .ToTable("pts_stop_times");

        _ = modelBuilder.Entity<Stop>()
            .ToTable("pts_stops");

        _ = modelBuilder.Entity<Trip>()
            .ToTable("pts_trips");
    }
}
