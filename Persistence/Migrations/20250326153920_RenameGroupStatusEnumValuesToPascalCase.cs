using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RenameGroupStatusEnumValuesToPascalCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "groups",
                type: "ENUM('NotStarted', 'Started')",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "ENUM('NOT_STARTED', 'STARTED')")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "groups",
                type: "ENUM('NOT_STARTED', 'STARTED')",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "ENUM('NotStarted', 'Started')")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
