using Domain.Enums;
using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

/// <summary>
/// Represents the Entity Framework Core database context used to interact
/// with the application's relational database. Configures entity mappings
/// and database schema for the domain models.
/// </summary>
/// <param name="options">The options to configure the database context.</param>
public class DatabaseContext(DbContextOptions<DatabaseContext> options)
    : DbContext(options)
{
    /// <summary>
    /// Gets or sets the <see cref="DbSet{TEntity}"/> representing users.
    /// </summary>
    public DbSet<User> Users { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="DbSet{TEntity}"/> representing groups.
    /// </summary>
    public DbSet<Group> Groups { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="DbSet{TEntity}"/> representing user configurations.
    /// </summary>
    public DbSet<UserConfiguration> UserConfigurations { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="DbSet{TEntity}"/> representing messages.
    /// </summary>
    public DbSet<Message> Messages { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="DbSet{TEntity}"/> representing user locations.
    /// </summary>
    public DbSet<UserLocation> UserLocations { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="DbSet{TEntity}"/> representing accepted group paths.
    /// </summary>
    public DbSet<GroupPath> GroupPaths { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="DbSet{TEntity}"/> representing proposed paths.
    /// </summary>
    public DbSet<ProposedPath> ProposedPaths { get; set; }

    /// <summary>
    /// Gets or sets the database set for friend invitations.
    /// </summary>
    public DbSet<FriendInvitation> FriendInvitations { get; set; }

    /// <summary>
    /// Configures the entity model and relationships for the database schema.
    /// </summary>
    /// <param name="modelBuilder">The builder used to construct the model for this context.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        _ = modelBuilder.Entity<User>(entity =>
        {
            _ = entity.ToTable("users");
            _ = entity.HasKey(p => p.Id).HasName("PK_User");

            _ = entity.Property(p => p.Id).HasColumnName("id").HasColumnType("char(36)");
            _ = entity.Property(p => p.Username).HasColumnName("username").HasMaxLength(32);
            _ = entity.Property(p => p.Nickname).HasColumnName("nickname");
            _ = entity.Property(p => p.Email).HasColumnName("email");
            _ = entity.Property(p => p.PasswordHash).HasColumnName("password_hash");
            _ = entity.Property(p => p.RefreshToken).HasColumnName("refresh_token");
            _ = entity.Property(p => p.RefreshTokenExpiryTime).HasColumnName("refresh_token_expiry_time").HasColumnType("datetime");

            _ = entity.Property(p => p.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("datetime")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        _ = modelBuilder.Entity<Group>(entity =>
        {
            _ = entity.ToTable("groups");
            _ = entity.HasKey(p => p.Id).HasName("PK_Group");

            _ = entity.Property(p => p.Id).HasColumnName("id").HasColumnType("int").ValueGeneratedOnAdd();
            _ = entity.Property(p => p.JoiningCode).HasColumnName("joining_code");
            _ = entity.Property(p => p.CreatorId).HasColumnName("creator_id").HasColumnType("char(36)");

            _ = entity.Property(p => p.Status)
                .HasColumnName("status")
                .HasColumnType("ENUM('NotStarted', 'Started')")
                .HasConversion(
                    v => v.ToString(),
                    v => (Status)Enum.Parse(typeof(Status), v));

            _ = entity.HasOne(c => c.Creator)
                .WithMany()
                .HasForeignKey(g => g.CreatorId);
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
                .HasColumnType("char(36)");

            _ = entity.Property(p => p.Language)
               .HasColumnName("language")
               .HasColumnType("ENUM('English', 'Polish')")
               .HasConversion(
                   v => v.ToString(),
                   v => (Language)Enum.Parse(typeof(Language), v));

            _ = entity.Property(p => p.TimeSystem)
                .HasColumnName("time_system")
                .HasColumnType("ENUM('TwelveHour', 'TwentyFourHour')")
                .HasConversion(
                    v => v.ToString(),
                    v => (TimeSystem)Enum.Parse(typeof(TimeSystem), v));

            _ = entity.Property(p => p.DistanceUnit)
                .HasColumnName("distance_unit")
                .HasColumnType("ENUM('Kilometers', 'Miles')")
                .HasConversion(
                    v => v.ToString(),
                    v => (DistanceUnit)Enum.Parse(typeof(DistanceUnit), v));

            _ = entity.Property(p => p.Theme)
                .HasColumnName("theme")
                .HasColumnType("ENUM('Dark', 'Light')")
                .HasConversion(
                    v => v.ToString(),
                    v => (Theme)Enum.Parse(typeof(Theme), v));
        });

        _ = modelBuilder.Entity<Message>(entity =>
        {
            _ = entity.ToTable("messages");
            _ = entity.HasKey(p => p.Id).HasName("PK_Message");

            _ = entity.Property(p => p.Id).HasColumnName("id").HasColumnType("int").ValueGeneratedOnAdd();
            _ = entity.Property(p => p.GroupId).HasColumnName("group_id").HasColumnType("int");
            _ = entity.Property(p => p.UserId).HasColumnName("user_id").HasColumnType("char(36)");
            _ = entity.Property(p => p.CreatedAt).HasColumnName("created_at").HasColumnType("datetime");
            _ = entity.Property(p => p.Content).HasColumnName("content");

            _ = entity.HasOne(m => m.Group)
                .WithMany()
                .HasForeignKey(m => m.GroupId);

            _ = entity.HasOne(m => m.User)
                .WithMany()
                .HasForeignKey(m => m.UserId);
        });

        _ = modelBuilder.Entity<GroupPath>(entity =>
        {
            _ = entity.ToTable("group_paths");

            _ = entity.HasKey(p => p.Id).HasName("PK_GroupPath");

            _ = entity.Property(p => p.Id)
                .HasColumnName("id")
                .HasColumnType("char(36)");

            _ = entity.Property(p => p.GroupId)
                .HasColumnName("group_id")
                .HasColumnType("int");

            _ = entity.HasIndex(p => p.GroupId)
                .IsUnique();

            _ = entity.HasOne(p => p.Group)
                .WithOne(g => g.CurrentPath)
                .HasForeignKey<GroupPath>(p => p.GroupId)
                .OnDelete(DeleteBehavior.Cascade);

            _ = entity.Property(p => p.CreatedAt).
                HasColumnName("created_at")
                .HasColumnType("datetime");

            _ = entity.Property(p => p.SerializedDto)
                .HasColumnName("serialized_dto")
                .HasColumnType("LONGTEXT");
        });

        _ = modelBuilder.Entity<ProposedPath>(entity =>
        {
            _ = entity.ToTable("proposed_paths");

            _ = entity.HasKey(p => p.Id).HasName("PK_ProposedPath");

            _ = entity.Property(p => p.Id)
                .HasColumnName("id")
                .HasColumnType("char(36)");

            _ = entity.Property(p => p.GroupId)
                .HasColumnName("group_id")
                .HasColumnType("int");

            _ = entity.HasOne(p => p.Group)
                .WithMany(g => g.ProposedPaths)
                .HasForeignKey(p => p.GroupId)
                .OnDelete(DeleteBehavior.Cascade);

            _ = entity.Property(p => p.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("datetime");

            _ = entity.Property(p => p.SerializedDto)
                .HasColumnName("serialized_dto")
                .HasColumnType("LONGTEXT");
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
            .WithMany(g => g.GroupMembers)
            .UsingEntity<Dictionary<string, object>>(
                "group_members",
                j => j.HasOne<Group>().WithMany().HasForeignKey("group_id"),
                j => j.HasOne<User>().WithMany().HasForeignKey("user_id"));

        _ = modelBuilder.Entity<User>()
            .HasOne(u => u.UserConfiguration)
            .WithOne()
            .HasForeignKey<UserConfiguration>(uc => uc.UserId);

        _ = modelBuilder.Entity<UserLocation>(entity =>
        {
            _ = entity.ToTable("user_locations");
            _ = entity.HasKey(p => p.Id).HasName("PK_user_locations");

            _ = entity.Property(p => p.Id)
                .HasColumnName("id")
                .HasColumnType("int").ValueGeneratedOnAdd();

            _ = entity.Property(p => p.UserId).HasColumnName("user_id").HasColumnType("char(36)");
            _ = entity.Property(p => p.Latitude).HasColumnName("latitude").HasColumnType("double");
            _ = entity.Property(p => p.Longitude).HasColumnName("longitude").HasColumnType("double");

            _ = entity.Property(p => p.UpdatedAt)
                .HasColumnName("updated_at")
                .HasColumnType("datetime")
                .HasDefaultValueSql("CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP");
        });

        _ = modelBuilder.Entity<User>()
            .HasOne(u => u.UserLocation)
            .WithOne()
            .HasForeignKey<UserLocation>(ul => ul.UserId);

        // Add FriendInvitation configuration
        _ = modelBuilder.Entity<FriendInvitation>(entity =>
        {
            entity.ToTable("friend_invitations");

            entity.HasKey(e => e.InvitationId);

            entity.Property(e => e.InvitationId)
                .HasColumnName("invitation_id");

            entity.Property(e => e.SenderId)
                .HasColumnName("sender_id");

            entity.Property(e => e.ReceiverId)
                .HasColumnName("receiver_id");

            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at");

            entity.HasOne(e => e.Sender)
                .WithMany()
                .HasForeignKey(e => e.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Receiver)
                .WithMany()
                .HasForeignKey(e => e.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
