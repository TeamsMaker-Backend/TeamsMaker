using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamsMaker.Api.Migrations
{
    /// <inheritdoc />
    public partial class add_proposal_entity_structure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Classification",
                schema: "lookups",
                table: "ImportedStaff",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Proposal",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    File_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    File_ContentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CircleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proposal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Proposal_Circle_CircleId",
                        column: x => x.CircleId,
                        principalSchema: "dbo",
                        principalTable: "Circle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApprovalRequest",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsAccepted = table.Column<bool>(type: "bit", nullable: false),
                    ProposalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StaffId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApprovalRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApprovalRequest_Proposal_ProposalId",
                        column: x => x.ProposalId,
                        principalSchema: "dbo",
                        principalTable: "Proposal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApprovalRequest_Staff_StaffId",
                        column: x => x.StaffId,
                        principalSchema: "dbo",
                        principalTable: "Staff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                schema: "lookups",
                table: "ImportedStaff",
                keyColumn: "Id",
                keyValue: new Guid("3e9f4430-2927-41eb-a8a5-099248d1e6ba"),
                column: "Classification",
                value: null);

            migrationBuilder.UpdateData(
                schema: "lookups",
                table: "ImportedStaff",
                keyColumn: "Id",
                keyValue: new Guid("9266966b-fa8e-461a-bd61-0a1a15d5c234"),
                column: "Classification",
                value: null);

            migrationBuilder.UpdateData(
                schema: "lookups",
                table: "ImportedStudent",
                keyColumn: "Id",
                keyValue: new Guid("5cba5edb-d6f0-4dee-85df-7f23fcbf86d3"),
                column: "CollegeId",
                value: "01HVK5SHDAA4NG1AJHJDY3MFDW");

            migrationBuilder.UpdateData(
                schema: "lookups",
                table: "ImportedStudent",
                keyColumn: "Id",
                keyValue: new Guid("86281c15-127d-4c91-9dff-dcc24164f79b"),
                column: "CollegeId",
                value: "01HVK5SHD9JFNX853XW06NAD4W");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalRequest_ProposalId",
                schema: "dbo",
                table: "ApprovalRequest",
                column: "ProposalId");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalRequest_StaffId",
                schema: "dbo",
                table: "ApprovalRequest",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_Proposal_CircleId",
                schema: "dbo",
                table: "Proposal",
                column: "CircleId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApprovalRequest",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Proposal",
                schema: "dbo");

            migrationBuilder.DropColumn(
                name: "Classification",
                schema: "lookups",
                table: "ImportedStaff");

            migrationBuilder.UpdateData(
                schema: "lookups",
                table: "ImportedStudent",
                keyColumn: "Id",
                keyValue: new Guid("5cba5edb-d6f0-4dee-85df-7f23fcbf86d3"),
                column: "CollegeId",
                value: "College-123");

            migrationBuilder.UpdateData(
                schema: "lookups",
                table: "ImportedStudent",
                keyColumn: "Id",
                keyValue: new Guid("86281c15-127d-4c91-9dff-dcc24164f79b"),
                column: "CollegeId",
                value: "College-456");
        }
    }
}
