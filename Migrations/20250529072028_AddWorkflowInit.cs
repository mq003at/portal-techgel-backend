using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace portal_techgel_api.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkflowInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MainId",
                table: "Signatures",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "MainId",
                table: "Employees",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "MainId",
                table: "Documents",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "LegalDocumentInfo_PublishDate",
                table: "Documents",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LegalDocumentInfo_InspectionDate",
                table: "Documents",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LegalDocumentInfo_FinalApprovalDate",
                table: "Documents",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LegalDocumentInfo_ExpiredDate",
                table: "Documents",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LegalDocumentInfo_EffectiveDate",
                table: "Documents",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LegalDocumentInfo_DraftDate",
                table: "Documents",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ApprovalWorkflowNodeId",
                table: "Documents",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int[]>(
                name: "GeneralDocumentInfo_GeneralWorkflowIds",
                table: "Documents",
                type: "integer[]",
                nullable: false,
                defaultValue: new int[0]);

            migrationBuilder.CreateTable(
                name: "GeneralWorkflows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GeneralWorkflowInfo_Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    GeneralWorkflowInfo_Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    GeneralWorkflowInfo_Status = table.Column<int>(type: "integer", nullable: false),
                    GeneralWorkflowInfo_WorkflowLogic = table.Column<int>(type: "integer", nullable: false),
                    GeneralWorkflowInfo_ApprovedByIds = table.Column<int[]>(type: "integer[]", nullable: false),
                    GeneralWorkflowInfo_DraftedByIds = table.Column<int[]>(type: "integer[]", nullable: false),
                    GeneralWorkflowInfo_Quota = table.Column<int>(type: "integer", nullable: true),
                    MainId = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralWorkflows", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApprovalWorkflowNodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    SenderId = table.Column<int>(type: "integer", nullable: false),
                    SenderName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    SenderMessage = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true, defaultValue: ""),
                    ReceiverId = table.Column<int>(type: "integer", nullable: false),
                    ReceiverName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    ReceiverMessage = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true, defaultValue: ""),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ApprovalDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ApprovalComment = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true, defaultValue: ""),
                    SortOrder = table.Column<int>(type: "integer", nullable: true),
                    GeneralWorkflowId = table.Column<int>(type: "integer", nullable: false),
                    DocumentIds = table.Column<List<int>>(type: "integer[]", nullable: false),
                    MainId = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApprovalWorkflowNodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApprovalWorkflowNodes_GeneralWorkflows_GeneralWorkflowId",
                        column: x => x.GeneralWorkflowId,
                        principalTable: "GeneralWorkflows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Documents_ApprovalWorkflowNodeId",
                table: "Documents",
                column: "ApprovalWorkflowNodeId");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalWorkflowNodes_GeneralWorkflowId",
                table: "ApprovalWorkflowNodes",
                column: "GeneralWorkflowId");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_ApprovalWorkflowNodes_ApprovalWorkflowNodeId",
                table: "Documents",
                column: "ApprovalWorkflowNodeId",
                principalTable: "ApprovalWorkflowNodes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_ApprovalWorkflowNodes_ApprovalWorkflowNodeId",
                table: "Documents");

            migrationBuilder.DropTable(
                name: "ApprovalWorkflowNodes");

            migrationBuilder.DropTable(
                name: "GeneralWorkflows");

            migrationBuilder.DropIndex(
                name: "IX_Documents_ApprovalWorkflowNodeId",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "ApprovalWorkflowNodeId",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "GeneralDocumentInfo_GeneralWorkflowIds",
                table: "Documents");

            migrationBuilder.AlterColumn<string>(
                name: "MainId",
                table: "Signatures",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "MainId",
                table: "Employees",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "MainId",
                table: "Documents",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LegalDocumentInfo_PublishDate",
                table: "Documents",
                type: "date",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LegalDocumentInfo_InspectionDate",
                table: "Documents",
                type: "date",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LegalDocumentInfo_FinalApprovalDate",
                table: "Documents",
                type: "date",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LegalDocumentInfo_ExpiredDate",
                table: "Documents",
                type: "date",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LegalDocumentInfo_EffectiveDate",
                table: "Documents",
                type: "date",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LegalDocumentInfo_DraftDate",
                table: "Documents",
                type: "date",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldDefaultValue: "");
        }
    }
}
