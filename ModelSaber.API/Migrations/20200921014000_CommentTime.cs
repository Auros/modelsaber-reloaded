using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ModelSaber.API.Migrations
{
    public partial class CommentTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "time",
                table: "comments",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "time",
                table: "comments");
        }
    }
}
