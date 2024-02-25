using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamsMaker.Api.Migrations
{
    /// <inheritdoc />
    public partial class update_file_types : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CV",
                schema: "dbo",
                table: "Student",
                newName: "CV_Name");

            migrationBuilder.RenameColumn(
                name: "Header",
                table: "AspNetUsers",
                newName: "Header_Name");

            migrationBuilder.RenameColumn(
                name: "Avatar",
                table: "AspNetUsers",
                newName: "Header_ContentType");

            migrationBuilder.AddColumn<string>(
                name: "CV_ContentType",
                schema: "dbo",
                table: "Student",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Avatar_ContentType",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Avatar_Name",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CV_ContentType",
                schema: "dbo",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "Avatar_ContentType",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Avatar_Name",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "CV_Name",
                schema: "dbo",
                table: "Student",
                newName: "CV");

            migrationBuilder.RenameColumn(
                name: "Header_Name",
                table: "AspNetUsers",
                newName: "Header");

            migrationBuilder.RenameColumn(
                name: "Header_ContentType",
                table: "AspNetUsers",
                newName: "Avatar");
        }
    }
}
