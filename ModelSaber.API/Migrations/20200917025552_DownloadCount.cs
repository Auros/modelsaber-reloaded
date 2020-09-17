using Microsoft.EntityFrameworkCore.Migrations;

namespace ModelSaber.API.Migrations
{
    public partial class DownloadCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "download_count",
                table: "models",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "download_count",
                table: "models");
        }
    }
}
