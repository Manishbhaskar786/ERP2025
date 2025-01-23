using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPack.Migrations
{
    /// <inheritdoc />
    public partial class Added_BoardType_In_Enquiry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Enquiries_BoardTypeId",
                table: "Enquiries",
                column: "BoardTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Enquiries_BoardTypes_BoardTypeId",
                table: "Enquiries",
                column: "BoardTypeId",
                principalTable: "BoardTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enquiries_BoardTypes_BoardTypeId",
                table: "Enquiries");

            migrationBuilder.DropIndex(
                name: "IX_Enquiries_BoardTypeId",
                table: "Enquiries");
        }
    }
}
