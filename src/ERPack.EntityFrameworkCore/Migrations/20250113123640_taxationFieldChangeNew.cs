using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPack.Migrations
{
    /// <inheritdoc />
    public partial class taxationFieldChangeNew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "StateId",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TaxationStateId",
                table: "CustomerTaxationInfos",
                newName: "StateId");

            migrationBuilder.RenameColumn(
                name: "TaxationCountryId",
                table: "CustomerTaxationInfos",
                newName: "CountryId");

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "Customers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StateId",
                table: "Customers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerTaxationInfos_CountryId",
                table: "CustomerTaxationInfos",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerTaxationInfos_StateId",
                table: "CustomerTaxationInfos",
                column: "StateId");

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
    }
}
