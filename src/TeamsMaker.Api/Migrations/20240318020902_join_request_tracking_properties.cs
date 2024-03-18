using System;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamsMaker.Api.Migrations
{
    /// <inheritdoc />
    public partial class join_request_tracking_properties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "dbo",
                table: "JoinRequest",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                schema: "dbo",
                table: "JoinRequest",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationDate",
                schema: "dbo",
                table: "JoinRequest",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                schema: "dbo",
                table: "JoinRequest",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "dbo",
                table: "JoinRequest");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                schema: "dbo",
                table: "JoinRequest");

            migrationBuilder.DropColumn(
                name: "LastModificationDate",
                schema: "dbo",
                table: "JoinRequest");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                schema: "dbo",
                table: "JoinRequest");
        }
    }
}
