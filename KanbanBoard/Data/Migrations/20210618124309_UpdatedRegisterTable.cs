using Microsoft.EntityFrameworkCore.Migrations;

namespace KanbanBoard.Data.Migrations
{
    public partial class UpdatedRegisterTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RegisterCodes",
                table: "RegisterCodes");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "RegisterCodes");

            migrationBuilder.RenameTable(
                name: "RegisterCodes",
                newName: "RegisterEmails");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "RegisterEmails",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RegisterEmails",
                table: "RegisterEmails",
                column: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RegisterEmails",
                table: "RegisterEmails");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "RegisterEmails");

            migrationBuilder.RenameTable(
                name: "RegisterEmails",
                newName: "RegisterCodes");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "RegisterCodes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RegisterCodes",
                table: "RegisterCodes",
                column: "ID");
        }
    }
}
