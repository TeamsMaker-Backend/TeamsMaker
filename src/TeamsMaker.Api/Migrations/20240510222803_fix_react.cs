using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamsMaker.Api.Migrations
{
    /// <inheritdoc />
    public partial class fix_react : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_React_AspNetUsers_UserId",
                schema: "dbo",
                table: "React");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                schema: "dbo",
                table: "React",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_React_AspNetUsers_UserId",
                schema: "dbo",
                table: "React",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_React_AspNetUsers_UserId",
                schema: "dbo",
                table: "React");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                schema: "dbo",
                table: "React",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_React_AspNetUsers_UserId",
                schema: "dbo",
                table: "React",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
