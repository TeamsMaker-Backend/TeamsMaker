using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamsMaker.Api.Migrations
{
    /// <inheritdoc />
    public partial class handle_user_childs_relationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Staff_AspNetUsers_Id",
                schema: "dbo",
                table: "Staff");

            migrationBuilder.DropForeignKey(
                name: "FK_Student_AspNetUsers_Id",
                schema: "dbo",
                table: "Student");

            migrationBuilder.AddForeignKey(
                name: "FK_Staff_AspNetUsers_Id",
                schema: "dbo",
                table: "Staff",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Student_AspNetUsers_Id",
                schema: "dbo",
                table: "Student",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Staff_AspNetUsers_Id",
                schema: "dbo",
                table: "Staff");

            migrationBuilder.DropForeignKey(
                name: "FK_Student_AspNetUsers_Id",
                schema: "dbo",
                table: "Student");

            migrationBuilder.AddForeignKey(
                name: "FK_Staff_AspNetUsers_Id",
                schema: "dbo",
                table: "Staff",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Student_AspNetUsers_Id",
                schema: "dbo",
                table: "Student",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
