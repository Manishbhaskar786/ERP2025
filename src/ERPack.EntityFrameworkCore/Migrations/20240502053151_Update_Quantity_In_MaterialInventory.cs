using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPack.Migrations
{
    /// <inheritdoc />
    public partial class Update_Quantity_In_MaterialInventory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "MaterialInventory",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryIssuedItems_CreatorUserId",
                table: "InventoryIssuedItems",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryIssuedItems_FromStoreId",
                table: "InventoryIssuedItems",
                column: "FromStoreId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryIssuedItems_IssuedDepartmentId",
                table: "InventoryIssuedItems",
                column: "IssuedDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryIssuedItems_ItemTypeId",
                table: "InventoryIssuedItems",
                column: "ItemTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryIssuedItems_MaterialId",
                table: "InventoryIssuedItems",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryIssuedItems_ToStoreId",
                table: "InventoryIssuedItems",
                column: "ToStoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryIssuedItems_AbpUsers_CreatorUserId",
                table: "InventoryIssuedItems",
                column: "CreatorUserId",
                principalTable: "AbpUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryIssuedItems_Departments_IssuedDepartmentId",
                table: "InventoryIssuedItems",
                column: "IssuedDepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryIssuedItems_ItemTypes_ItemTypeId",
                table: "InventoryIssuedItems",
                column: "ItemTypeId",
                principalTable: "ItemTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryIssuedItems_Materials_MaterialId",
                table: "InventoryIssuedItems",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryIssuedItems_Stores_FromStoreId",
                table: "InventoryIssuedItems",
                column: "FromStoreId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryIssuedItems_Stores_ToStoreId",
                table: "InventoryIssuedItems",
                column: "ToStoreId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryIssuedItems_AbpUsers_CreatorUserId",
                table: "InventoryIssuedItems");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryIssuedItems_Departments_IssuedDepartmentId",
                table: "InventoryIssuedItems");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryIssuedItems_ItemTypes_ItemTypeId",
                table: "InventoryIssuedItems");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryIssuedItems_Materials_MaterialId",
                table: "InventoryIssuedItems");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryIssuedItems_Stores_FromStoreId",
                table: "InventoryIssuedItems");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryIssuedItems_Stores_ToStoreId",
                table: "InventoryIssuedItems");

            migrationBuilder.DropIndex(
                name: "IX_InventoryIssuedItems_CreatorUserId",
                table: "InventoryIssuedItems");

            migrationBuilder.DropIndex(
                name: "IX_InventoryIssuedItems_FromStoreId",
                table: "InventoryIssuedItems");

            migrationBuilder.DropIndex(
                name: "IX_InventoryIssuedItems_IssuedDepartmentId",
                table: "InventoryIssuedItems");

            migrationBuilder.DropIndex(
                name: "IX_InventoryIssuedItems_ItemTypeId",
                table: "InventoryIssuedItems");

            migrationBuilder.DropIndex(
                name: "IX_InventoryIssuedItems_MaterialId",
                table: "InventoryIssuedItems");

            migrationBuilder.DropIndex(
                name: "IX_InventoryIssuedItems_ToStoreId",
                table: "InventoryIssuedItems");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "MaterialInventory",
                type: "int",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2);
        }
    }
}
