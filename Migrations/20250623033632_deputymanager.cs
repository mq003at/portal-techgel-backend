using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace portal_techgel_api.Migrations
{
    /// <inheritdoc />
    public partial class deputymanager : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_RoleInfo_RoleInfoId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationEntityEmployees_RoleInfo_RoleInfoId",
                table: "OrganizationEntityEmployees");

            migrationBuilder.DropTable(
                name: "RoleInfoManagedOrganizationEntities");

            migrationBuilder.DropTable(
                name: "RoleInfo");

            migrationBuilder.RenameColumn(
                name: "RoleInfoId",
                table: "OrganizationEntityEmployees",
                newName: "EmployeeId1");

            migrationBuilder.RenameIndex(
                name: "IX_OrganizationEntityEmployees_RoleInfoId",
                table: "OrganizationEntityEmployees",
                newName: "IX_OrganizationEntityEmployees_EmployeeId1");

            migrationBuilder.RenameColumn(
                name: "RoleInfoId",
                table: "Employees",
                newName: "SupervisorId");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_RoleInfoId",
                table: "Employees",
                newName: "IX_Employees_SupervisorId");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "OrganizationEntityEmployees",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "OrganizationEntityEmployees",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<bool>(
                name: "IsPrimary",
                table: "OrganizationEntityEmployees",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MainId",
                table: "OrganizationEntityEmployees",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "OrganizationRelationType",
                table: "OrganizationEntityEmployees",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "OrganizationEntityEmployees",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<int>(
                name: "DeputySupervisorId",
                table: "Employees",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DeputySupervisorId",
                table: "Employees",
                column: "DeputySupervisorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Employees_DeputySupervisorId",
                table: "Employees",
                column: "DeputySupervisorId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Employees_SupervisorId",
                table: "Employees",
                column: "SupervisorId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationEntityEmployees_Employees_EmployeeId1",
                table: "OrganizationEntityEmployees",
                column: "EmployeeId1",
                principalTable: "Employees",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Employees_DeputySupervisorId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Employees_SupervisorId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationEntityEmployees_Employees_EmployeeId1",
                table: "OrganizationEntityEmployees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_DeputySupervisorId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "OrganizationEntityEmployees");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "OrganizationEntityEmployees");

            migrationBuilder.DropColumn(
                name: "IsPrimary",
                table: "OrganizationEntityEmployees");

            migrationBuilder.DropColumn(
                name: "MainId",
                table: "OrganizationEntityEmployees");

            migrationBuilder.DropColumn(
                name: "OrganizationRelationType",
                table: "OrganizationEntityEmployees");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "OrganizationEntityEmployees");

            migrationBuilder.DropColumn(
                name: "DeputySupervisorId",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "EmployeeId1",
                table: "OrganizationEntityEmployees",
                newName: "RoleInfoId");

            migrationBuilder.RenameIndex(
                name: "IX_OrganizationEntityEmployees_EmployeeId1",
                table: "OrganizationEntityEmployees",
                newName: "IX_OrganizationEntityEmployees_RoleInfoId");

            migrationBuilder.RenameColumn(
                name: "SupervisorId",
                table: "Employees",
                newName: "RoleInfoId");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_SupervisorId",
                table: "Employees",
                newName: "IX_Employees_RoleInfoId");

            migrationBuilder.CreateTable(
                name: "RoleInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DeputySupervisorId = table.Column<int>(type: "integer", nullable: true),
                    EmployeeId = table.Column<int>(type: "integer", nullable: false),
                    SupervisorId = table.Column<int>(type: "integer", nullable: true),
                    GroupId = table.Column<int>(type: "integer", nullable: true),
                    ManagedOrganizationEntityIds = table.Column<List<int>>(type: "integer[]", nullable: false)
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
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RoleInfo_Employees_SupervisorId",
                        column: x => x.SupervisorId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RoleInfoManagedOrganizationEntities",
                columns: table => new
                {
                    ManagedOrganizationEntitiesId = table.Column<int>(type: "integer", nullable: false),
                    RoleInfoId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleInfoManagedOrganizationEntities", x => new { x.ManagedOrganizationEntitiesId, x.RoleInfoId });
                    table.ForeignKey(
                        name: "FK_RoleInfoManagedOrganizationEntities_OrganizationEntities_Ma~",
                        column: x => x.ManagedOrganizationEntitiesId,
                        principalTable: "OrganizationEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleInfoManagedOrganizationEntities_RoleInfo_RoleInfoId",
                        column: x => x.RoleInfoId,
                        principalTable: "RoleInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_RoleInfoManagedOrganizationEntities_RoleInfoId",
                table: "RoleInfoManagedOrganizationEntities",
                column: "RoleInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_RoleInfo_RoleInfoId",
                table: "Employees",
                column: "RoleInfoId",
                principalTable: "RoleInfo",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationEntityEmployees_RoleInfo_RoleInfoId",
                table: "OrganizationEntityEmployees",
                column: "RoleInfoId",
                principalTable: "RoleInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
