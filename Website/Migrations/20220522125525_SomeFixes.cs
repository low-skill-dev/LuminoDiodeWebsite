using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Website.Migrations
{
    public partial class SomeFixes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreRenderedHtml",
                table: "DbDocuments");

            migrationBuilder.AddColumn<string>(
                name: "TextPrerenderedHtml",
                table: "DbDocuments",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "DbDocuments",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TextPrerenderedHtml",
                table: "DbDocuments");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "DbDocuments");

            migrationBuilder.AddColumn<string>(
                name: "PreRenderedHtml",
                table: "DbDocuments",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
