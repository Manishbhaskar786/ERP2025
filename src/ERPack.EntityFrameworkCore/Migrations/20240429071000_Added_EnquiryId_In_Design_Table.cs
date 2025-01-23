using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPack.Migrations
{
    /// <inheritdoc />
    public partial class Added_EnquiryId_In_Design_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "EnquiryId",
                table: "Designs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_WorkorderTasks_UserId",
                table: "WorkorderTasks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkorderTasks_WorkorderSubTaskId",
                table: "WorkorderTasks",
                column: "WorkorderSubTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkorderTasks_AbpUsers_UserId",
                table: "WorkorderTasks",
                column: "UserId",
                principalTable: "AbpUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkorderTasks_WorkorderSubTasks_WorkorderSubTaskId",
                table: "WorkorderTasks",
                column: "WorkorderSubTaskId",
                principalTable: "WorkorderSubTasks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkorderTasks_AbpUsers_UserId",
                table: "WorkorderTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkorderTasks_WorkorderSubTasks_WorkorderSubTaskId",
                table: "WorkorderTasks");

            migrationBuilder.DropIndex(
                name: "IX_WorkorderTasks_UserId",
                table: "WorkorderTasks");

            migrationBuilder.DropIndex(
                name: "IX_WorkorderTasks_WorkorderSubTaskId",
                table: "WorkorderTasks");

            migrationBuilder.DropColumn(
                name: "EnquiryId",
                table: "Designs");
        }
    }
}
