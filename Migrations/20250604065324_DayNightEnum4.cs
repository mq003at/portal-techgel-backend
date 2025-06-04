using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace portal_techgel_api.Migrations
{
    /// <inheritdoc />
    public partial class DayNightEnum4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<List<int>>(
                name: "ReceiverIds",
                table: "LeaveRequestWorkflows",
                type: "integer[]",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<List<int>>(
                name: "HasBeenApprovedByIds",
                table: "LeaveRequestWorkflows",
                type: "integer[]",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<List<int>>(
                name: "DraftedByIds",
                table: "LeaveRequestWorkflows",
                type: "integer[]",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<List<DateTime>>(
                name: "ApprovedDates",
                table: "LeaveRequestWorkflows",
                type: "timestamptz[]",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<float>(
                name: "FinalEmployeeAnnualLeaveTotalDays",
                table: "LeaveRequestWorkflows",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "SenderId",
                table: "LeaveRequestWorkflows",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<List<int>>(
                name: "HasBeenApprovedByIds",
                table: "LeaveRequestNodes",
                type: "integer[]",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<List<int>>(
                name: "DocumentIds",
                table: "LeaveRequestNodes",
                type: "integer[]",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<List<DateTime>>(
                name: "ApprovedDates",
                table: "LeaveRequestNodes",
                type: "timestamptz[]",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<List<int>>(
                name: "ApprovedByIds",
                table: "LeaveRequestNodes",
                type: "integer[]",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SenderId",
                table: "GatePasses",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinalEmployeeAnnualLeaveTotalDays",
                table: "LeaveRequestWorkflows");

            migrationBuilder.DropColumn(
                name: "SenderId",
                table: "LeaveRequestWorkflows");

            migrationBuilder.DropColumn(
                name: "SenderId",
                table: "GatePasses");

            migrationBuilder.AlterColumn<string>(
                name: "ReceiverIds",
                table: "LeaveRequestWorkflows",
                type: "text",
                nullable: false,
                oldClrType: typeof(List<int>),
                oldType: "integer[]");

            migrationBuilder.AlterColumn<string>(
                name: "HasBeenApprovedByIds",
                table: "LeaveRequestWorkflows",
                type: "text",
                nullable: false,
                oldClrType: typeof(List<int>),
                oldType: "integer[]");

            migrationBuilder.AlterColumn<string>(
                name: "DraftedByIds",
                table: "LeaveRequestWorkflows",
                type: "text",
                nullable: false,
                oldClrType: typeof(List<int>),
                oldType: "integer[]");

            migrationBuilder.AlterColumn<string>(
                name: "ApprovedDates",
                table: "LeaveRequestWorkflows",
                type: "text",
                nullable: false,
                oldClrType: typeof(List<DateTime>),
                oldType: "timestamptz[]");

            migrationBuilder.AlterColumn<string>(
                name: "HasBeenApprovedByIds",
                table: "LeaveRequestNodes",
                type: "text",
                nullable: true,
                oldClrType: typeof(List<int>),
                oldType: "integer[]",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DocumentIds",
                table: "LeaveRequestNodes",
                type: "text",
                nullable: true,
                oldClrType: typeof(List<int>),
                oldType: "integer[]",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ApprovedDates",
                table: "LeaveRequestNodes",
                type: "text",
                nullable: true,
                oldClrType: typeof(List<DateTime>),
                oldType: "timestamptz[]",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ApprovedByIds",
                table: "LeaveRequestNodes",
                type: "text",
                nullable: true,
                oldClrType: typeof(List<int>),
                oldType: "integer[]",
                oldNullable: true);
        }
    }
}
