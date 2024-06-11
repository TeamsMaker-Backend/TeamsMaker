using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamsMaker.Api.Migrations
{
    /// <inheritdoc />
    public partial class add_circle_member_flags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCosupervisor",
                schema: "dbo",
                table: "CircleMember",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSupervisor",
                schema: "dbo",
                table: "CircleMember",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCosupervisor",
                schema: "dbo",
                table: "CircleMember");

            migrationBuilder.DropColumn(
                name: "IsSupervisor",
                schema: "dbo",
                table: "CircleMember");
        }
    }
}
