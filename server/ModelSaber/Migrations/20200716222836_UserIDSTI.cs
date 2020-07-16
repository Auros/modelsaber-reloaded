using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ModelSaber.Migrations
{
    public partial class UserIDSTI : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string[]>(
                name: "permissions",
                table: "users",
                nullable: true,
                defaultValue: new[] { "*.upload" },
                oldClrType: typeof(string[]),
                oldType: "text[]",
                oldNullable: true,
                oldDefaultValue: new[] { "*.upload" });

            migrationBuilder.AlterColumn<string[]>(
                name: "external_profiles",
                table: "users",
                nullable: true,
                defaultValue: new string[] { },
                oldClrType: typeof(string[]),
                oldType: "text[]",
                oldNullable: true,
                oldDefaultValue: new string[] { });

            migrationBuilder.AlterColumn<string>(
                name: "id",
                table: "users",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<string[]>(
                name: "tags",
                table: "models",
                nullable: true,
                defaultValue: new string[] { },
                oldClrType: typeof(string[]),
                oldType: "text[]",
                oldNullable: true,
                oldDefaultValue: new string[] { });

            migrationBuilder.AlterColumn<long>(
                name: "id",
                table: "models",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "visibility",
                table: "models",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<long>(
                name: "id",
                table: "model_collections",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<long>(
                name: "id",
                table: "localization_table",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<long>(
                name: "id",
                table: "games",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "visibility",
                table: "games",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "visibility",
                table: "models");

            migrationBuilder.DropColumn(
                name: "visibility",
                table: "games");

            migrationBuilder.AlterColumn<string[]>(
                name: "permissions",
                table: "users",
                type: "text[]",
                nullable: true,
                defaultValue: new[] { "*.upload" },
                oldClrType: typeof(string[]),
                oldNullable: true,
                oldDefaultValue: new[] { "*.upload" });

            migrationBuilder.AlterColumn<string[]>(
                name: "external_profiles",
                table: "users",
                type: "text[]",
                nullable: true,
                defaultValue: new string[] { },
                oldClrType: typeof(string[]),
                oldNullable: true,
                oldDefaultValue: new string[] { });

            migrationBuilder.AlterColumn<long>(
                name: "id",
                table: "users",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<string[]>(
                name: "tags",
                table: "models",
                type: "text[]",
                nullable: true,
                defaultValue: new string[] { },
                oldClrType: typeof(string[]),
                oldNullable: true,
                oldDefaultValue: new string[] { });

            migrationBuilder.AlterColumn<long>(
                name: "id",
                table: "models",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<long>(
                name: "id",
                table: "model_collections",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<long>(
                name: "id",
                table: "localization_table",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<long>(
                name: "id",
                table: "games",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);
        }
    }
}
