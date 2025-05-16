using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Systemthemeadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "theme",
                table: "user_configurations",
                type: "ENUM('Dark', 'Light', 'System')",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "ENUM('Dark', 'Light')")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "theme",
                table: "user_configurations",
                type: "ENUM('Dark', 'Light')",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "ENUM('Dark', 'Light', 'System')")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
