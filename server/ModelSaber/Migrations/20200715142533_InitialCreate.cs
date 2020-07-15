using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ModelSaber.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "discord_user",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    username = table.Column<string>(nullable: true),
                    discriminator = table.Column<string>(nullable: true),
                    avatar = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_discord_user", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "games",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false),
                    title = table.Column<string>(nullable: false),
                    created = table.Column<DateTime>(nullable: false),
                    description = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_games", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "localization_table",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false),
                    data = table.Column<string>(nullable: true),
                    locale = table.Column<string>(nullable: true),
                    source = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_localization_table", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "model_collections",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false),
                    name = table.Column<string>(nullable: false),
                    icon_url = table.Column<string>(nullable: true),
                    created = table.Column<DateTime>(nullable: false),
                    description = table.Column<string>(nullable: false),
                    install_path = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_model_collections", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false),
                    profile_id = table.Column<string>(nullable: true),
                    permissions = table.Column<string[]>(nullable: true, defaultValue: new[] { "*.upload" }),
                    external_profiles = table.Column<string[]>(nullable: true, defaultValue: new string[] {  })
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                    table.ForeignKey(
                        name: "fk_users_discord_user_profile_id",
                        column: x => x.profile_id,
                        principalTable: "discord_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "models",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false),
                    name = table.Column<string>(nullable: false),
                    hash = table.Column<string>(nullable: false),
                    tags = table.Column<string[]>(nullable: true, defaultValue: new string[] {  }),
                    preview = table.Column<string>(nullable: false),
                    created = table.Column<DateTime>(nullable: false),
                    collection_id = table.Column<long>(nullable: false),
                    install_url = table.Column<string>(nullable: true),
                    download_url = table.Column<string>(nullable: false),
                    type = table.Column<int>(nullable: false, defaultValue: 0),
                    is_variation = table.Column<bool>(nullable: false),
                    variations = table.Column<long[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_models", x => x.id);
                    table.ForeignKey(
                        name: "fk_models_model_collections_collection_id",
                        column: x => x.collection_id,
                        principalTable: "model_collections",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_models_collection_id",
                table: "models",
                column: "collection_id");

            migrationBuilder.CreateIndex(
                name: "ix_users_profile_id",
                table: "users",
                column: "profile_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "games");

            migrationBuilder.DropTable(
                name: "localization_table");

            migrationBuilder.DropTable(
                name: "models");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "model_collections");

            migrationBuilder.DropTable(
                name: "discord_user");
        }
    }
}
