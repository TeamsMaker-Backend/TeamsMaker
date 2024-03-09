using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamsMaker.Api.Migrations
{
    /// <inheritdoc />
    public partial class update_circle_entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Skill_Project_ProjectId",
                schema: "dbo",
                table: "Skill");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                schema: "dbo",
                table: "Skill",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<Guid>(
                name: "CircleId",
                schema: "dbo",
                table: "Skill",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "lookups",
                table: "Permission",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Group",
                schema: "lookups",
                table: "Permission",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CircleId",
                schema: "dbo",
                table: "Link",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Badge",
                schema: "dbo",
                table: "CircleMember",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "dbo",
                table: "Circle",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSummaryPublic",
                schema: "dbo",
                table: "Circle",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrganizationId",
                schema: "dbo",
                table: "Circle",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "Rate",
                schema: "dbo",
                table: "Circle",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Summary",
                schema: "dbo",
                table: "Circle",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Skill_CircleId",
                schema: "dbo",
                table: "Skill",
                column: "CircleId");

            migrationBuilder.CreateIndex(
                name: "IX_Link_CircleId",
                schema: "dbo",
                table: "Link",
                column: "CircleId");

            migrationBuilder.CreateIndex(
                name: "IX_Circle_OrganizationId",
                schema: "dbo",
                table: "Circle",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Circle_Organization_OrganizationId",
                schema: "dbo",
                table: "Circle",
                column: "OrganizationId",
                principalSchema: "dbo",
                principalTable: "Organization",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Link_Circle_CircleId",
                schema: "dbo",
                table: "Link",
                column: "CircleId",
                principalSchema: "dbo",
                principalTable: "Circle",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Skill_Circle_CircleId",
                schema: "dbo",
                table: "Skill",
                column: "CircleId",
                principalSchema: "dbo",
                principalTable: "Circle",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Skill_Project_ProjectId",
                schema: "dbo",
                table: "Skill",
                column: "ProjectId",
                principalSchema: "dbo",
                principalTable: "Project",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Circle_Organization_OrganizationId",
                schema: "dbo",
                table: "Circle");

            migrationBuilder.DropForeignKey(
                name: "FK_Link_Circle_CircleId",
                schema: "dbo",
                table: "Link");

            migrationBuilder.DropForeignKey(
                name: "FK_Skill_Circle_CircleId",
                schema: "dbo",
                table: "Skill");

            migrationBuilder.DropForeignKey(
                name: "FK_Skill_Project_ProjectId",
                schema: "dbo",
                table: "Skill");

            migrationBuilder.DropIndex(
                name: "IX_Skill_CircleId",
                schema: "dbo",
                table: "Skill");

            migrationBuilder.DropIndex(
                name: "IX_Link_CircleId",
                schema: "dbo",
                table: "Link");

            migrationBuilder.DropIndex(
                name: "IX_Circle_OrganizationId",
                schema: "dbo",
                table: "Circle");

            migrationBuilder.DropColumn(
                name: "CircleId",
                schema: "dbo",
                table: "Skill");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "lookups",
                table: "Permission");

            migrationBuilder.DropColumn(
                name: "Group",
                schema: "lookups",
                table: "Permission");

            migrationBuilder.DropColumn(
                name: "CircleId",
                schema: "dbo",
                table: "Link");

            migrationBuilder.DropColumn(
                name: "Badge",
                schema: "dbo",
                table: "CircleMember");

            migrationBuilder.DropColumn(
                name: "IsSummaryPublic",
                schema: "dbo",
                table: "Circle");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                schema: "dbo",
                table: "Circle");

            migrationBuilder.DropColumn(
                name: "Rate",
                schema: "dbo",
                table: "Circle");

            migrationBuilder.DropColumn(
                name: "Summary",
                schema: "dbo",
                table: "Circle");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                schema: "dbo",
                table: "Skill",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "dbo",
                table: "Circle",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Skill_Project_ProjectId",
                schema: "dbo",
                table: "Skill",
                column: "ProjectId",
                principalSchema: "dbo",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
