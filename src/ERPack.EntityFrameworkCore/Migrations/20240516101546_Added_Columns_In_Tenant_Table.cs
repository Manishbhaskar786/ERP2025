using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPack.Migrations
{
    /// <inheritdoc />
    public partial class Added_Columns_In_Tenant_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address1",
                table: "AbpTenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address2",
                table: "AbpTenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "AbpTenants",
                type: "nvarchar(170)",
                maxLength: 170,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "AbpTenants",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Logo",
                table: "AbpTenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PinCode",
                table: "AbpTenants",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "AbpTenants",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address1",
                table: "AbpTenants");

            migrationBuilder.DropColumn(
                name: "Address2",
                table: "AbpTenants");

            migrationBuilder.DropColumn(
                name: "City",
                table: "AbpTenants");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "AbpTenants");

            migrationBuilder.DropColumn(
                name: "Logo",
                table: "AbpTenants");

            migrationBuilder.DropColumn(
                name: "PinCode",
                table: "AbpTenants");

            migrationBuilder.DropColumn(
                name: "State",
                table: "AbpTenants");
        }
    }
}
