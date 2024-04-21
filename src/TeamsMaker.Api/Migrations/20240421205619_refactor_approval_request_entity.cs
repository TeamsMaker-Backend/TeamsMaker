using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamsMaker.Api.Migrations
{
    /// <inheritdoc />
    public partial class refactor_approval_request_entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Destination",
                schema: "dbo",
                table: "ApprovalRequest",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Position",
                schema: "dbo",
                table: "ApprovalRequest",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SupervisorId",
                schema: "dbo",
                table: "ApprovalRequest",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalRequest_SupervisorId",
                schema: "dbo",
                table: "ApprovalRequest",
                column: "SupervisorId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovalRequest_Staff_SupervisorId",
                schema: "dbo",
                table: "ApprovalRequest",
                column: "SupervisorId",
                principalSchema: "dbo",
                principalTable: "Staff",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApprovalRequest_Staff_SupervisorId",
                schema: "dbo",
                table: "ApprovalRequest");

            migrationBuilder.DropIndex(
                name: "IX_ApprovalRequest_SupervisorId",
                schema: "dbo",
                table: "ApprovalRequest");

            migrationBuilder.DropColumn(
                name: "Destination",
                schema: "dbo",
                table: "ApprovalRequest");

            migrationBuilder.DropColumn(
                name: "Position",
                schema: "dbo",
                table: "ApprovalRequest");

            migrationBuilder.DropColumn(
                name: "SupervisorId",
                schema: "dbo",
                table: "ApprovalRequest");
        }
    }
}
