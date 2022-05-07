using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Website.Migrations
{
    public partial class fromString64ToByteArrayAgain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthHashedPasswordString64",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AuthPasswordSaltString64",
                table: "Users");

            migrationBuilder.AddColumn<byte[]>(
                name: "AuthHashedPassword",
                table: "Users",
                type: "bytea",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "AuthPasswordSalt",
                table: "Users",
                type: "bytea",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthHashedPassword",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AuthPasswordSalt",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "AuthHashedPasswordString64",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AuthPasswordSaltString64",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
