using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFriendInvitations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_friends_users_friend_id",
                table: "friends");

            migrationBuilder.DropForeignKey(
                name: "fk_friends_users_user_id",
                table: "friends");

            migrationBuilder.DropPrimaryKey(
                name: "pk_friends",
                table: "friends");

            migrationBuilder.DropIndex(
                name: "ix_friends_user_id",
                table: "friends");

            migrationBuilder.AddColumn<Guid>(
                name: "user_id",
                table: "users",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddPrimaryKey(
                name: "pk_friends",
                table: "friends",
                columns: new[] { "user_id", "friend_id" });

            migrationBuilder.CreateTable(
                name: "friend_invitations",
                columns: table => new
                {
                    invitation_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    sender_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    receiver_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_friend_invitations", x => x.invitation_id);
                    table.ForeignKey(
                        name: "fk_friend_invitations_users_receiver_id",
                        column: x => x.receiver_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_friend_invitations_users_sender_id",
                        column: x => x.sender_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "ix_users_user_id",
                table: "users",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_friends_friend_id",
                table: "friends",
                column: "friend_id");

            migrationBuilder.CreateIndex(
                name: "ix_friend_invitations_receiver_id",
                table: "friend_invitations",
                column: "receiver_id");

            migrationBuilder.CreateIndex(
                name: "ix_friend_invitations_sender_id",
                table: "friend_invitations",
                column: "sender_id");

            migrationBuilder.AddForeignKey(
                name: "fk_friends_users_user_id",
                table: "friends",
                column: "friend_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_friends_users_user_id1",
                table: "friends",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_users_users_user_id",
                table: "users",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_friends_users_user_id",
                table: "friends");

            migrationBuilder.DropForeignKey(
                name: "fk_friends_users_user_id1",
                table: "friends");

            migrationBuilder.DropForeignKey(
                name: "fk_users_users_user_id",
                table: "users");

            migrationBuilder.DropTable(
                name: "friend_invitations");

            migrationBuilder.DropIndex(
                name: "ix_users_user_id",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "pk_friends",
                table: "friends");

            migrationBuilder.DropIndex(
                name: "ix_friends_friend_id",
                table: "friends");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "users");

            migrationBuilder.AddPrimaryKey(
                name: "pk_friends",
                table: "friends",
                columns: new[] { "friend_id", "user_id" });

            migrationBuilder.CreateIndex(
                name: "ix_friends_user_id",
                table: "friends",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_friends_users_friend_id",
                table: "friends",
                column: "friend_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_friends_users_user_id",
                table: "friends",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
