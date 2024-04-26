using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamsMaker.Api.Migrations
{
    /// <inheritdoc />
    public partial class update_reacts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_React_Post_PostId",
                schema: "dbo",
                table: "React");

            migrationBuilder.AlterColumn<Guid>(
                name: "PostId",
                schema: "dbo",
                table: "React",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_React_Post_PostId",
                schema: "dbo",
                table: "React",
                column: "PostId",
                principalSchema: "dbo",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_React_Post_PostId",
                schema: "dbo",
                table: "React");

            migrationBuilder.AlterColumn<Guid>(
                name: "PostId",
                schema: "dbo",
                table: "React",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_React_Post_PostId",
                schema: "dbo",
                table: "React",
                column: "PostId",
                principalSchema: "dbo",
                principalTable: "Post",
                principalColumn: "Id");
        }
    }
}
