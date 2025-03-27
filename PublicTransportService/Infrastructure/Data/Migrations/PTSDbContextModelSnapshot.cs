﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PublicTransportService.Infrastructure.Data;

#nullable disable

namespace PublicTransportService.Infrastructure.Data.Migrations
{
    [DbContext(typeof(PTSDbContext))]
    partial class PTSDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

                    b.Property<int>("LocationType")
                        .HasColumnType("int")
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

                    b.Property<string>("ParentStationId")
                        .HasColumnType("longtext")
                        .HasColumnName("parent_station_id");

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

                    b.Property<int>("DropOffType")
                        .HasColumnType("int")
                        .HasColumnName("drop_off_type");

                    b.Property<int>("PickupType")
                        .HasColumnType("int")
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
#pragma warning restore 612, 618
        }
    }
}
