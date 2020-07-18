using Microsoft.EntityFrameworkCore.Migrations;

namespace ModelSaber.Migrations
{
    public partial class GameCollectionRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "game_id",
                table: "model_collections",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "ix_model_collections_game_id",
                table: "model_collections",
                column: "game_id");

            migrationBuilder.AddForeignKey(
                name: "fk_model_collections_games_game_id",
                table: "model_collections",
                column: "game_id",
                principalTable: "games",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_model_collections_games_game_id",
                table: "model_collections");

            migrationBuilder.DropIndex(
                name: "ix_model_collections_game_id",
                table: "model_collections");

            migrationBuilder.DropColumn(
                name: "game_id",
                table: "model_collections");
        }
    }
}
