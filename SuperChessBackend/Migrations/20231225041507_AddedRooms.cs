using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SuperChessBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddedRooms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "user",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "game",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "room",
                columns: table => new
                {
                    RoomId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoomName = table.Column<Guid>(type: "uuid", nullable: false),
                    RoomCreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_room", x => x.RoomId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_user_RoomId",
                table: "user",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_game_RoomId",
                table: "game",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_game_room_RoomId",
                table: "game",
                column: "RoomId",
                principalTable: "room",
                principalColumn: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_user_room_RoomId",
                table: "user",
                column: "RoomId",
                principalTable: "room",
                principalColumn: "RoomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_game_room_RoomId",
                table: "game");

            migrationBuilder.DropForeignKey(
                name: "FK_user_room_RoomId",
                table: "user");

            migrationBuilder.DropTable(
                name: "room");

            migrationBuilder.DropIndex(
                name: "IX_user_RoomId",
                table: "user");

            migrationBuilder.DropIndex(
                name: "IX_game_RoomId",
                table: "game");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "user");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "game");
        }
    }
}
