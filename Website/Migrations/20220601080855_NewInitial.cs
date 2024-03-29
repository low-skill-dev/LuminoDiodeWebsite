﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using NpgsqlTypes;

#nullable disable

namespace Website.Migrations
{
    public partial class NewInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DbDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    TitleTsVector = table.Column<NpgsqlTsVector>(type: "tsvector", nullable: false)
                        .Annotation("Npgsql:TsVectorConfig", "english")
                        .Annotation("Npgsql:TsVectorProperties", new[] { "Title" }),
                    AuthorId = table.Column<int>(type: "integer", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Utf8JsonSerializedParagraphs = table.Column<byte[]>(type: "bytea", nullable: false),
                    TextPrerenderedHtml = table.Column<string>(type: "text", nullable: true),
                    PreRenderedHtmlCreationDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ProjectId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbDocuments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProjectType = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    ShortDescription = table.Column<string>(type: "text", nullable: true),
                    OwnerId = table.Column<int>(type: "integer", nullable: true),
                    ProjectsGroupId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectsGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OwnerIdId = table.Column<int>(type: "integer", nullable: false),
                    ShortDescription = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectsGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserType = table.Column<int>(type: "integer", nullable: false),
                    DisplayedName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    AboutMe = table.Column<string>(type: "character varying(3000)", maxLength: 3000, nullable: true),
                    TelegramLink = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    VkLink = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    City = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    PostalCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    String64_ProfileImage = table.Column<string>(type: "text", nullable: true),
                    EmailAdress = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    AuthHashedPassword = table.Column<byte[]>(type: "bytea", nullable: true),
                    AuthPasswordSalt = table.Column<byte[]>(type: "bytea", nullable: true),
                    RegistrationStartedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RegistrationCompleteDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RegistrationStage = table.Column<int>(type: "integer", nullable: false),
                    ProjectId = table.Column<int>(type: "integer", nullable: true),
                    ProjectsGroupId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.UniqueConstraint("AK_Users_EmailAdress", x => x.EmailAdress);
                    table.ForeignKey(
                        name: "FK_Users_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Users_ProjectsGroups_ProjectsGroupId",
                        column: x => x.ProjectsGroupId,
                        principalTable: "ProjectsGroups",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DbDocuments_AuthorId",
                table: "DbDocuments",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_DbDocuments_ProjectId",
                table: "DbDocuments",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_DbDocuments_TitleTsVector",
                table: "DbDocuments",
                column: "TitleTsVector")
                .Annotation("Npgsql:IndexMethod", "GIN");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_OwnerId",
                table: "Projects",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProjectsGroupId",
                table: "Projects",
                column: "ProjectsGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectsGroups_OwnerIdId",
                table: "ProjectsGroups",
                column: "OwnerIdId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ProjectId",
                table: "Users",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ProjectsGroupId",
                table: "Users",
                column: "ProjectsGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_DbDocuments_Projects_ProjectId",
                table: "DbDocuments",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DbDocuments_Users_AuthorId",
                table: "DbDocuments",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_ProjectsGroups_ProjectsGroupId",
                table: "Projects",
                column: "ProjectsGroupId",
                principalTable: "ProjectsGroups",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Users_OwnerId",
                table: "Projects",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectsGroups_Users_OwnerIdId",
                table: "ProjectsGroups",
                column: "OwnerIdId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Projects_ProjectId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectsGroups_Users_OwnerIdId",
                table: "ProjectsGroups");

            migrationBuilder.DropTable(
                name: "DbDocuments");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "ProjectsGroups");
        }
    }
}
