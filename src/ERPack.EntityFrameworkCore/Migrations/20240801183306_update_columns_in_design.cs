using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPack.Migrations
{
    /// <inheritdoc />
    public partial class update_columns_in_design : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comments",
                table: "Designs");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Designs");

            migrationBuilder.DropColumn(
                name: "DesignName",
                table: "Designs");

            migrationBuilder.DropColumn(
                name: "DesignNumber",
                table: "Designs");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "Designs");

            migrationBuilder.DropColumn(
                name: "Length",
                table: "Designs");

            migrationBuilder.DropColumn(
                name: "SheetSizeLength",
                table: "Designs");

            migrationBuilder.DropColumn(
                name: "SheetSizeWidth",
                table: "Designs");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "Designs");

            migrationBuilder.AlterColumn<int>(
                name: "NumberOfUps",
                table: "Enquiries",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "NumberOfUps",
                table: "Enquiries",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Comments",
                table: "Designs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CustomerId",
                table: "Designs",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DesignName",
                table: "Designs",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DesignNumber",
                table: "Designs",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Height",
                table: "Designs",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Length",
                table: "Designs",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SheetSizeLength",
                table: "Designs",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SheetSizeWidth",
                table: "Designs",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Width",
                table: "Designs",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);
        }
    }
}
