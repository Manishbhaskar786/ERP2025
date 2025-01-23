using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPack.Migrations
{
    /// <inheritdoc />
    public partial class Update_MaterialId_In_MaterialInventory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "MaterialId",
                table: "MaterialInventory",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseIndents_RequestedBy",
                table: "PurchaseIndents",
                column: "RequestedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseIndents_AbpUsers_RequestedBy",
                table: "PurchaseIndents",
                column: "RequestedBy",
                principalTable: "AbpUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseIndents_AbpUsers_RequestedBy",
                table: "PurchaseIndents");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseIndents_RequestedBy",
                table: "PurchaseIndents");

            migrationBuilder.AlterColumn<long>(
                name: "MaterialId",
                table: "MaterialInventory",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
