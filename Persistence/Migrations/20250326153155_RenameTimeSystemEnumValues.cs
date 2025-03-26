using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RenameTimeSystemEnumValues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "time_system",
                table: "user_configurations",
                type: "ENUM('TwelveHour', 'TwentyFourHour')",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "ENUM('AMPM', 'TwentyFourHour')")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "time_system",
                table: "user_configurations",
                type: "ENUM('AMPM', 'TwentyFourHour')",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "ENUM('TwelveHour', 'TwentyFourHour')")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
