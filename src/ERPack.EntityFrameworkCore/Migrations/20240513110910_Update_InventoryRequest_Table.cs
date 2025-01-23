using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPack.Migrations
{
    /// <inheritdoc />
    public partial class Update_InventoryRequest_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "RequestFromUserId",
                table: "InventoryRequests",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryRequests_MaterialId",
                table: "InventoryRequests",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryRequests_RequestFromUserId",
                table: "InventoryRequests",
                column: "RequestFromUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryRequests_AbpUsers_RequestFromUserId",
                table: "InventoryRequests",
                column: "RequestFromUserId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryRequests_Materials_MaterialId",
                table: "InventoryRequests",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryRequests_AbpUsers_RequestFromUserId",
                table: "InventoryRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryRequests_Materials_MaterialId",
                table: "InventoryRequests");

            migrationBuilder.DropIndex(
                name: "IX_InventoryRequests_MaterialId",
                table: "InventoryRequests");

            migrationBuilder.DropIndex(
                name: "IX_InventoryRequests_RequestFromUserId",
                table: "InventoryRequests");

            migrationBuilder.AlterColumn<int>(
                name: "RequestFromUserId",
                table: "InventoryRequests",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
