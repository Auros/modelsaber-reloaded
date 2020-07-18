using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ModelSaber.Migrations
{
    public partial class Initial : Migration
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
                    id = table.Column<decimal>(nullable: false),
                    title = table.Column<string>(nullable: false),
                    icon_url = table.Column<string>(nullable: true),
                    created = table.Column<DateTime>(nullable: false),
                    description = table.Column<string>(nullable: false),
                    visibility = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_games", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "model_collections",
                columns: table => new
                {
                    id = table.Column<decimal>(nullable: false),
                    name = table.Column<string>(nullable: false),
                    icon_url = table.Column<string>(nullable: true),
                    created = table.Column<DateTime>(nullable: false),
                    description = table.Column<string>(nullable: false),
                    install_path = table.Column<string>(nullable: true),
                    file_extension = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_model_collections", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "model_stats",
                columns: table => new
                {
                    id = table.Column<decimal>(nullable: false),
                    downloads = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_model_stats", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    profile_id = table.Column<string>(nullable: true),
                    role = table.Column<int>(nullable: false, defaultValue: 0),
                    external_profiles = table.Column<List<string>>(nullable: true)
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
                    id = table.Column<decimal>(nullable: false),
                    name = table.Column<string>(nullable: false),
                    hash = table.Column<string>(nullable: false),
                    preview = table.Column<string>(nullable: false),
                    created = table.Column<DateTime>(nullable: false),
                    tags = table.Column<List<string>>(nullable: true),
                    install_url = table.Column<string>(nullable: true),
                    collection_id = table.Column<decimal>(nullable: false),
                    download_url = table.Column<string>(nullable: false),
                    type = table.Column<int>(nullable: false, defaultValue: 0),
                    visibility = table.Column<int>(nullable: false),
                    variation_of_id = table.Column<decimal>(nullable: true),
                    stats_id = table.Column<decimal>(nullable: true)
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
                    table.ForeignKey(
                        name: "fk_models_model_stats_stats_id",
                        column: x => x.stats_id,
                        principalTable: "model_stats",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_models_models_variation_of_id",
                        column: x => x.variation_of_id,
                        principalTable: "models",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "vote",
                columns: table => new
                {
                    id = table.Column<decimal>(nullable: false),
                    voter_id = table.Column<string>(nullable: true),
                    is_upvote = table.Column<bool>(nullable: false),
                    model_stats_id = table.Column<decimal>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_vote", x => x.id);
                    table.ForeignKey(
                        name: "fk_vote_model_stats_model_stats_id",
                        column: x => x.model_stats_id,
                        principalTable: "model_stats",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_vote_users_voter_id",
                        column: x => x.voter_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_models_collection_id",
                table: "models",
                column: "collection_id");

            migrationBuilder.CreateIndex(
                name: "ix_models_stats_id",
                table: "models",
                column: "stats_id");

            migrationBuilder.CreateIndex(
                name: "ix_models_variation_of_id",
                table: "models",
                column: "variation_of_id");

            migrationBuilder.CreateIndex(
                name: "ix_users_profile_id",
                table: "users",
                column: "profile_id");

            migrationBuilder.CreateIndex(
                name: "ix_vote_model_stats_id",
                table: "vote",
                column: "model_stats_id");

            migrationBuilder.CreateIndex(
                name: "ix_vote_voter_id",
                table: "vote",
                column: "voter_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "games");

            migrationBuilder.DropTable(
                name: "models");

            migrationBuilder.DropTable(
                name: "vote");

            migrationBuilder.DropTable(
                name: "model_collections");

            migrationBuilder.DropTable(
                name: "model_stats");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "discord_user");
        }
    }
}
