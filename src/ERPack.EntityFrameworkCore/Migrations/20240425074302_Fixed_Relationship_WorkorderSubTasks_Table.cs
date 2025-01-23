using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPack.Migrations
{
    /// <inheritdoc />
    public partial class Fixed_Relationship_WorkorderSubTasks_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkOrderTaskId",
                table: "WorkorderSubTasks");

            migrationBuilder.RenameColumn(
                name: "WorkOrderId",
                table: "WorkorderTasks",
                newName: "WorkorderId");

            migrationBuilder.RenameColumn(
                name: "WorkOrderSubTaskId",
                table: "WorkorderSubTasks",
                newName: "WorkorderSubTaskId");

            migrationBuilder.AddColumn<long>(
                name: "WorkorderSubTaskId",
                table: "WorkorderTasks",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkorderSubTaskId",
                table: "WorkorderTasks");

            migrationBuilder.RenameColumn(
                name: "WorkorderId",
                table: "WorkorderTasks",
                newName: "WorkOrderId");

            migrationBuilder.RenameColumn(
                name: "WorkorderSubTaskId",
                table: "WorkorderSubTasks",
                newName: "WorkOrderSubTaskId");

            migrationBuilder.AddColumn<long>(
                name: "WorkOrderTaskId",
                table: "WorkorderSubTasks",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
