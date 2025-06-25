using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace portal_techgel_api.Migrations
{
    /// <inheritdoc />
    public partial class DeputyManagerForOrg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationEntities_Employees_ManagerId",
                table: "OrganizationEntities");

            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationEntities_OrganizationEntities_ParentId",
                table: "OrganizationEntities");

            migrationBuilder.DropIndex(
                name: "IX_OrganizationEntities_ManagerId",
                table: "OrganizationEntities");

            migrationBuilder.DropColumn(
                name: "ChildrenIds",
                table: "OrganizationEntities");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "OrganizationEntities");

            migrationBuilder.RenameColumn(
                name: "NodeId",
                table: "DocumentAssociations",
                newName: "EntityId");



            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "OrganizationEntityEmployees",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<string>(
                name: "MainId",
                table: "OrganizationEntityEmployees",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPrimary",
                table: "OrganizationEntityEmployees",
                type: "boolean",
                nullable: false,
                comment: "Marks the primary association of this employee to the org entity.",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "OrganizationEntityEmployees",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "OrganizationEntityEmployees",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "OrganizationEntities",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<int>(
                name: "SortOrder",
                table: "OrganizationEntities",
                type: "integer",
                nullable: true,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "OrganizationEntities",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "MainId",
                table: "OrganizationEntities",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "OrganizationEntities",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "OrganizationEntities",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<int>(
                name: "OrganizationEntityId",
                table: "Employees",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationEntityEmployees_OrganizationEntityId_IsPrimary",
                table: "OrganizationEntityEmployees",
                columns: new[] { "OrganizationEntityId", "IsPrimary" });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_OrganizationEntityId",
                table: "Employees",
                column: "OrganizationEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_OrganizationEntities_OrganizationEntityId",
                table: "Employees",
                column: "OrganizationEntityId",
                principalTable: "OrganizationEntities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationEntities_OrganizationEntities_ParentId",
                table: "OrganizationEntities",
                column: "ParentId",
                principalTable: "OrganizationEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_OrganizationEntities_OrganizationEntityId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationEntities_OrganizationEntities_ParentId",
                table: "OrganizationEntities");

            migrationBuilder.DropIndex(
                name: "IX_OrganizationEntityEmployees_OrganizationEntityId_IsPrimary",
                table: "OrganizationEntityEmployees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_OrganizationEntityId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "OrganizationEntityId",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "EntityId",
                table: "DocumentAssociations",
                newName: "NodeId");



            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "OrganizationEntityEmployees",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<string>(
                name: "MainId",
                table: "OrganizationEntityEmployees",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPrimary",
                table: "OrganizationEntityEmployees",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "Marks the primary association of this employee to the org entity.");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "OrganizationEntityEmployees",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "OrganizationEntityEmployees",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "OrganizationEntities",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<int>(
                name: "SortOrder",
                table: "OrganizationEntities",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "OrganizationEntities",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "MainId",
                table: "OrganizationEntities",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "OrganizationEntities",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "OrganizationEntities",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<List<int>>(
                name: "ChildrenIds",
                table: "OrganizationEntities",
                type: "integer[]",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ManagerId",
                table: "OrganizationEntities",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationEntities_ManagerId",
                table: "OrganizationEntities",
                column: "ManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationEntities_Employees_ManagerId",
                table: "OrganizationEntities",
                column: "ManagerId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationEntities_OrganizationEntities_ParentId",
                table: "OrganizationEntities",
                column: "ParentId",
                principalTable: "OrganizationEntities",
                principalColumn: "Id");
        }
    }
}
