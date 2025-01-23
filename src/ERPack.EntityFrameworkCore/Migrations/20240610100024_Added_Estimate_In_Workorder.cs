using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPack.Migrations
{
    /// <inheritdoc />
    public partial class Added_Estimate_In_Workorder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "EstimateId",
                table: "Workorders",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Workorders_EstimateId",
                table: "Workorders",
                column: "EstimateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Workorders_Estimate_EstimateId",
                table: "Workorders",
                column: "EstimateId",
                principalTable: "Estimate",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workorders_Estimate_EstimateId",
                table: "Workorders");

            migrationBuilder.DropIndex(
                name: "IX_Workorders_EstimateId",
                table: "Workorders");

            migrationBuilder.AlterColumn<int>(
                name: "EstimateId",
                table: "Workorders",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);
        }
    }
}
