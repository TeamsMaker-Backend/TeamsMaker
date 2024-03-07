using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamsMaker.Api.Migrations
{
    /// <inheritdoc />
    public partial class circle_related_enitites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                schema: "dbo",
                table: "Experience",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsHead",
                schema: "dbo",
                table: "DepartmentStaff",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Circle",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Avatar_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Avatar_ContentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Header_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Header_ContentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Circle", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permission",
                schema: "lookups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CircleMember",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CircleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsOwner = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CircleMember", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CircleMember_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CircleMember_Circle_CircleId",
                        column: x => x.CircleId,
                        principalSchema: "dbo",
                        principalTable: "Circle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MemberPermission",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: false),
                    CircleMemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                name: "IX_CircleMember_CircleId",
                schema: "dbo",
                table: "CircleMember",
                column: "CircleId");

            migrationBuilder.CreateIndex(
                name: "IX_CircleMember_UserId",
                schema: "dbo",
                table: "CircleMember",
                column: "UserId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MemberPermission",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "CircleMember",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Permission",
                schema: "lookups");

            migrationBuilder.DropTable(
                name: "Circle",
                schema: "dbo");

            migrationBuilder.DropColumn(
                name: "Title",
                schema: "dbo",
                table: "Experience");

            migrationBuilder.DropColumn(
                name: "IsHead",
                schema: "dbo",
                table: "DepartmentStaff");
        }
    }
}
