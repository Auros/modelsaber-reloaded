using Microsoft.EntityFrameworkCore.Migrations;
using ModelSaber.Common;

namespace ModelSaber.API.Migrations
{
    public partial class License : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<License>(
                name: "license",
                table: "models",
                type: "jsonb",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "license",
                table: "models");
        }
    }
}
