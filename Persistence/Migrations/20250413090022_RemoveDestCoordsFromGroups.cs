using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDestCoordsFromGroups : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "destination_lat",
                table: "groups");

            migrationBuilder.DropColumn(
                name: "destination_lon",
                table: "groups");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "destination_lat",
                table: "groups",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "destination_lon",
                table: "groups",
                type: "double",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
