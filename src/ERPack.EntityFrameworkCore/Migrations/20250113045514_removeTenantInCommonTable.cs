using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPack.Migrations
{
    /// <inheritdoc />
    public partial class removeTenantInCommonTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StateMasters_CountryMasters_CountryId",
                table: "StateMasters");

            migrationBuilder.DropIndex(
                name: "IX_StateMasters_CountryId",
                table: "StateMasters");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "StateMasters");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "CurrencyMasters");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "CountryMasters");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "StateMasters",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "CurrencyMasters",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "CountryMasters",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StateMasters_CountryId",
                table: "StateMasters",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_StateMasters_CountryMasters_CountryId",
                table: "StateMasters",
                column: "CountryId",
                principalTable: "CountryMasters",
                principalColumn: "CountryId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
