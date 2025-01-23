using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPack.Migrations
{
    /// <inheritdoc />
    public partial class Added_Columns_In_InventoryRequest_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "InventoryRequests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsReqClose",
                table: "InventoryRequests",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remark",
                table: "InventoryRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ReqQty",
                table: "InventoryRequests",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<long>(
                name: "WorkorderTaskId",
                table: "InventoryRequests",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkorderTasks_WorkorderId",
                table: "WorkorderTasks",
                column: "WorkorderId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkorderTasks_Workorders_WorkorderId",
                table: "WorkorderTasks",
                column: "WorkorderId",
                principalTable: "Workorders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkorderTasks_Workorders_WorkorderId",
                table: "WorkorderTasks");

            migrationBuilder.DropIndex(
                name: "IX_WorkorderTasks_WorkorderId",
                table: "WorkorderTasks");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "InventoryRequests");

            migrationBuilder.DropColumn(
                name: "IsReqClose",
                table: "InventoryRequests");

            migrationBuilder.DropColumn(
                name: "Remark",
                table: "InventoryRequests");

            migrationBuilder.DropColumn(
                name: "ReqQty",
                table: "InventoryRequests");

            migrationBuilder.DropColumn(
                name: "WorkorderTaskId",
                table: "InventoryRequests");
        }
    }
}
