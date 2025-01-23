using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPack.Migrations
{
    /// <inheritdoc />
    public partial class Removed_extra_columns_PurchaseReceives_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentUrl",
                table: "PurchaseReceives");

            migrationBuilder.DropColumn(
                name: "POCode",
                table: "PurchaseReceives");

            migrationBuilder.AddColumn<DateTime>(
                name: "PurchaseReceiveDate",
                table: "PurchaseReceives",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseReceives_PurchaseOrderId",
                table: "PurchaseReceives",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseReceives_VendorId",
                table: "PurchaseReceives",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_VendorId",
                table: "PurchaseOrders",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderItems_PurchaseOrderId",
                table: "PurchaseOrderItems",
                column: "PurchaseOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderItems_PurchaseOrders_PurchaseOrderId",
                table: "PurchaseOrderItems",
                column: "PurchaseOrderId",
                principalTable: "PurchaseOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_Vendors_VendorId",
                table: "PurchaseOrders",
                column: "VendorId",
                principalTable: "Vendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseReceives_PurchaseOrders_PurchaseOrderId",
                table: "PurchaseReceives",
                column: "PurchaseOrderId",
                principalTable: "PurchaseOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseReceives_Vendors_VendorId",
                table: "PurchaseReceives",
                column: "VendorId",
                principalTable: "Vendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrderItems_PurchaseOrders_PurchaseOrderId",
                table: "PurchaseOrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_Vendors_VendorId",
                table: "PurchaseOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseReceives_PurchaseOrders_PurchaseOrderId",
                table: "PurchaseReceives");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseReceives_Vendors_VendorId",
                table: "PurchaseReceives");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseReceives_PurchaseOrderId",
                table: "PurchaseReceives");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseReceives_VendorId",
                table: "PurchaseReceives");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrders_VendorId",
                table: "PurchaseOrders");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrderItems_PurchaseOrderId",
                table: "PurchaseOrderItems");

            migrationBuilder.DropColumn(
                name: "PurchaseReceiveDate",
                table: "PurchaseReceives");

            migrationBuilder.AddColumn<string>(
                name: "DocumentUrl",
                table: "PurchaseReceives",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "POCode",
                table: "PurchaseReceives",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
