using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamsMaker.Api.Migrations
{
    /// <inheritdoc />
    public partial class add_organization_id_imported_user : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "GPA",
                schema: "lookups",
                table: "ImportedUser",
                type: "real",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Department",
                schema: "lookups",
                table: "ImportedUser",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "OrganizationId",
                schema: "lookups",
                table: "ImportedUser",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrganizationId",
                schema: "lookups",
                table: "ImportedUser");

            migrationBuilder.AlterColumn<string>(
                name: "GPA",
                schema: "lookups",
                table: "ImportedUser",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Department",
                schema: "lookups",
                table: "ImportedUser",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
