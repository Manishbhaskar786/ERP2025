using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPack.Migrations
{
    /// <inheritdoc />
    public partial class Update_In_Vendors_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AcountNumber",
                table: "Vendors",
                newName: "AccountNumber");

            migrationBuilder.AddColumn<string>(
                name: "VendorCode",
                table: "Vendors",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VendorCode",
                table: "Vendors");

            migrationBuilder.RenameColumn(
                name: "AccountNumber",
                table: "Vendors",
                newName: "AcountNumber");
        }
    }
}
