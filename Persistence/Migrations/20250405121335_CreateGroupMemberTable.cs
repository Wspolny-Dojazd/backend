using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CreateGroupMemberTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "user_id",
                table: "groups",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "ix_groups_user_id",
                table: "groups",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_groups_users_user_id",
                table: "groups",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_groups_users_user_id",
                table: "groups");

            migrationBuilder.DropIndex(
                name: "ix_groups_user_id",
                table: "groups");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "groups");
        }
    }
}
