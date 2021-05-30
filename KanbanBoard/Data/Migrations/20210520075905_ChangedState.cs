using Microsoft.EntityFrameworkCore.Migrations;

namespace KanbanBoard.Data.Migrations
{
    public partial class ChangedState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issues_States_StateID",
                table: "Issues");

            migrationBuilder.DropTable(
                name: "States");

            migrationBuilder.RenameColumn(
                name: "StateID",
                table: "Issues",
                newName: "StateId");

            migrationBuilder.RenameIndex(
                name: "IX_Issues_StateID",
                table: "Issues",
                newName: "IX_Issues_StateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_KanbanFlag_StateId",
                table: "Issues",
                column: "StateId",
                principalTable: "KanbanFlag",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issues_KanbanFlag_StateId",
                table: "Issues");

            migrationBuilder.RenameColumn(
                name: "StateId",
                table: "Issues",
                newName: "StateID");

            migrationBuilder.RenameIndex(
                name: "IX_Issues_StateId",
                table: "Issues",
                newName: "IX_Issues_StateID");

            migrationBuilder.CreateTable(
                name: "States",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<int>(type: "int", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_States", x => x.ID);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_States_StateID",
                table: "Issues",
                column: "StateID",
                principalTable: "States",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
