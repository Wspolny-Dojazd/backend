﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PublicTransportService.Infrastructure.Data;

#nullable disable

namespace PublicTransportService.Infrastructure.Data.Migrations
{
    [DbContext(typeof(PTSDbContext))]
    [Migration("20250409090937_RenameParentStationIdColumn")]
    partial class RenameParentStationIdColumn
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("PublicTransportService.Domain.Entities.Route", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)")
                        .HasColumnName("id");

                    b.Property<string>("AgencyId")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("agency_id");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("color");

                    b.Property<string>("LongName")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("long_name");

                    b.Property<string>("ShortName")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("short_name");

                    b.Property<string>("TextColor")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("text_color");

                    b.Property<int>("Type")
                        .HasColumnType("int")
                        .HasColumnName("type");

                    b.HasKey("Id")
                        .HasName("pk_pts_routes");

                    b.ToTable("pts_routes", (string)null);
                });

            modelBuilder.Entity("PublicTransportService.Domain.Entities.Shape", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)")
                        .HasColumnName("id");

                    b.Property<int>("PtSequence")
                        .HasColumnType("int")
                        .HasColumnName("pt_sequence");

                    b.Property<double>("PtLatitude")
                        .HasColumnType("double")
                        .HasColumnName("pt_latitude");

                    b.Property<double>("PtLongitude")
                        .HasColumnType("double")
                        .HasColumnName("pt_longitude");

                    b.HasKey("Id", "PtSequence")
                        .HasName("pk_pts_shapes");

                    b.ToTable("pts_shapes", (string)null);
                });

            modelBuilder.Entity("PublicTransportService.Domain.Entities.Stop", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)")
                        .HasColumnName("id");

                    b.Property<string>("Code")
                        .HasColumnType("longtext")
                        .HasColumnName("code");

                    b.Property<double>("Latitude")
                        .HasColumnType("double")
                        .HasColumnName("latitude");

                    b.Property<byte>("LocationType")
                        .HasColumnType("tinyint unsigned")
                        .HasColumnName("location_type");

                    b.Property<double>("Longitude")
                        .HasColumnType("double")
                        .HasColumnName("longitude");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("name");

                    b.Property<string>("NameStem")
                        .HasColumnType("longtext")
                        .HasColumnName("name_stem");

                    b.Property<string>("ParentStation")
                        .HasColumnType("longtext")
                        .HasColumnName("parent_station");

                    b.Property<string>("PlatformCode")
                        .HasColumnType("longtext")
                        .HasColumnName("platform_code");

                    b.Property<string>("StreetName")
                        .HasColumnType("longtext")
                        .HasColumnName("street_name");

                    b.Property<string>("TownName")
                        .HasColumnType("longtext")
                        .HasColumnName("town_name");

                    b.Property<bool>("WheelchairBoarding")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("wheelchair_boarding");

                    b.HasKey("Id")
                        .HasName("pk_pts_stops");

                    b.ToTable("pts_stops", (string)null);
                });

            modelBuilder.Entity("PublicTransportService.Domain.Entities.StopTime", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("ArrivalTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("arrival_time");

                    b.Property<DateTime>("DepartureTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("departure_time");

                    b.Property<byte>("DropOffType")
                        .HasColumnType("tinyint unsigned")
                        .HasColumnName("drop_off_type");

                    b.Property<byte>("PickupType")
                        .HasColumnType("tinyint unsigned")
                        .HasColumnName("pickup_type");

                    b.Property<string>("StopId")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("stop_id");

                    b.Property<int>("StopSequence")
                        .HasColumnType("int")
                        .HasColumnName("stop_sequence");

                    b.Property<string>("TripId")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("trip_id");

                    b.HasKey("Id")
                        .HasName("pk_pts_stop_times");

                    b.ToTable("pts_stop_times", (string)null);
                });

            modelBuilder.Entity("Trip", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)")
                        .HasColumnName("id");

                    b.Property<string>("Brigade")
                        .HasColumnType("longtext")
                        .HasColumnName("brigade");

                    b.Property<int>("DirectionId")
                        .HasColumnType("int")
                        .HasColumnName("direction_id");

                    b.Property<string>("FleetType")
                        .HasColumnType("longtext")
                        .HasColumnName("fleet_type");

                    b.Property<string>("HeadSign")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("head_sign");

                    b.Property<string>("HiddenBlockId")
                        .HasColumnType("longtext")
                        .HasColumnName("hidden_block_id");

                    b.Property<string>("RouteId")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("route_id");

                    b.Property<string>("ServiceId")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("service_id");

                    b.Property<string>("ShapeId")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("shape_id");

                    b.Property<string>("ShortName")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("trip_short_name");

                    b.Property<bool>("WheelchairAccessible")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("wheelchair_accessible");

                    b.HasKey("Id")
                        .HasName("pk_pts_trips");

                    b.ToTable("pts_trips", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
