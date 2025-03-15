using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CreatedAtAddedtoUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_userConfigurations_users_user_id",
                table: "userConfigurations");

            migrationBuilder.RenameTable(
                name: "userConfigurations",
                newName: "user_configurations");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "users",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_userConfigurations_user_id",
                table: "user_configurations",
                newName: "IX_user_configurations_user_id");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "users",
                type: "datetime",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "groups",
                type: "ENUM('NOT_STARTED', 'STARTED')",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "language",
                table: "user_configurations",
                type: "ENUM('English', 'Polish')",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "ENUM('English', 'Polish', 'German')")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_user_configurations_users_user_id",
                table: "user_configurations",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_configurations_users_user_id",
                table: "user_configurations");

            migrationBuilder.RenameTable(
                name: "user_configurations",
                newName: "userConfigurations");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "users",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "IX_user_configurations_user_id",
                table: "userConfigurations",
                newName: "IX_userConfigurations_user_id");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "users",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<int>(
                name: "status",
                table: "groups",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "ENUM('NOT_STARTED', 'STARTED')")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "language",
                table: "userConfigurations",
                type: "ENUM('English', 'Polish', 'German')",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "ENUM('English', 'Polish')")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_userConfigurations_users_user_id",
                table: "userConfigurations",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
