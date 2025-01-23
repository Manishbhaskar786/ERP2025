using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPack.Migrations
{
    /// <inheritdoc />
    public partial class Added_dates_In_Workorder_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TaskIssueActualCompleteDate",
                table: "Workorders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TaskIssueCompleteDate",
                table: "Workorders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TaskIssueDate",
                table: "Workorders",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaskIssueActualCompleteDate",
                table: "Workorders");

            migrationBuilder.DropColumn(
                name: "TaskIssueCompleteDate",
                table: "Workorders");

            migrationBuilder.DropColumn(
                name: "TaskIssueDate",
                table: "Workorders");
        }
    }
}
