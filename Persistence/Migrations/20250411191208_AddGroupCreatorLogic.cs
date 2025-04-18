using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddGroupCreatorLogic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "creator_id",
                table: "groups",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "ix_groups_creator_id",
                table: "groups",
                column: "creator_id");

            migrationBuilder.AddForeignKey(
                name: "fk_groups_users_creator_id",
                table: "groups",
                column: "creator_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_groups_users_creator_id",
                table: "groups");

            migrationBuilder.DropIndex(
                name: "ix_groups_creator_id",
                table: "groups");

            migrationBuilder.DropColumn(
                name: "creator_id",
                table: "groups");
        }
    }
}
