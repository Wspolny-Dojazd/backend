using Domain.Enums;
using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

/// <summary>
/// Class <c>DatabaseContext</c> connects database tables with model objects.
/// </summary>
public class DatabaseContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DatabaseContext"/> class.
    /// </summary>
    /// <param name="options">Database context options, that are passed to the base constructor.</param>
    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Gets or sets Group DbSet.
    /// </summary>
    public DbSet<Group> Groups { get; set; }

    /// <summary>
    /// Gets or sets Location DbSet.
    /// </summary>
    public DbSet<Location> Locations { get; set; }

    /// <summary>
    /// Gets or sets Message DbSet.
    /// </summary>
    public DbSet<Message> Messages { get; set; }

    /// <summary>
    /// Gets or sets UserConfiguration DbSet.
    /// </summary>
    public DbSet<UserConfiguration> UserConfiguration { get; set; }

    /// <summary>
    /// Gets or sets User DbSet.
    /// </summary>
    public DbSet<User> Users { get; set; }

    /// <summary>
    /// Gets or sets Route DbSet.
    /// </summary>
    public DbSet<Route> Routes { get; set; }

    /// <summary>
    /// Connects model with database tables.
    /// </summary>
    /// <param name="modelBuilder">Constructs model for a context. </param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        _ = modelBuilder.Entity<User>(entity =>
        {
            _ = entity.ToTable("users");
            _ = entity.HasKey(p => p.Id).HasName("PK_User");

            _ = entity.Property(p => p.Id).HasColumnName("id").HasColumnType("int").ValueGeneratedOnAdd();
            _ = entity.Property(p => p.Nickname).HasColumnName("nickname");
            _ = entity.Property(p => p.Email).HasColumnName("email");
            _ = entity.Property(p => p.PasswordHash).HasColumnName("password_hash");
        });

        _ = modelBuilder.Entity<Group>(entity =>
        {
            _ = entity.ToTable("groups");
            _ = entity.HasKey(p => p.Id).HasName("PK_Group");

            _ = entity.Property(p => p.Id).HasColumnName("id").HasColumnType("int").ValueGeneratedOnAdd();
            _ = entity.Property(p => p.JoiningCode).HasColumnName("joining_code");
            _ = entity.Property(p => p.DestinationLat).HasColumnName("destination_lat").HasColumnType("int");
            _ = entity.Property(p => p.DestinationLon).HasColumnName("destination_lon").HasColumnType("int");

            _ = entity.Property(p => p.Status)
                .HasColumnName("status")
                .HasColumnType("ENUM('NOT_STARTED', 'STARTED')")
                .HasConversion(
                    v => v.ToString(),
                    v => (Status)Enum.Parse(typeof(Status), v));

            _ = entity.HasMany(g => g.Routes)
                .WithOne()
                .HasForeignKey("group_id");

            _ = entity.HasMany(g => g.LiveLocations)
                .WithOne()
                .HasForeignKey("group_id");
        });

        _ = modelBuilder.Entity<UserConfiguration>(entity =>
        {
            _ = entity.ToTable("user_configurations");
            _ = entity.HasKey(p => p.Id).HasName("PK_UserConfiguration");

            _ = entity.Property(p => p.Id)
                .HasColumnName("id")
                .HasColumnType("int").ValueGeneratedOnAdd();

            _ = entity.Property(p => p.UserId)
                .HasColumnName("user_id")
                .HasColumnType("int");

            _ = entity.Property(p => p.Language)
               .HasColumnName("language")
               .HasColumnType("ENUM('English', 'Polish')")
               .HasConversion(
                   v => v.ToString(),
                   v => (Language)Enum.Parse(typeof(Language), v));

            _ = entity.Property(p => p.TimeSystem)
                .HasColumnName("time_system")
                .HasColumnType("ENUM('AMPM', 'TwentyFourHour')")
                .HasConversion(
                    v => v.ToString(),
                    v => (TimeSystem)Enum.Parse(typeof(TimeSystem), v));

            _ = entity.Property(p => p.DistanceUnit)
                .HasColumnName("distance_unit")
                .HasColumnType("ENUM('Kilometers', 'Miles')")
                .HasConversion(
                    v => v.ToString(),
                    v => (DistanceUnit)Enum.Parse(typeof(DistanceUnit), v));
        });

        _ = modelBuilder.Entity<Route>(entity =>
        {
            _ = entity.ToTable("routes");
            _ = entity.HasKey(p => p.Id).HasName("PK_Route");

            _ = entity.Property(p => p.Id).HasColumnName("id").HasColumnType("int").ValueGeneratedOnAdd();
            _ = entity.Property(p => p.Tip).HasColumnName("tip");
            _ = entity.Property(p => p.Lat).HasColumnName("lat").HasColumnType("int");
            _ = entity.Property(p => p.Lon).HasColumnName("lon").HasColumnType("int");
        });

        _ = modelBuilder.Entity<Location>(entity =>
        {
            _ = entity.ToTable("locations");
            _ = entity.HasKey(p => p.Id).HasName("PK_Location");

            _ = entity.Property(p => p.Id).HasColumnName("id").HasColumnType("int").ValueGeneratedOnAdd();
            _ = entity.Property(p => p.UserId).HasColumnName("user_id").HasColumnType("int");
            _ = entity.Property(p => p.Lat).HasColumnName("lat").HasColumnType("int");
            _ = entity.Property(p => p.Lon).HasColumnName("lon").HasColumnType("int");

            _ = entity.HasOne(l => l.User)
                .WithMany()
                .HasForeignKey(l => l.UserId);
        });

        _ = modelBuilder.Entity<Message>(entity =>
        {
            _ = entity.ToTable("messages");
            _ = entity.HasKey(p => p.Id).HasName("PK_Message");

            _ = entity.Property(p => p.Id).HasColumnName("id").HasColumnType("int").ValueGeneratedOnAdd();
            _ = entity.Property(p => p.GroupId).HasColumnName("group_id").HasColumnType("int");
            _ = entity.Property(p => p.UserId).HasColumnName("user_id").HasColumnType("int");
            _ = entity.Property(p => p.Content).HasColumnName("content");

            _ = entity.HasOne(m => m.Group)
                .WithMany()
                .HasForeignKey(m => m.GroupId);

            _ = entity.HasOne(m => m.User)
                .WithMany()
                .HasForeignKey(m => m.UserId);
        });

        _ = modelBuilder.Entity<User>()
            .HasMany(u => u.Friends)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "friends",
                j => j.HasOne<User>().WithMany().HasForeignKey("friend_id"),
                j => j.HasOne<User>().WithMany().HasForeignKey("user_id"));

        _ = modelBuilder.Entity<User>()
            .HasMany(u => u.Groups)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "group_members",
                j => j.HasOne<Group>().WithMany().HasForeignKey("group_id"),
                j => j.HasOne<User>().WithMany().HasForeignKey("user_id"));

        _ = modelBuilder.Entity<User>()
            .HasOne(u => u.UserConfiguration)
            .WithOne()
            .HasForeignKey<UserConfiguration>(uc => uc.UserId);
    }
}
