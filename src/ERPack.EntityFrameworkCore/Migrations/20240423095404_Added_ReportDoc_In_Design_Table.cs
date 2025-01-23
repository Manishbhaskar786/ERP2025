using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPack.Migrations
{
    /// <inheritdoc />
    public partial class Added_ReportDoc_In_Design_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReportDoc",
                table: "Designs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EstimateTasks_MaterialId",
                table: "EstimateTasks",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_EstimateTasks_UnitId",
                table: "EstimateTasks",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Estimate_CustomerId",
                table: "Estimate",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Estimate_Customers_CustomerId",
                table: "Estimate",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EstimateTasks_Materials_MaterialId",
                table: "EstimateTasks",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EstimateTasks_Units_UnitId",
                table: "EstimateTasks",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estimate_Customers_CustomerId",
                table: "Estimate");

            migrationBuilder.DropForeignKey(
                name: "FK_EstimateTasks_Materials_MaterialId",
                table: "EstimateTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_EstimateTasks_Units_UnitId",
                table: "EstimateTasks");

            migrationBuilder.DropIndex(
                name: "IX_EstimateTasks_MaterialId",
                table: "EstimateTasks");

            migrationBuilder.DropIndex(
                name: "IX_EstimateTasks_UnitId",
                table: "EstimateTasks");

            migrationBuilder.DropIndex(
                name: "IX_Estimate_CustomerId",
                table: "Estimate");

            migrationBuilder.DropColumn(
                name: "ReportDoc",
                table: "Designs");
        }
    }
}
