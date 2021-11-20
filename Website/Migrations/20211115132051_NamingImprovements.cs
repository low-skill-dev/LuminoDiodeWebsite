using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using System;

namespace Website.Migrations
{
	public partial class NamingImprovements : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "DbDocuments",
				columns: table => new
				{
					Id = table.Column<int>(type: "integer", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					Title = table.Column<string>(type: "text", nullable: true),
					AuthorUserId = table.Column<int>(type: "integer", nullable: false),
					Tags = table.Column<string[]>(type: "text[]", nullable: true),
					CreatedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
					Utf8JsonSerializedParagraphs = table.Column<byte[]>(type: "bytea", nullable: true)
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
					ShortDescription = table.Column<string>(type: "text", nullable: true),
					OwnerId = table.Column<int>(type: "integer", nullable: false),
					AdminsId = table.Column<int[]>(type: "integer[]", nullable: true),
					OrderedDocumentsId = table.Column<int[]>(type: "integer[]", nullable: true)
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
					OwnerId = table.Column<int>(type: "integer", nullable: false),
					AdminsId = table.Column<int[]>(type: "integer[]", nullable: true),
					OrderedProjectsId = table.Column<int[]>(type: "integer[]", nullable: true),
					ShortDescription = table.Column<string>(type: "text", nullable: true)
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
					FirstName = table.Column<string>(type: "text", nullable: true),
					LastName = table.Column<string>(type: "text", nullable: true),
					AboutMe = table.Column<string>(type: "text", nullable: true),
					TelegramLink = table.Column<string>(type: "text", nullable: true),
					VkLink = table.Column<string>(type: "text", nullable: true),
					City = table.Column<string>(type: "text", nullable: true),
					PostalCode = table.Column<string>(type: "text", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Users", x => x.Id);
				});
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "DbDocuments");

			migrationBuilder.DropTable(
				name: "Projects");

			migrationBuilder.DropTable(
				name: "ProjectsGroups");

			migrationBuilder.DropTable(
				name: "Users");
		}
	}
}
