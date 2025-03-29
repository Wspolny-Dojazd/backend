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
    /// Gets or sets the <see cref="DbSet{TEntity}"/> representing GTFS routes.
    /// </summary>
    internal DbSet<Route> Routes { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="DbSet{TEntity}"/> representing GTFS shapes.
    /// </summary>
    internal DbSet<Shape> Shapes { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="DbSet{TEntity}"/> representing GTFS stops.
    /// </summary>
    internal DbSet<Stop> Stops { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="DbSet{TEntity}"/> representing GTFS stop times.
    /// </summary>
    internal DbSet<StopTime> StopTimes { get; set; }

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
    }
}
