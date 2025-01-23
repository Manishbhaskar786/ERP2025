using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPack.Migrations
{
    /// <inheritdoc />
    public partial class Added_CustomerId_In_Design : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CustomerId",
                table: "Designs",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Designs_BoardTypeId",
                table: "Designs",
                column: "BoardTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Designs_BoardTypes_BoardTypeId",
                table: "Designs",
                column: "BoardTypeId",
                principalTable: "BoardTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Designs_BoardTypes_BoardTypeId",
                table: "Designs");

            migrationBuilder.DropIndex(
                name: "IX_Designs_BoardTypeId",
                table: "Designs");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Designs");
        }
    }
}
