using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPack.Migrations
{
    /// <inheritdoc />
    public partial class rename_boxColumns_in_Enquiry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enquiries_BoardTypes_BoardTypeId",
                table: "Enquiries");

            migrationBuilder.RenameColumn(
                name: "Width",
                table: "Enquiries",
                newName: "BoxWidth");

            migrationBuilder.RenameColumn(
                name: "Length",
                table: "Enquiries",
                newName: "BoxLength");

            migrationBuilder.RenameColumn(
                name: "Height",
                table: "Enquiries",
                newName: "BoxHeight");

            migrationBuilder.AlterColumn<int>(
                name: "BoardTypeId",
                table: "Enquiries",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Enquiries_BoardTypes_BoardTypeId",
                table: "Enquiries",
                column: "BoardTypeId",
                principalTable: "BoardTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enquiries_BoardTypes_BoardTypeId",
                table: "Enquiries");

            migrationBuilder.RenameColumn(
                name: "BoxWidth",
                table: "Enquiries",
                newName: "Width");

            migrationBuilder.RenameColumn(
                name: "BoxLength",
                table: "Enquiries",
                newName: "Length");

            migrationBuilder.RenameColumn(
                name: "BoxHeight",
                table: "Enquiries",
                newName: "Height");

            migrationBuilder.AlterColumn<int>(
                name: "BoardTypeId",
                table: "Enquiries",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Enquiries_BoardTypes_BoardTypeId",
                table: "Enquiries",
                column: "BoardTypeId",
                principalTable: "BoardTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
