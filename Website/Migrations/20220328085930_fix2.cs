using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Website.Migrations
{
	public partial class fix2 : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AlterColumn<string>(
				name: "LastName",
				table: "Users",
				type: "character varying(30)",
				maxLength: 30,
				nullable: true,
				oldClrType: typeof(string),
				oldType: "character varying(30)",
				oldMaxLength: 30);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AlterColumn<string>(
				name: "LastName",
				table: "Users",
				type: "character varying(30)",
				maxLength: 30,
				nullable: false,
				defaultValue: "",
				oldClrType: typeof(string),
				oldType: "character varying(30)",
				oldMaxLength: 30,
				oldNullable: true);
		}
	}
}
