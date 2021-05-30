using Microsoft.EntityFrameworkCore.Migrations;

namespace KanbanBoard.Data.Migrations
{
    public partial class Update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedUser",
                table: "Issues");

            migrationBuilder.AddColumn<string>(
                name: "AssignedUserId",
                table: "Issues",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Issues_AssignedUserId",
                table: "Issues",
                column: "AssignedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_AspNetUsers_AssignedUserId",
                table: "Issues",
                column: "AssignedUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issues_AspNetUsers_AssignedUserId",
                table: "Issues");

            migrationBuilder.DropIndex(
                name: "IX_Issues_AssignedUserId",
                table: "Issues");

            migrationBuilder.DropColumn(
                name: "AssignedUserId",
                table: "Issues");

            migrationBuilder.AddColumn<string>(
                name: "AssignedUser",
                table: "Issues",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
