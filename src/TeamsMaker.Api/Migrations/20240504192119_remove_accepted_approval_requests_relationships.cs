using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamsMaker.Api.Migrations
{
    /// <inheritdoc />
    public partial class remove_accepted_approval_requests_relationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "IsReseted",
                schema: "dbo",
                table: "ApprovalRequest");

            migrationBuilder.DropColumn(
                name: "SupervisorId",
                schema: "dbo",
                table: "ApprovalRequest");

            migrationBuilder.AddColumn<bool>(
                name: "IsReseted",
                schema: "dbo",
                table: "Proposal",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReseted",
                schema: "dbo",
                table: "Proposal");

            migrationBuilder.AddColumn<bool>(
                name: "IsReseted",
                schema: "dbo",
                table: "ApprovalRequest",
                type: "bit",
                nullable: false,
                defaultValue: false);

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
    }
}
