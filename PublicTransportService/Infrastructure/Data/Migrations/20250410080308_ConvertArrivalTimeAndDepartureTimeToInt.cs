using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PublicTransportService.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ConvertArrivalTimeAndDepartureTimeToInt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "departure_time",
                table: "pts_stop_times",
                type: "int",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<int>(
                name: "arrival_time",
                table: "pts_stop_times",
                type: "int",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "departure_time",
                table: "pts_stop_times",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "arrival_time",
                table: "pts_stop_times",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
