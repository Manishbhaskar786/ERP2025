using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPack.Migrations
{
    /// <inheritdoc />
    public partial class Added_Enquires_From_workorder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Estimate_EnquiryId",
                table: "Estimate",
                column: "EnquiryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Estimate_Enquiries_EnquiryId",
                table: "Estimate",
                column: "EnquiryId",
                principalTable: "Enquiries",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estimate_Enquiries_EnquiryId",
                table: "Estimate");

            migrationBuilder.DropIndex(
                name: "IX_Estimate_EnquiryId",
                table: "Estimate");
        }
    }
}
