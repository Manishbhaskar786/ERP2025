using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPack.Migrations
{
    /// <inheritdoc />
    public partial class dataTypeChangeTaxation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "CustomerTaxationInfos");

            migrationBuilder.DropColumn(
                name: "State",
                table: "CustomerTaxationInfos");

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "CustomerTaxationInfos",
                type: "int",
                maxLength: 100,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StateId",
                table: "CustomerTaxationInfos",
                type: "int",
                maxLength: 50,
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "CustomerTaxationInfos");

            migrationBuilder.DropColumn(
                name: "StateId",
                table: "CustomerTaxationInfos");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "CustomerTaxationInfos",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "CustomerTaxationInfos",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
