using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPack.Migrations
{
    /// <inheritdoc />
    public partial class Update_Purchase_Tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromStoreId",
                table: "InventoryIssued");

            migrationBuilder.DropColumn(
                name: "InventoryRequestId",
                table: "InventoryIssued");

            migrationBuilder.DropColumn(
                name: "IsManual",
                table: "InventoryIssued");

            migrationBuilder.DropColumn(
                name: "IssuedDepartmentId",
                table: "InventoryIssued");

            migrationBuilder.DropColumn(
                name: "MaterialId",
                table: "InventoryIssued");

            migrationBuilder.DropColumn(
                name: "PersonIssuedId",
                table: "InventoryIssued");

            migrationBuilder.DropColumn(
                name: "QtyTransferred",
                table: "InventoryIssued");

            migrationBuilder.DropColumn(
                name: "ToStoreId",
                table: "InventoryIssued");

            migrationBuilder.RenameColumn(
                name: "Qty",
                table: "PurchaseIndents",
                newName: "Quantity");

            migrationBuilder.RenameColumn(
                name: "InventoryIssueId",
                table: "InventoryIssued",
                newName: "IssueCode");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RequestedDate",
                table: "PurchaseIndents",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ItemCode",
                table: "PurchaseIndents",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "PurchaseIndents",
                newName: "Qty");

            migrationBuilder.RenameColumn(
                name: "IssueCode",
                table: "InventoryIssued",
                newName: "InventoryIssueId");

            migrationBuilder.AlterColumn<string>(
                name: "RequestedDate",
                table: "PurchaseIndents",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ItemCode",
                table: "PurchaseIndents",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FromStoreId",
                table: "InventoryIssued",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "InventoryRequestId",
                table: "InventoryIssued",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsManual",
                table: "InventoryIssued",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "IssuedDepartmentId",
                table: "InventoryIssued",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaterialId",
                table: "InventoryIssued",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "PersonIssuedId",
                table: "InventoryIssued",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<decimal>(
                name: "QtyTransferred",
                table: "InventoryIssued",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "ToStoreId",
                table: "InventoryIssued",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
