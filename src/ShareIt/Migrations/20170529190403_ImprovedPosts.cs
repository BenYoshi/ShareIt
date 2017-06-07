using Microsoft.EntityFrameworkCore.Migrations;

namespace ShareIt.Migrations
{
    public partial class ImprovedPosts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Posts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PosterId",
                table: "Posts",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_PosterId",
                table: "Posts",
                column: "PosterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_AspNetUsers_PosterId",
                table: "Posts",
                column: "PosterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AspNetUsers_PosterId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_PosterId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "PosterId",
                table: "Posts");
        }
    }
}
