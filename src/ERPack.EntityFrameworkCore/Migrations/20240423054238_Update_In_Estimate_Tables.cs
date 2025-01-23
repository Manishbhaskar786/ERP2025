using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPack.Migrations
{
    /// <inheritdoc />
    public partial class Update_In_Estimate_Tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CGSTAmount",
                table: "EstimateTasks");

            migrationBuilder.RenameColumn(
                name: "SGSTAmount",
                table: "EstimateTasks",
                newName: "SGST");

            migrationBuilder.RenameColumn(
                name: "NetAmount",
                table: "EstimateTasks",
                newName: "IGST");

            migrationBuilder.RenameColumn(
                name: "IGSTAmount",
                table: "EstimateTasks",
                newName: "CGST");

            migrationBuilder.RenameColumn(
                name: "GrossAmount",
                table: "EstimateTasks",
                newName: "Amount");

            migrationBuilder.AddColumn<decimal>(
                name: "CGSTAmount",
                table: "Estimate",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "GrossAmount",
                table: "Estimate",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "IGSTAmount",
                table: "Estimate",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SGSTAmount",
                table: "Estimate",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_DesignMaterials_MaterialId",
                table: "DesignMaterials",
                column: "MaterialId");

            migrationBuilder.AddForeignKey(
                name: "FK_DesignMaterials_Materials_MaterialId",
                table: "DesignMaterials",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DesignMaterials_Materials_MaterialId",
                table: "DesignMaterials");

            migrationBuilder.DropIndex(
                name: "IX_DesignMaterials_MaterialId",
                table: "DesignMaterials");

            migrationBuilder.DropColumn(
                name: "CGSTAmount",
                table: "Estimate");

            migrationBuilder.DropColumn(
                name: "GrossAmount",
                table: "Estimate");

            migrationBuilder.DropColumn(
                name: "IGSTAmount",
                table: "Estimate");

            migrationBuilder.DropColumn(
                name: "SGSTAmount",
                table: "Estimate");

            migrationBuilder.RenameColumn(
                name: "SGST",
                table: "EstimateTasks",
                newName: "SGSTAmount");

            migrationBuilder.RenameColumn(
                name: "IGST",
                table: "EstimateTasks",
                newName: "NetAmount");

            migrationBuilder.RenameColumn(
                name: "CGST",
                table: "EstimateTasks",
                newName: "IGSTAmount");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "EstimateTasks",
                newName: "GrossAmount");

            migrationBuilder.AddColumn<decimal>(
                name: "CGSTAmount",
                table: "EstimateTasks",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }
    }
}
