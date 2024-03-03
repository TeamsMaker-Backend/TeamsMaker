using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamsMaker.Api.Migrations
{
    /// <inheritdoc />
    public partial class add_organization_logo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Logo",
                schema: "dbo",
                table: "Organization",
                newName: "Logo_Name");

            migrationBuilder.AddColumn<string>(
                name: "Logo_ContentType",
                schema: "dbo",
                table: "Organization",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Logo_ContentType",
                schema: "dbo",
                table: "Organization");

            migrationBuilder.RenameColumn(
                name: "Logo_Name",
                schema: "dbo",
                table: "Organization",
                newName: "Logo");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Organization",
                keyColumn: "Id",
                keyValue: 1,
                column: "Logo",
                value: null);
        }
    }
}
