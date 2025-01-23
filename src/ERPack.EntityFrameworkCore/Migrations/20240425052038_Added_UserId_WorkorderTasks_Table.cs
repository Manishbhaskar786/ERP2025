using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPack.Migrations
{
    /// <inheritdoc />
    public partial class Added_UserId_WorkorderTasks_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "WorkorderTasks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkorderTasks_MaterialId",
                table: "WorkorderTasks",
                column: "MaterialId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkorderTasks_Materials_MaterialId",
                table: "WorkorderTasks",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkorderTasks_Materials_MaterialId",
                table: "WorkorderTasks");

            migrationBuilder.DropIndex(
                name: "IX_WorkorderTasks_MaterialId",
                table: "WorkorderTasks");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "WorkorderTasks");
        }
    }
}
