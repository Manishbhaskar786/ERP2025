using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPack.Migrations
{
    /// <inheritdoc />
    public partial class Update_PurchaseReceiveId_In_PurchaseReceiveItem_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PurchaseRecieveId",
                table: "PurchaseReceiveItems",
                newName: "PurchaseReceiveId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PurchaseReceiveId",
                table: "PurchaseReceiveItems",
                newName: "PurchaseRecieveId");
        }
    }
}
