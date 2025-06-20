﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Persistence;

#nullable disable

namespace Persistence.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20250524094054_AddGroupInvitationTable")]
    partial class AddGroupInvitationTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("Domain.Model.FriendInvitation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("created_at");

                    b.Property<Guid>("ReceiverId")
                        .HasColumnType("char(36)")
                        .HasColumnName("receiver_id");

                    b.Property<Guid>("SenderId")
                        .HasColumnType("char(36)")
                        .HasColumnName("sender_id");

                    b.HasKey("Id")
                        .HasName("pk_friend_invitations");

                    b.HasIndex("ReceiverId")
                        .HasDatabaseName("ix_friend_invitations_receiver_id");

                    b.HasIndex("SenderId")
                        .HasDatabaseName("ix_friend_invitations_sender_id");

                    b.ToTable("friend_invitations", (string)null);
                });

            modelBuilder.Entity("Domain.Model.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("char(36)")
                        .HasColumnName("creator_id");

                    b.Property<string>("JoiningCode")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("joining_code");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("ENUM('NotStarted', 'Started')")
                        .HasColumnName("status");

                    b.HasKey("Id")
                        .HasName("PK_Group");

                    b.HasIndex("CreatorId")
                        .HasDatabaseName("ix_groups_creator_id");

                    b.ToTable("groups", (string)null);
                });

            modelBuilder.Entity("Domain.Model.GroupInvitation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("created_at");

                    b.Property<int>("GroupId")
                        .HasColumnType("int")
                        .HasColumnName("group_id");

                    b.Property<Guid>("ReceiverId")
                        .HasColumnType("char(36)")
                        .HasColumnName("receiver_id");

                    b.Property<Guid>("SenderId")
                        .HasColumnType("char(36)")
                        .HasColumnName("sender_id");

                    b.HasKey("Id")
                        .HasName("pk_group_invitations");

                    b.HasIndex("GroupId")
                        .HasDatabaseName("ix_group_invitations_group_id");

                    b.HasIndex("ReceiverId")
                        .HasDatabaseName("ix_group_invitations_receiver_id");

                    b.HasIndex("SenderId")
                        .HasDatabaseName("ix_group_invitations_sender_id");

                    b.ToTable("group_invitations", (string)null);
                });

            modelBuilder.Entity("Domain.Model.GroupPath", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime")
                        .HasColumnName("created_at");

                    b.Property<int>("GroupId")
                        .HasColumnType("int")
                        .HasColumnName("group_id");

                    b.Property<string>("SerializedDto")
                        .IsRequired()
                        .HasColumnType("LONGTEXT")
                        .HasColumnName("serialized_dto");

                    b.HasKey("Id")
                        .HasName("PK_GroupPath");

                    b.HasIndex("GroupId")
                        .IsUnique()
                        .HasDatabaseName("ix_group_paths_group_id");

                    b.ToTable("group_paths", (string)null);
                });

            modelBuilder.Entity("Domain.Model.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("content");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime")
                        .HasColumnName("created_at");

                    b.Property<int>("GroupId")
                        .HasColumnType("int")
                        .HasColumnName("group_id");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("PK_Message");

                    b.HasIndex("GroupId")
                        .HasDatabaseName("ix_messages_group_id");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_messages_user_id");

                    b.ToTable("messages", (string)null);
                });

            modelBuilder.Entity("Domain.Model.ProposedPath", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime")
                        .HasColumnName("created_at");

                    b.Property<int>("GroupId")
                        .HasColumnType("int")
                        .HasColumnName("group_id");

                    b.Property<string>("SerializedDto")
                        .IsRequired()
                        .HasColumnType("LONGTEXT")
                        .HasColumnName("serialized_dto");

                    b.HasKey("Id")
                        .HasName("PK_ProposedPath");

                    b.HasIndex("GroupId")
                        .HasDatabaseName("ix_proposed_paths_group_id");

                    b.ToTable("proposed_paths", (string)null);
                });

            modelBuilder.Entity("Domain.Model.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("email");

                    b.Property<string>("Nickname")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("nickname");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("password_hash");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("refresh_token");

                    b.Property<DateTime>("RefreshTokenExpiryTime")
                        .HasColumnType("datetime")
                        .HasColumnName("refresh_token_expiry_time");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("varchar(32)")
                        .HasColumnName("username");

                    b.HasKey("Id")
                        .HasName("PK_User");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("Domain.Model.UserConfiguration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("DistanceUnit")
                        .IsRequired()
                        .HasColumnType("ENUM('Kilometers', 'Miles')")
                        .HasColumnName("distance_unit");

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasColumnType("ENUM('English', 'Polish')")
                        .HasColumnName("language");

                    b.Property<string>("Theme")
                        .IsRequired()
                        .HasColumnType("ENUM('Dark', 'Light', 'System')")
                        .HasColumnName("theme");

                    b.Property<string>("TimeSystem")
                        .IsRequired()
                        .HasColumnType("ENUM('TwelveHour', 'TwentyFourHour')")
                        .HasColumnName("time_system");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("PK_UserConfiguration");

                    b.HasIndex("UserId")
                        .IsUnique()
                        .HasDatabaseName("ix_user_configurations_user_id");

                    b.ToTable("user_configurations", (string)null);
                });

            modelBuilder.Entity("Domain.Model.UserLocation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<double?>("Latitude")
                        .HasColumnType("double")
                        .HasColumnName("latitude");

                    b.Property<double?>("Longitude")
                        .HasColumnType("double")
                        .HasColumnName("longitude");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("updated_at")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("PK_user_locations");

                    b.HasIndex("UserId")
                        .IsUnique()
                        .HasDatabaseName("ix_user_locations_user_id");

                    b.ToTable("user_locations", (string)null);
                });

            modelBuilder.Entity("friends", b =>
                {
                    b.Property<Guid>("friend_id")
                        .HasColumnType("char(36)")
                        .HasColumnName("friend_id");

                    b.Property<Guid>("user_id")
                        .HasColumnType("char(36)")
                        .HasColumnName("user_id");

                    b.HasKey("friend_id", "user_id")
                        .HasName("pk_friends");

                    b.HasIndex("user_id")
                        .HasDatabaseName("ix_friends_user_id");

                    b.ToTable("friends", (string)null);
                });

            modelBuilder.Entity("group_members", b =>
                {
                    b.Property<int>("group_id")
                        .HasColumnType("int")
                        .HasColumnName("group_id");

                    b.Property<Guid>("user_id")
                        .HasColumnType("char(36)")
                        .HasColumnName("user_id");

                    b.HasKey("group_id", "user_id")
                        .HasName("pk_group_members");

                    b.HasIndex("user_id")
                        .HasDatabaseName("ix_group_members_user_id");

                    b.ToTable("group_members", (string)null);
                });

            modelBuilder.Entity("Domain.Model.FriendInvitation", b =>
                {
                    b.HasOne("Domain.Model.User", "Receiver")
                        .WithMany()
                        .HasForeignKey("ReceiverId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_friend_invitations_users_receiver_id");

                    b.HasOne("Domain.Model.User", "Sender")
                        .WithMany()
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_friend_invitations_users_sender_id");

                    b.Navigation("Receiver");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("Domain.Model.Group", b =>
                {
                    b.HasOne("Domain.Model.User", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_groups_users_creator_id");

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("Domain.Model.GroupInvitation", b =>
                {
                    b.HasOne("Domain.Model.Group", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_group_invitations_groups_group_id");

                    b.HasOne("Domain.Model.User", "Receiver")
                        .WithMany()
                        .HasForeignKey("ReceiverId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_group_invitations_users_receiver_id");

                    b.HasOne("Domain.Model.User", "Sender")
                        .WithMany()
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_group_invitations_users_sender_id");

                    b.Navigation("Group");

                    b.Navigation("Receiver");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("Domain.Model.GroupPath", b =>
                {
                    b.HasOne("Domain.Model.Group", "Group")
                        .WithOne("CurrentPath")
                        .HasForeignKey("Domain.Model.GroupPath", "GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_group_paths_groups_group_id");

                    b.Navigation("Group");
                });

            modelBuilder.Entity("Domain.Model.Message", b =>
                {
                    b.HasOne("Domain.Model.Group", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_messages_groups_group_id");

                    b.HasOne("Domain.Model.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_messages_users_user_id");

                    b.Navigation("Group");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Model.ProposedPath", b =>
                {
                    b.HasOne("Domain.Model.Group", "Group")
                        .WithMany("ProposedPaths")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_proposed_paths_groups_group_id");

                    b.Navigation("Group");
                });

            modelBuilder.Entity("Domain.Model.UserConfiguration", b =>
                {
                    b.HasOne("Domain.Model.User", null)
                        .WithOne("UserConfiguration")
                        .HasForeignKey("Domain.Model.UserConfiguration", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_configurations_users_user_id");
                });

            modelBuilder.Entity("Domain.Model.UserLocation", b =>
                {
                    b.HasOne("Domain.Model.User", null)
                        .WithOne("UserLocation")
                        .HasForeignKey("Domain.Model.UserLocation", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_locations_users_user_id");
                });

            modelBuilder.Entity("friends", b =>
                {
                    b.HasOne("Domain.Model.User", null)
                        .WithMany()
                        .HasForeignKey("friend_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_friends_users_friend_id");

                    b.HasOne("Domain.Model.User", null)
                        .WithMany()
                        .HasForeignKey("user_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_friends_users_user_id");
                });

            modelBuilder.Entity("group_members", b =>
                {
                    b.HasOne("Domain.Model.Group", null)
                        .WithMany()
                        .HasForeignKey("group_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_group_members_groups_group_id");

                    b.HasOne("Domain.Model.User", null)
                        .WithMany()
                        .HasForeignKey("user_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_group_members_users_user_id");
                });

            modelBuilder.Entity("Domain.Model.Group", b =>
                {
                    b.Navigation("CurrentPath");

                    b.Navigation("ProposedPaths");
                });

            modelBuilder.Entity("Domain.Model.User", b =>
                {
                    b.Navigation("UserConfiguration")
                        .IsRequired();

                    b.Navigation("UserLocation");
                });
#pragma warning restore 612, 618
        }
    }
}
