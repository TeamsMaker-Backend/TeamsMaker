using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamsMaker.Api.Migrations
{
    /// <inheritdoc />
    public partial class refactor_proposal_model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "File_ContentType",
                schema: "dbo",
                table: "Proposal");

            migrationBuilder.DropColumn(
                name: "File_Name",
                schema: "dbo",
                table: "Proposal");

            migrationBuilder.AddColumn<string>(
                name: "Objectives",
                schema: "dbo",
                table: "Proposal",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Overview",
                schema: "dbo",
                table: "Proposal",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TeckStack",
                schema: "dbo",
                table: "Proposal",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                schema: "lookups",
                table: "ImportedStaff",
                keyColumn: "Id",
                keyValue: new Guid("3e9f4430-2927-41eb-a8a5-099248d1e6ba"),
                column: "Classification",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "lookups",
                table: "ImportedStaff",
                keyColumn: "Id",
                keyValue: new Guid("9266966b-fa8e-461a-bd61-0a1a15d5c234"),
                column: "Classification",
                value: 2);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Objectives",
                schema: "dbo",
                table: "Proposal");

            migrationBuilder.DropColumn(
                name: "Overview",
                schema: "dbo",
                table: "Proposal");

            migrationBuilder.DropColumn(
                name: "TeckStack",
                schema: "dbo",
                table: "Proposal");

            migrationBuilder.AddColumn<string>(
                name: "File_ContentType",
                schema: "dbo",
                table: "Proposal",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "File_Name",
                schema: "dbo",
                table: "Proposal",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "lookups",
                table: "ImportedStaff",
                keyColumn: "Id",
                keyValue: new Guid("3e9f4430-2927-41eb-a8a5-099248d1e6ba"),
                column: "Classification",
                value: null);

            migrationBuilder.UpdateData(
                schema: "lookups",
                table: "ImportedStaff",
                keyColumn: "Id",
                keyValue: new Guid("9266966b-fa8e-461a-bd61-0a1a15d5c234"),
                column: "Classification",
                value: null);
        }
    }
}
