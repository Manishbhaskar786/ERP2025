using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPack.Migrations
{
    /// <inheritdoc />
    public partial class Updated_InventoryIssueItems_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InventoryIssueId",
                table: "InventoryIssuedItems");

            migrationBuilder.DropColumn(
                name: "IsManual",
                table: "InventoryIssuedItems");

            migrationBuilder.AddColumn<bool>(
                name: "IsManual",
                table: "InventoryIssued",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsManual",
                table: "InventoryIssued");

            migrationBuilder.AddColumn<long>(
                name: "InventoryIssueId",
                table: "InventoryIssuedItems",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "IsManual",
                table: "InventoryIssuedItems",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
