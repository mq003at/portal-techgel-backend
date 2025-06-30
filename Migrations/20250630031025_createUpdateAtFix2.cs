using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace portal_techgel_api.Migrations
{
    /// <inheritdoc />
    public partial class createUpdateAtFix2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Signatures",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP"
            );

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Signatures",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP"
            );

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "OrganizationEntityEmployees",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP"
            );

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "OrganizationEntityEmployees",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP"
            );

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "OrganizationEntities",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP"
            );

            migrationBuilder.AlterColumn<string>(
                name: "MainId",
                table: "OrganizationEntities",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValue: ""
            );

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "OrganizationEntities",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP"
            );

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Notifications",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP"
            );

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Notifications",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP"
            );

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "LeaveRequestWorkflows",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP"
            );

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "LeaveRequestWorkflows",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP"
            );

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "LeaveRequestNodes",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP"
            );

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "LeaveRequestNodes",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP"
            );

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "GatePassWorkflows",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP"
            );

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "GatePassWorkflows",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP"
            );

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "GatePassNodes",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP"
            );

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "GatePassNodes",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP"
            );

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Employees",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP"
            );

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Employees",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP"
            );

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Documents",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP"
            );

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Documents",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP"
            );

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationEntities_MainId",
                table: "OrganizationEntities",
                column: "MainId",
                unique: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrganizationEntities_MainId",
                table: "OrganizationEntities"
            );

            migrationBuilder.DropColumn(name: "ChildrenIds", table: "OrganizationEntities");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Signatures",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone"
            );

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Signatures",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone"
            );

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "OrganizationEntityEmployees",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone"
            );

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "OrganizationEntityEmployees",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone"
            );

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "OrganizationEntities",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone"
            );

            migrationBuilder.AlterColumn<string>(
                name: "MainId",
                table: "OrganizationEntities",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldDefaultValue: ""
            );

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "OrganizationEntities",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone"
            );

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Notifications",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone"
            );

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Notifications",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone"
            );

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "LeaveRequestWorkflows",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone"
            );

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "LeaveRequestWorkflows",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone"
            );

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "LeaveRequestNodes",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone"
            );

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "LeaveRequestNodes",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone"
            );

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "GatePassWorkflows",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone"
            );

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "GatePassWorkflows",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone"
            );

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "GatePassNodes",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone"
            );

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "GatePassNodes",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone"
            );

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Employees",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone"
            );

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Employees",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone"
            );

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Documents",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone"
            );

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Documents",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone"
            );
        }
    }
}
