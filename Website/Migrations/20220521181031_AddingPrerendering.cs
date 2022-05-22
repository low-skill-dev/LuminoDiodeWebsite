using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Website.Migrations
{
    public partial class AddingPrerendering : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PreRenderedHtml",
                table: "DbDocuments",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "PreRenderedHtmlCreationDateTime",
                table: "DbDocuments",
                type: "timestamp with time zone",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreRenderedHtml",
                table: "DbDocuments");

            migrationBuilder.DropColumn(
                name: "PreRenderedHtmlCreationDateTime",
                table: "DbDocuments");
        }
    }
}
