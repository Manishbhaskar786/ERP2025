using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPack.Migrations
{
    /// <inheritdoc />
    public partial class Updated_Material_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuyingUnit",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "MaterialUserName",
                table: "Materials");

            migrationBuilder.RenameColumn(
                name: "UnitId",
                table: "Materials",
                newName: "SellingUnitId");

            migrationBuilder.RenameColumn(
                name: "SellingUnit",
                table: "Materials",
                newName: "MaterialName");

            migrationBuilder.AddColumn<int>(
                name: "BuyingUnitId",
                table: "Materials",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "Materials",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Materials_BuyingUnitId",
                table: "Materials",
                column: "BuyingUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_DepartmentId",
                table: "Materials",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_ItemCategoryId",
                table: "Materials",
                column: "ItemCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_ItemTypeId",
                table: "Materials",
                column: "ItemTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_SellingUnitId",
                table: "Materials",
                column: "SellingUnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Materials_Departments_DepartmentId",
                table: "Materials",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Materials_ItemCategories_ItemCategoryId",
                table: "Materials",
                column: "ItemCategoryId",
                principalTable: "ItemCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Materials_ItemTypes_ItemTypeId",
                table: "Materials",
                column: "ItemTypeId",
                principalTable: "ItemTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Materials_Units_BuyingUnitId",
                table: "Materials",
                column: "BuyingUnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Materials_Units_SellingUnitId",
                table: "Materials",
                column: "SellingUnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Materials_Departments_DepartmentId",
                table: "Materials");

            migrationBuilder.DropForeignKey(
                name: "FK_Materials_ItemCategories_ItemCategoryId",
                table: "Materials");

            migrationBuilder.DropForeignKey(
                name: "FK_Materials_ItemTypes_ItemTypeId",
                table: "Materials");

            migrationBuilder.DropForeignKey(
                name: "FK_Materials_Units_BuyingUnitId",
                table: "Materials");

            migrationBuilder.DropForeignKey(
                name: "FK_Materials_Units_SellingUnitId",
                table: "Materials");

            migrationBuilder.DropIndex(
                name: "IX_Materials_BuyingUnitId",
                table: "Materials");

            migrationBuilder.DropIndex(
                name: "IX_Materials_DepartmentId",
                table: "Materials");

            migrationBuilder.DropIndex(
                name: "IX_Materials_ItemCategoryId",
                table: "Materials");

            migrationBuilder.DropIndex(
                name: "IX_Materials_ItemTypeId",
                table: "Materials");

            migrationBuilder.DropIndex(
                name: "IX_Materials_SellingUnitId",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "BuyingUnitId",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Materials");

            migrationBuilder.RenameColumn(
                name: "SellingUnitId",
                table: "Materials",
                newName: "UnitId");

            migrationBuilder.RenameColumn(
                name: "MaterialName",
                table: "Materials",
                newName: "SellingUnit");

            migrationBuilder.AddColumn<string>(
                name: "BuyingUnit",
                table: "Materials",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaterialUserName",
                table: "Materials",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
