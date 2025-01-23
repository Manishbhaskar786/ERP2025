using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPack.Migrations
{
    /// <inheritdoc />
    public partial class Update_ToolConfigurationName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ToolConfiguraionName",
                table: "ToolConfigurations",
                newName: "ToolConfigurationName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ToolConfigurationName",
                table: "ToolConfigurations",
                newName: "ToolConfiguraionName");
        }
    }
}
