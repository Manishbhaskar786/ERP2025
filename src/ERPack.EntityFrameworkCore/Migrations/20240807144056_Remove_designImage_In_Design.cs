using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPack.Migrations
{
    /// <inheritdoc />
    public partial class Remove_designImage_In_Design : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Designs_BoardTypes_BoardTypeId",
                table: "Designs");

            migrationBuilder.DropIndex(
                name: "IX_Designs_BoardTypeId",
                table: "Designs");

            migrationBuilder.DropColumn(
                name: "BoardTypeId",
                table: "Designs");

            migrationBuilder.DropColumn(
                name: "DesignImage",
                table: "Designs");

            migrationBuilder.CreateIndex(
                name: "IX_DesignMaterials_DesignId",
                table: "DesignMaterials",
                column: "DesignId");

            migrationBuilder.AddForeignKey(
                name: "FK_DesignMaterials_Designs_DesignId",
                table: "DesignMaterials",
                column: "DesignId",
                principalTable: "Designs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DesignMaterials_Designs_DesignId",
                table: "DesignMaterials");

            migrationBuilder.DropIndex(
                name: "IX_DesignMaterials_DesignId",
                table: "DesignMaterials");

            migrationBuilder.AddColumn<int>(
                name: "BoardTypeId",
                table: "Designs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DesignImage",
                table: "Designs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Designs_BoardTypeId",
                table: "Designs",
                column: "BoardTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Designs_BoardTypes_BoardTypeId",
                table: "Designs",
                column: "BoardTypeId",
                principalTable: "BoardTypes",
                principalColumn: "Id");
        }
    }
}
