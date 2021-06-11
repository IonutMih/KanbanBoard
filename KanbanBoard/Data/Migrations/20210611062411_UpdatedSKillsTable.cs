using Microsoft.EntityFrameworkCore.Migrations;

namespace KanbanBoard.Data.Migrations
{
    public partial class UpdatedSKillsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Skills_UserSkills_UserSkillID",
                table: "Skills");

            migrationBuilder.DropIndex(
                name: "IX_Skills_UserSkillID",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "UserSkillID",
                table: "Skills");

            migrationBuilder.AddColumn<int>(
                name: "skillID",
                table: "UserSkills",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSkills_skillID",
                table: "UserSkills",
                column: "skillID");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSkills_Skills_skillID",
                table: "UserSkills",
                column: "skillID",
                principalTable: "Skills",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSkills_Skills_skillID",
                table: "UserSkills");

            migrationBuilder.DropIndex(
                name: "IX_UserSkills_skillID",
                table: "UserSkills");

            migrationBuilder.DropColumn(
                name: "skillID",
                table: "UserSkills");

            migrationBuilder.AddColumn<int>(
                name: "UserSkillID",
                table: "Skills",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Skills_UserSkillID",
                table: "Skills",
                column: "UserSkillID");

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_UserSkills_UserSkillID",
                table: "Skills",
                column: "UserSkillID",
                principalTable: "UserSkills",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
