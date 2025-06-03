using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace portal_techgel_api.Migrations
{
    /// <inheritdoc />
    public partial class LeaveRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApprovalWorkflowNodeDocuments");

            migrationBuilder.DropTable(
                name: "ApprovalWorkflowNodes");

            migrationBuilder.DropTable(
                name: "GeneralWorkflows");

            migrationBuilder.AddColumn<float>(
                name: "CompanyInfo_AnnualLeaveTotalDays",
                table: "Employees",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.CreateTable(
                name: "GatePasses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmployeeId = table.Column<int>(type: "integer", nullable: false),
                    Reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MainId = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ReceiverIds = table.Column<string>(type: "text", nullable: false),
                    DraftedByIds = table.Column<string>(type: "text", nullable: false),
                    HasBeenApprovedByIds = table.Column<string>(type: "text", nullable: false),
                    ApprovedDates = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GatePasses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LeaveRequestWorkflows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmployeeId = table.Column<int>(type: "integer", nullable: false),
                    Reason = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EmployeeAnnualLeaveTotalDays = table.Column<float>(type: "real", nullable: false),
                    MainId = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ReceiverIds = table.Column<string>(type: "text", nullable: false),
                    DraftedByIds = table.Column<string>(type: "text", nullable: false),
                    HasBeenApprovedByIds = table.Column<string>(type: "text", nullable: false),
                    ApprovedDates = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveRequestWorkflows", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GatePassNodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GatePassId = table.Column<int>(type: "integer", nullable: false),
                    MainId = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    SenderId = table.Column<int>(type: "integer", nullable: false),
                    ApprovedByIds = table.Column<string>(type: "text", nullable: false),
                    HasBeenApprovedByIds = table.Column<string>(type: "text", nullable: false),
                    ApprovedDates = table.Column<string>(type: "text", nullable: false),
                    DocumentIds = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GatePassNodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GatePassNodes_GatePasses_GatePassId",
                        column: x => x.GatePassId,
                        principalTable: "GatePasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LeaveRequestNodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LeaveRequestWorkflowId = table.Column<int>(type: "integer", nullable: false),
                    StepType = table.Column<int>(type: "integer", nullable: false),
                    MainId = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    SenderId = table.Column<int>(type: "integer", nullable: false),
                    ApprovedByIds = table.Column<string>(type: "text", nullable: false),
                    HasBeenApprovedByIds = table.Column<string>(type: "text", nullable: false),
                    ApprovedDates = table.Column<string>(type: "text", nullable: false),
                    DocumentIds = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveRequestNodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeaveRequestNodes_LeaveRequestWorkflows_LeaveRequestWorkflo~",
                        column: x => x.LeaveRequestWorkflowId,
                        principalTable: "LeaveRequestWorkflows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GatePassNodes_GatePassId",
                table: "GatePassNodes",
                column: "GatePassId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequestNodes_LeaveRequestWorkflowId",
                table: "LeaveRequestNodes",
                column: "LeaveRequestWorkflowId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GatePassNodes");

            migrationBuilder.DropTable(
                name: "LeaveRequestNodes");

            migrationBuilder.DropTable(
                name: "GatePasses");

            migrationBuilder.DropTable(
                name: "LeaveRequestWorkflows");

            migrationBuilder.DropColumn(
                name: "CompanyInfo_AnnualLeaveTotalDays",
                table: "Employees");

            migrationBuilder.CreateTable(
                name: "GeneralWorkflows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ApprovalWorkflowNodesIds = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    MainId = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    GeneralWorkflowInfo_ApprovedByIds = table.Column<int[]>(type: "integer[]", nullable: false),
                    GeneralWorkflowInfo_Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    GeneralWorkflowInfo_DraftedByIds = table.Column<int[]>(type: "integer[]", nullable: false),
                    GeneralWorkflowInfo_Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    GeneralWorkflowInfo_Quota = table.Column<int>(type: "integer", nullable: true),
                    GeneralWorkflowInfo_Status = table.Column<int>(type: "integer", nullable: false),
                    GeneralWorkflowInfo_WorkflowLogic = table.Column<int>(type: "integer", nullable: false)
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
                    GeneralWorkflowId = table.Column<int>(type: "integer", nullable: false),
                    ApprovalCommentIds = table.Column<string>(type: "text", nullable: false),
                    ApprovalComments = table.Column<string>(type: "text", nullable: true),
                    ApprovalDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    DocumentIds = table.Column<string>(type: "text", nullable: false),
                    MainId = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: true),
                    ReceiverIds = table.Column<string>(type: "text", nullable: false),
                    ReceiverMessages = table.Column<string>(type: "text", nullable: true),
                    ReceiverNames = table.Column<string>(type: "text", nullable: true),
                    SenderId = table.Column<int>(type: "integer", nullable: false),
                    SenderMessage = table.Column<string>(type: "text", nullable: true),
                    SenderName = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "ApprovalWorkflowNodeDocuments",
                columns: table => new
                {
                    ApprovalWorkflowNodeId = table.Column<int>(type: "integer", nullable: false),
                    DocumentsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApprovalWorkflowNodeDocuments", x => new { x.ApprovalWorkflowNodeId, x.DocumentsId });
                    table.ForeignKey(
                        name: "FK_ApprovalWorkflowNodeDocuments_ApprovalWorkflowNodes_Approva~",
                        column: x => x.ApprovalWorkflowNodeId,
                        principalTable: "ApprovalWorkflowNodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApprovalWorkflowNodeDocuments_Documents_DocumentsId",
                        column: x => x.DocumentsId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalWorkflowNodeDocuments_DocumentsId",
                table: "ApprovalWorkflowNodeDocuments",
                column: "DocumentsId");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalWorkflowNodes_GeneralWorkflowId",
                table: "ApprovalWorkflowNodes",
                column: "GeneralWorkflowId");
        }
    }
}
