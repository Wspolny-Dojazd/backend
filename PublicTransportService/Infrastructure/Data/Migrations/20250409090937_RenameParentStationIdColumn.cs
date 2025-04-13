using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PublicTransportService.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameParentStationIdColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "parent_station_id",
                table: "pts_stops",
                newName: "parent_station");

            migrationBuilder.AlterColumn<byte>(
                name: "location_type",
                table: "pts_stops",
                type: "tinyint unsigned",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<byte>(
                name: "pickup_type",
                table: "pts_stop_times",
                type: "tinyint unsigned",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<byte>(
                name: "drop_off_type",
                table: "pts_stop_times",
                type: "tinyint unsigned",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "parent_station",
                table: "pts_stops",
                newName: "parent_station_id");

            migrationBuilder.AlterColumn<int>(
                name: "location_type",
                table: "pts_stops",
                type: "int",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint unsigned");

            migrationBuilder.AlterColumn<int>(
                name: "pickup_type",
                table: "pts_stop_times",
                type: "int",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint unsigned");

            migrationBuilder.AlterColumn<int>(
                name: "drop_off_type",
                table: "pts_stop_times",
                type: "int",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint unsigned");
        }
    }
}
