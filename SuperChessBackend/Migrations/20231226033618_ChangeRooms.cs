using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperChessBackend.Migrations
{
    /// <inheritdoc />
    public partial class ChangeRooms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_room_RoomId",
                table: "user");

            migrationBuilder.DropIndex(
                name: "IX_user_RoomId",
                table: "user");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "user");

            migrationBuilder.RenameColumn(
                name: "RoomCreationDate",
                table: "room",
                newName: "CreationDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "room",
                newName: "RoomCreationDate");

            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "user",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_RoomId",
                table: "user",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_user_room_RoomId",
                table: "user",
                column: "RoomId",
                principalTable: "room",
                principalColumn: "RoomId");
        }
    }
}
