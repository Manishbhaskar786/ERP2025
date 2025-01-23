using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPack.Migrations
{
    /// <inheritdoc />
    public partial class Remove_PurchaseIndent_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchaseIndents");

            migrationBuilder.AlterColumn<int>(
                name: "PurchaseReceiveId",
                table: "PurchaseReceiveItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "PurchaseOrderItemId",
                table: "PurchaseReceiveItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseReceiveItems_PurchaseOrderItemId",
                table: "PurchaseReceiveItems",
                column: "PurchaseOrderItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseReceiveItems_StoreId",
                table: "PurchaseReceiveItems",
                column: "StoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseReceiveItems_PurchaseOrderItems_PurchaseOrderItemId",
                table: "PurchaseReceiveItems",
                column: "PurchaseOrderItemId",
                principalTable: "PurchaseOrderItems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseReceiveItems_Stores_StoreId",
                table: "PurchaseReceiveItems",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseReceiveItems_PurchaseOrderItems_PurchaseOrderItemId",
                table: "PurchaseReceiveItems");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseReceiveItems_Stores_StoreId",
                table: "PurchaseReceiveItems");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseReceiveItems_PurchaseOrderItemId",
                table: "PurchaseReceiveItems");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseReceiveItems_StoreId",
                table: "PurchaseReceiveItems");

            migrationBuilder.AlterColumn<int>(
                name: "PurchaseReceiveId",
                table: "PurchaseReceiveItems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PurchaseOrderItemId",
                table: "PurchaseReceiveItems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "PurchaseIndents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ItemCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ItemDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RequiredBy = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    TypeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseIndents", x => x.Id);
                });
        }
    }
}
