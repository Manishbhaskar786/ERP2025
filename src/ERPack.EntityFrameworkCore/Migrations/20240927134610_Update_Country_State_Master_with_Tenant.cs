using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPack.Migrations
{
    /// <inheritdoc />
    public partial class Update_Country_State_Master_with_Tenant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "StateMasters",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                table: "StateMasters",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeleterUserId",
                table: "StateMasters",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "StateMasters",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "StateMasters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "StateMasters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "StateMasters",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastModifierUserId",
                table: "StateMasters",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "StateMasters",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "CountryMasters",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                table: "CountryMasters",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeleterUserId",
                table: "CountryMasters",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "CountryMasters",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "CountryMasters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CountryMasters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "CountryMasters",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastModifierUserId",
                table: "CountryMasters",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "CountryMasters",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "StateMasters");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "StateMasters");

            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                table: "StateMasters");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "StateMasters");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "StateMasters");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "StateMasters");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "StateMasters");

            migrationBuilder.DropColumn(
                name: "LastModifierUserId",
                table: "StateMasters");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "StateMasters");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "CountryMasters");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "CountryMasters");

            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                table: "CountryMasters");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "CountryMasters");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CountryMasters");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CountryMasters");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "CountryMasters");

            migrationBuilder.DropColumn(
                name: "LastModifierUserId",
                table: "CountryMasters");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "CountryMasters");
        }
    }
}
