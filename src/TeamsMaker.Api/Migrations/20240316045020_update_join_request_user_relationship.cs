using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamsMaker.Api.Migrations
{
    /// <inheritdoc />
    public partial class update_join_request_user_relationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JoinRequest_AspNetUsers_UserId",
                schema: "dbo",
                table: "JoinRequest");

            migrationBuilder.RenameColumn(
                name: "UserId",
                schema: "dbo",
                table: "JoinRequest",
                newName: "StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_JoinRequest_UserId",
                schema: "dbo",
                table: "JoinRequest",
                newName: "IX_JoinRequest_StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_JoinRequest_Student_StudentId",
                schema: "dbo",
                table: "JoinRequest",
                column: "StudentId",
                principalSchema: "dbo",
                principalTable: "Student",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JoinRequest_Student_StudentId",
                schema: "dbo",
                table: "JoinRequest");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                schema: "dbo",
                table: "JoinRequest",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_JoinRequest_StudentId",
                schema: "dbo",
                table: "JoinRequest",
                newName: "IX_JoinRequest_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_JoinRequest_AspNetUsers_UserId",
                schema: "dbo",
                table: "JoinRequest",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
