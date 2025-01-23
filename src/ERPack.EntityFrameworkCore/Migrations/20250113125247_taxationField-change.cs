using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPack.Migrations
{
    /// <inheritdoc />
    public partial class taxationFieldchange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TaxationStateId",
                table: "CustomerTaxationInfos",
                newName: "StateId");

            migrationBuilder.RenameColumn(
                name: "TaxationCountryId",
                table: "CustomerTaxationInfos",
                newName: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerTaxationInfos_CountryId",
                table: "CustomerTaxationInfos",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerTaxationInfos_StateId",
                table: "CustomerTaxationInfos",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CountryId",
                table: "Customers",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_StateId",
                table: "Customers",
                column: "StateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_CountryMasters_CountryId",
                table: "Customers",
                column: "CountryId",
                principalTable: "CountryMasters",
                principalColumn: "CountryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_StateMasters_StateId",
                table: "Customers",
                column: "StateId",
                principalTable: "StateMasters",
                principalColumn: "StateId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerTaxationInfos_CountryMasters_CountryId",
                table: "CustomerTaxationInfos",
                column: "CountryId",
                principalTable: "CountryMasters",
                principalColumn: "CountryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerTaxationInfos_StateMasters_StateId",
                table: "CustomerTaxationInfos",
                column: "StateId",
                principalTable: "StateMasters",
                principalColumn: "StateId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_CountryMasters_CountryId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_StateMasters_StateId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerTaxationInfos_CountryMasters_CountryId",
                table: "CustomerTaxationInfos");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerTaxationInfos_StateMasters_StateId",
                table: "CustomerTaxationInfos");

            migrationBuilder.DropIndex(
                name: "IX_CustomerTaxationInfos_CountryId",
                table: "CustomerTaxationInfos");

            migrationBuilder.DropIndex(
                name: "IX_CustomerTaxationInfos_StateId",
                table: "CustomerTaxationInfos");

            migrationBuilder.DropIndex(
                name: "IX_Customers_CountryId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_StateId",
                table: "Customers");

            migrationBuilder.RenameColumn(
                name: "StateId",
                table: "CustomerTaxationInfos",
                newName: "TaxationStateId");

            migrationBuilder.RenameColumn(
                name: "CountryId",
                table: "CustomerTaxationInfos",
                newName: "TaxationCountryId");
        }
    }
}
