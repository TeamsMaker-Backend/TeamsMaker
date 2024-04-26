using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamsMaker.Api.Migrations
{
    /// <inheritdoc />
    public partial class add_react_migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Likes",
                schema: "dbo",
                table: "Post",
                newName: "LikesNumber");

            migrationBuilder.CreateTable(
                name: "React",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CircleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PostId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_React", x => x.Id);
                    table.ForeignKey(
                        name: "FK_React_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_React_Circle_CircleId",
                        column: x => x.CircleId,
                        principalSchema: "dbo",
                        principalTable: "Circle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_React_Post_PostId",
                        column: x => x.PostId,
                        principalSchema: "dbo",
                        principalTable: "Post",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_React_CircleId",
                schema: "dbo",
                table: "React",
                column: "CircleId");

            migrationBuilder.CreateIndex(
                name: "IX_React_PostId",
                schema: "dbo",
                table: "React",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_React_UserId",
                schema: "dbo",
                table: "React",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "React",
                schema: "dbo");

            migrationBuilder.RenameColumn(
                name: "LikesNumber",
                schema: "dbo",
                table: "Post",
                newName: "Likes");
        }
    }
}
