using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPack.Migrations
{
    /// <inheritdoc />
    public partial class Added_ItemTypeId_In_InventoryIssuedItem_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ItemTypeId",
                table: "InventoryIssuedItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryIssued_CreatorUserId",
                table: "InventoryIssued",
                column: "CreatorUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryIssued_AbpUsers_CreatorUserId",
                table: "InventoryIssued",
                column: "CreatorUserId",
                principalTable: "AbpUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryIssued_AbpUsers_CreatorUserId",
                table: "InventoryIssued");

            migrationBuilder.DropIndex(
                name: "IX_InventoryIssued_CreatorUserId",
                table: "InventoryIssued");

            migrationBuilder.DropColumn(
                name: "ItemTypeId",
                table: "InventoryIssuedItems");
        }
    }
}
