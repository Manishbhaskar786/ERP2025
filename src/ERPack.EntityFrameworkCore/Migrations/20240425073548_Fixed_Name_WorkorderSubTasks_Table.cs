using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPack.Migrations
{
    /// <inheritdoc />
    public partial class Fixed_Name_WorkorderSubTasks_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WoWorkorderSubTasksrkorderTasks_Departments_DepartmentId",
                table: "WoWorkorderSubTasksrkorderTasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WoWorkorderSubTasksrkorderTasks",
                table: "WoWorkorderSubTasksrkorderTasks");

            migrationBuilder.RenameTable(
                name: "WoWorkorderSubTasksrkorderTasks",
                newName: "WorkorderSubTasks");

            migrationBuilder.RenameIndex(
                name: "IX_WoWorkorderSubTasksrkorderTasks_DepartmentId",
                table: "WorkorderSubTasks",
                newName: "IX_WorkorderSubTasks_DepartmentId");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Estimate",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkorderSubTasks",
                table: "WorkorderSubTasks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkorderSubTasks_Departments_DepartmentId",
                table: "WorkorderSubTasks",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkorderSubTasks_Departments_DepartmentId",
                table: "WorkorderSubTasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkorderSubTasks",
                table: "WorkorderSubTasks");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Estimate");

            migrationBuilder.RenameTable(
                name: "WorkorderSubTasks",
                newName: "WoWorkorderSubTasksrkorderTasks");

            migrationBuilder.RenameIndex(
                name: "IX_WorkorderSubTasks_DepartmentId",
                table: "WoWorkorderSubTasksrkorderTasks",
                newName: "IX_WoWorkorderSubTasksrkorderTasks_DepartmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WoWorkorderSubTasksrkorderTasks",
                table: "WoWorkorderSubTasksrkorderTasks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WoWorkorderSubTasksrkorderTasks_Departments_DepartmentId",
                table: "WoWorkorderSubTasksrkorderTasks",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
