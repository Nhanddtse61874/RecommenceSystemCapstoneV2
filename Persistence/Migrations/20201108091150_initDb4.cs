using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class initDb4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecommenceByBoth_User_UserId",
                table: "RecommenceByBoth");

            migrationBuilder.DropForeignKey(
                name: "FK_RecommencePrice_User_UserId",
                table: "RecommencePrice");

            migrationBuilder.DropColumn(
                name: "UserCode",
                table: "RecommencePrice");

            migrationBuilder.DropColumn(
                name: "UserCode",
                table: "RecommenceByBoth");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "RecommencePrice",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "RecommenceByBoth",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecommenceByBoth_User_UserId",
                table: "RecommenceByBoth");

            migrationBuilder.DropForeignKey(
                name: "FK_RecommencePrice_User_UserId",
                table: "RecommencePrice");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "RecommencePrice",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "UserCode",
                table: "RecommencePrice",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "RecommenceByBoth",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "UserCode",
                table: "RecommenceByBoth",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_RecommenceByBoth_User_UserId",
                table: "RecommenceByBoth",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RecommencePrice_User_UserId",
                table: "RecommencePrice",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
