using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class _123 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecommenceByBoth_User_UserId",
                table: "RecommenceByBoth");

            migrationBuilder.DropForeignKey(
                name: "FK_RecommencePrice_User_UserId",
                table: "RecommencePrice");

            migrationBuilder.DropIndex(
                name: "IX_RecommencePrice_UserId",
                table: "RecommencePrice");

            migrationBuilder.DropIndex(
                name: "IX_RecommenceByBoth_UserId",
                table: "RecommenceByBoth");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_RecommencePrice_UserId",
                table: "RecommencePrice",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RecommenceByBoth_UserId",
                table: "RecommenceByBoth",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecommenceByBoth_User_UserId",
                table: "RecommenceByBoth",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecommencePrice_User_UserId",
                table: "RecommencePrice",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
