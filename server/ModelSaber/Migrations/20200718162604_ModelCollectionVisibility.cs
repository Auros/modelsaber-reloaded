using Microsoft.EntityFrameworkCore.Migrations;

namespace ModelSaber.Migrations
{
    public partial class ModelCollectionVisibility : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "visibility",
                table: "model_collections",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "visibility",
                table: "model_collections");
        }
    }
}
