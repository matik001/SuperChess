using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperChessBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddedGuestGuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GuestGuid",
                table: "gameuser",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GuestGuid",
                table: "gameuser");
        }
    }
}
