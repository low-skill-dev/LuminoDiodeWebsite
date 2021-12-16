using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Website.Migrations
{
    public partial class FromIdToDirectEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminsId",
                table: "ProjectsGroups");

            migrationBuilder.DropColumn(
                name: "OrderedProjectsId",
                table: "ProjectsGroups");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "ProjectsGroups");

            migrationBuilder.DropColumn(
                name: "AdminsId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "OrderedDocumentsId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Projects");

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProjectsGroupId",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OwnerIdId",
                table: "ProjectsGroups",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OwnerIdId",
                table: "Projects",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "DbDocuments",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProjectsGroupId",
                table: "DbDocuments",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_ProjectId",
                table: "Users",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ProjectsGroupId",
                table: "Users",
                column: "ProjectsGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectsGroups_OwnerIdId",
                table: "ProjectsGroups",
                column: "OwnerIdId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_OwnerIdId",
                table: "Projects",
                column: "OwnerIdId");

            migrationBuilder.CreateIndex(
                name: "IX_DbDocuments_ProjectId",
                table: "DbDocuments",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_DbDocuments_ProjectsGroupId",
                table: "DbDocuments",
                column: "ProjectsGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_DbDocuments_Projects_ProjectId",
                table: "DbDocuments",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DbDocuments_ProjectsGroups_ProjectsGroupId",
                table: "DbDocuments",
                column: "ProjectsGroupId",
                principalTable: "ProjectsGroups",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Users_OwnerIdId",
                table: "Projects",
                column: "OwnerIdId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectsGroups_Users_OwnerIdId",
                table: "ProjectsGroups",
                column: "OwnerIdId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Projects_ProjectId",
                table: "Users",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_ProjectsGroups_ProjectsGroupId",
                table: "Users",
                column: "ProjectsGroupId",
                principalTable: "ProjectsGroups",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DbDocuments_Projects_ProjectId",
                table: "DbDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_DbDocuments_ProjectsGroups_ProjectsGroupId",
                table: "DbDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Users_OwnerIdId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectsGroups_Users_OwnerIdId",
                table: "ProjectsGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Projects_ProjectId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_ProjectsGroups_ProjectsGroupId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ProjectId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ProjectsGroupId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_ProjectsGroups_OwnerIdId",
                table: "ProjectsGroups");

            migrationBuilder.DropIndex(
                name: "IX_Projects_OwnerIdId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_DbDocuments_ProjectId",
                table: "DbDocuments");

            migrationBuilder.DropIndex(
                name: "IX_DbDocuments_ProjectsGroupId",
                table: "DbDocuments");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ProjectsGroupId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "OwnerIdId",
                table: "ProjectsGroups");

            migrationBuilder.DropColumn(
                name: "OwnerIdId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "DbDocuments");

            migrationBuilder.DropColumn(
                name: "ProjectsGroupId",
                table: "DbDocuments");

            migrationBuilder.AddColumn<int[]>(
                name: "AdminsId",
                table: "ProjectsGroups",
                type: "integer[]",
                nullable: true);

            migrationBuilder.AddColumn<int[]>(
                name: "OrderedProjectsId",
                table: "ProjectsGroups",
                type: "integer[]",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "ProjectsGroups",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int[]>(
                name: "AdminsId",
                table: "Projects",
                type: "integer[]",
                nullable: true);

            migrationBuilder.AddColumn<int[]>(
                name: "OrderedDocumentsId",
                table: "Projects",
                type: "integer[]",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "Projects",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
