using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPack.Migrations
{
    /// <inheritdoc />
    public partial class Added_columns_in_Enquiry_EnquiryMaterial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBraile",
                table: "Enquiries",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "BraileLength",
                table: "Enquiries",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "BraileWidth",
                table: "Enquiries",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BraileComments",
                table: "Enquiries",
                type: "nvarchar(max)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsEmboss",
                table: "Enquiries",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "EmbossLength",
                table: "Enquiries",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "EmbossWidth",
                table: "Enquiries",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmbossComments",
                table: "Enquiries",
                type: "nvarchar(max)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EnquiryMaterials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnquiryId = table.Column<long>(type: "bigint", nullable: false),
                    MaterialId = table.Column<int>(type: "int", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnquiryMaterials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnquiryMaterials_Enquiries_EnquiryId",
                        column: x => x.EnquiryId,
                        principalTable: "Enquiries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EnquiryMaterials_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EnquiryMaterials_EnquiryId",
                table: "EnquiryMaterials",
                column: "EnquiryId");

            migrationBuilder.CreateIndex(
                name: "IX_EnquiryMaterials_MaterialId",
                table: "EnquiryMaterials",
                column: "MaterialId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EnquiryMaterials");

            migrationBuilder.DropColumn(
                name: "BraileComments",
                table: "Enquiries");

            migrationBuilder.DropColumn(
                name: "BraileLength",
                table: "Enquiries");

            migrationBuilder.DropColumn(
                name: "BraileWidth",
                table: "Enquiries");

            migrationBuilder.DropColumn(
                name: "EmbossComments",
                table: "Enquiries");

            migrationBuilder.DropColumn(
                name: "EmbossLength",
                table: "Enquiries");

            migrationBuilder.DropColumn(
                name: "EmbossWidth",
                table: "Enquiries");

            migrationBuilder.DropColumn(
                name: "IsBraile",
                table: "Enquiries");

            migrationBuilder.DropColumn(
                name: "IsEmboss",
                table: "Enquiries");
        }
    }
}
