using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamsMaker.Api.Migrations
{
    /// <inheritdoc />
    public partial class add_joinRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JoinRequest",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Sender = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true),
                    IsAccepted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CircleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JoinRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JoinRequest_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_JoinRequest_Circle_CircleId",
                        column: x => x.CircleId,
                        principalSchema: "dbo",
                        principalTable: "Circle",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_JoinRequest_CircleId",
                schema: "dbo",
                table: "JoinRequest",
                column: "CircleId");

            migrationBuilder.CreateIndex(
                name: "IX_JoinRequest_UserId",
                schema: "dbo",
                table: "JoinRequest",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JoinRequest",
                schema: "dbo");
        }
    }
}
