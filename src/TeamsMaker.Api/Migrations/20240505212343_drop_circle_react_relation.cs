using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamsMaker.Api.Migrations
{
    /// <inheritdoc />
    public partial class drop_circle_react_relation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_React_Circle_CircleId",
                schema: "dbo",
                table: "React");

            migrationBuilder.DropIndex(
                name: "IX_React_CircleId",
                schema: "dbo",
                table: "React");

            migrationBuilder.DropColumn(
                name: "CircleId",
                schema: "dbo",
                table: "React");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CircleId",
                schema: "dbo",
                table: "React",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_React_CircleId",
                schema: "dbo",
                table: "React",
                column: "CircleId");

            migrationBuilder.AddForeignKey(
                name: "FK_React_Circle_CircleId",
                schema: "dbo",
                table: "React",
                column: "CircleId",
                principalSchema: "dbo",
                principalTable: "Circle",
                principalColumn: "Id");
        }
    }
}
