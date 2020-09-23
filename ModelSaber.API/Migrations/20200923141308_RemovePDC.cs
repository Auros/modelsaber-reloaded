using Microsoft.EntityFrameworkCore.Migrations;

namespace ModelSaber.API.Migrations
{
    public partial class RemovePDC : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "download_count",
                table: "playlists");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "download_count",
                table: "playlists",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
