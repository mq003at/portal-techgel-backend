using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace portal_techgel_api.Migrations
{
    /// <inheritdoc />
    public partial class nodeAppovalStartEnd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Employees_SupervisorId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_SupervisorId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ManagedOrganizationEntityIds",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "SubordinateIds",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "SupervisorId",
                table: "Employees");

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovalStartDate",
                table: "WorkflowNodeParticipants",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoleInfoId",
                table: "OrganizationEntityEmployees",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoleInfoId",
                table: "OrganizationEntities",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "TotalDays",
                table: "LeaveRequestWorkflows",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<double>(
                name: "FinalEmployeeAnnualLeaveTotalDays",
                table: "LeaveRequestWorkflows",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<double>(
                name: "EmployeeAnnualLeaveTotalDays",
                table: "LeaveRequestWorkflows",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<double>(
                name: "CompanyInfo_AnnualLeaveTotalDays",
                table: "Employees",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AddColumn<double>(
                name: "CompanyInfo_CompensatoryLeaveTotalDays",
                table: "Employees",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "RoleInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmployeeId = table.Column<int>(type: "integer", nullable: false),
                    SupervisorId = table.Column<int>(type: "integer", nullable: false),
                    SubordinateIds = table.Column<List<int>>(type: "integer[]", nullable: false),
                    DeputySupervisorId = table.Column<int>(type: "integer", nullable: false),
                    ManagedOrganizationEntityIds = table.Column<List<int>>(type: "integer[]", nullable: false),
                    GroupId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleInfo_Employees_DeputySupervisorId",
                        column: x => x.DeputySupervisorId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RoleInfo_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleInfo_Employees_SupervisorId",
                        column: x => x.SupervisorId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationEntityEmployees_RoleInfoId",
                table: "OrganizationEntityEmployees",
                column: "RoleInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationEntities_RoleInfoId",
                table: "OrganizationEntities",
                column: "RoleInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleInfo_DeputySupervisorId",
                table: "RoleInfo",
                column: "DeputySupervisorId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleInfo_EmployeeId",
                table: "RoleInfo",
                column: "EmployeeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoleInfo_SupervisorId",
                table: "RoleInfo",
                column: "SupervisorId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationEntities_RoleInfo_RoleInfoId",
                table: "OrganizationEntities",
                column: "RoleInfoId",
                principalTable: "RoleInfo",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationEntityEmployees_RoleInfo_RoleInfoId",
                table: "OrganizationEntityEmployees",
                column: "RoleInfoId",
                principalTable: "RoleInfo",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationEntities_RoleInfo_RoleInfoId",
                table: "OrganizationEntities");

            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationEntityEmployees_RoleInfo_RoleInfoId",
                table: "OrganizationEntityEmployees");

            migrationBuilder.DropTable(
                name: "RoleInfo");

            migrationBuilder.DropIndex(
                name: "IX_OrganizationEntityEmployees_RoleInfoId",
                table: "OrganizationEntityEmployees");

            migrationBuilder.DropIndex(
                name: "IX_OrganizationEntities_RoleInfoId",
                table: "OrganizationEntities");

            migrationBuilder.DropColumn(
                name: "ApprovalStartDate",
                table: "WorkflowNodeParticipants");

            migrationBuilder.DropColumn(
                name: "RoleInfoId",
                table: "OrganizationEntityEmployees");

            migrationBuilder.DropColumn(
                name: "RoleInfoId",
                table: "OrganizationEntities");

            migrationBuilder.DropColumn(
                name: "CompanyInfo_CompensatoryLeaveTotalDays",
                table: "Employees");

            migrationBuilder.AlterColumn<float>(
                name: "TotalDays",
                table: "LeaveRequestWorkflows",
                type: "real",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<float>(
                name: "FinalEmployeeAnnualLeaveTotalDays",
                table: "LeaveRequestWorkflows",
                type: "real",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<float>(
                name: "EmployeeAnnualLeaveTotalDays",
                table: "LeaveRequestWorkflows",
                type: "real",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<float>(
                name: "CompanyInfo_AnnualLeaveTotalDays",
                table: "Employees",
                type: "real",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "Employees",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<List<int>>(
                name: "ManagedOrganizationEntityIds",
                table: "Employees",
                type: "integer[]",
                nullable: false);

            migrationBuilder.AddColumn<List<int>>(
                name: "SubordinateIds",
                table: "Employees",
                type: "integer[]",
                nullable: false);

            migrationBuilder.AddColumn<int>(
                name: "SupervisorId",
                table: "Employees",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_SupervisorId",
                table: "Employees",
                column: "SupervisorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Employees_SupervisorId",
                table: "Employees",
                column: "SupervisorId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
