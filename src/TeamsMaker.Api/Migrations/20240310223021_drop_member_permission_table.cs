using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamsMaker.Api.Migrations
{
    /// <inheritdoc />
    public partial class drop_member_permission_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MemberPermission",
                schema: "dbo");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "lookups",
                table: "Permission");

            migrationBuilder.DropColumn(
                name: "Group",
                schema: "lookups",
                table: "Permission");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "lookups",
                table: "Permission");

            migrationBuilder.AddColumn<Guid>(
                name: "CircleMemberId",
                schema: "lookups",
                table: "Permission",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "UpdateFiles",
                schema: "lookups",
                table: "Permission",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UpdateInfo",
                schema: "lookups",
                table: "Permission",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Permission_CircleMemberId",
                schema: "lookups",
                table: "Permission",
                column: "CircleMemberId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Permission_CircleMember_CircleMemberId",
                schema: "lookups",
                table: "Permission",
                column: "CircleMemberId",
                principalSchema: "dbo",
                principalTable: "CircleMember",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permission_CircleMember_CircleMemberId",
                schema: "lookups",
                table: "Permission");

            migrationBuilder.DropIndex(
                name: "IX_Permission_CircleMemberId",
                schema: "lookups",
                table: "Permission");

            migrationBuilder.DropColumn(
                name: "CircleMemberId",
                schema: "lookups",
                table: "Permission");

            migrationBuilder.DropColumn(
                name: "UpdateFiles",
                schema: "lookups",
                table: "Permission");

            migrationBuilder.DropColumn(
                name: "UpdateInfo",
                schema: "lookups",
                table: "Permission");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "lookups",
                table: "Permission",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Group",
                schema: "lookups",
                table: "Permission",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "lookups",
                table: "Permission",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "MemberPermission",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CircleMemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberPermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberPermission_CircleMember_CircleMemberId",
                        column: x => x.CircleMemberId,
                        principalSchema: "dbo",
                        principalTable: "CircleMember",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberPermission_Permission_PermissionId",
                        column: x => x.PermissionId,
                        principalSchema: "lookups",
                        principalTable: "Permission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MemberPermission_CircleMemberId",
                schema: "dbo",
                table: "MemberPermission",
                column: "CircleMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberPermission_PermissionId",
                schema: "dbo",
                table: "MemberPermission",
                column: "PermissionId");
        }
    }
}
