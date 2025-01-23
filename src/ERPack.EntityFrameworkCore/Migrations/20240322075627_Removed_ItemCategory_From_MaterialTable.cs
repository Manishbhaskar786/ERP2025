using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPack.Migrations
{
    /// <inheritdoc />
    public partial class Removed_ItemCategory_From_MaterialTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Materials_ItemCategories_ItemCategoryId",
                table: "Materials");

            migrationBuilder.DropIndex(
                name: "IX_Materials_ItemCategoryId",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "ItemCategoryId",
                table: "Materials");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ItemCategoryId",
                table: "Materials",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Materials_ItemCategoryId",
                table: "Materials",
                column: "ItemCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Materials_ItemCategories_ItemCategoryId",
                table: "Materials",
                column: "ItemCategoryId",
                principalTable: "ItemCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
