using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamsMaker.Api.Migrations
{
    /// <inheritdoc />
    public partial class fix_column_typo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_Comment_ParentPostId",
                schema: "dbo",
                table: "Post");

            migrationBuilder.RenameColumn(
                name: "TeckStack",
                schema: "dbo",
                table: "Proposal",
                newName: "TechStack");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_Comment_ParentPostId",
                schema: "dbo",
                table: "Post",
                column: "ParentPostId",
                principalSchema: "dbo",
                principalTable: "Post",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_Comment_ParentPostId",
                schema: "dbo",
                table: "Post");

            migrationBuilder.RenameColumn(
                name: "TechStack",
                schema: "dbo",
                table: "Proposal",
                newName: "TeckStack");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_Comment_ParentPostId",
                schema: "dbo",
                table: "Post",
                column: "ParentPostId",
                principalSchema: "dbo",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
