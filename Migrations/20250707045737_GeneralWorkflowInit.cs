using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace portal_techgel_api.Migrations
{
    /// <inheritdoc />
    public partial class GeneralWorkflowInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DeliveredViaEmail",
                table: "Notifications",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DeliveredViaSMS",
                table: "Notifications",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DeliveredViaSignalR",
                table: "Notifications",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "DeliveryChannels",
                table: "Notifications",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsSent",
                table: "Notifications",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LastErrorMessage",
                table: "Notifications",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ScheduledAt",
                table: "Notifications",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TriggerEntity",
                table: "Notifications",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TriggerEntityId",
                table: "Notifications",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConditionExpression",
                table: "NotificationCategories",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DelaySeconds",
                table: "NotificationCategories",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeliveryChannels",
                table: "NotificationCategories",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MessageTemplate",
                table: "NotificationCategories",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TargetExpression",
                table: "NotificationCategories",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TargetType",
                table: "NotificationCategories",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TitleTemplate",
                table: "NotificationCategories",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TriggerEvent",
                table: "NotificationCategories",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "GeneralProposalWorkflows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    About = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Comment = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    ProjectName = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Reason = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Proposal = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    ApproverId = table.Column<int>(type: "integer", nullable: false),
                    MainId = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    SenderId = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    RejectReason = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralProposalWorkflows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneralProposalWorkflows_Employees_ApproverId",
                        column: x => x.ApproverId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GeneralProposalWorkflows_Employees_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GeneralProposalNodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StepType = table.Column<string>(type: "text", nullable: false),
                    MainId = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    WorkflowId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralProposalNodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneralProposalNodes_GeneralProposalWorkflows_WorkflowId",
                        column: x => x.WorkflowId,
                        principalTable: "GeneralProposalWorkflows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GeneralProposalNodes_WorkflowId",
                table: "GeneralProposalNodes",
                column: "WorkflowId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralProposalWorkflows_ApproverId",
                table: "GeneralProposalWorkflows",
                column: "ApproverId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralProposalWorkflows_SenderId",
                table: "GeneralProposalWorkflows",
                column: "SenderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeneralProposalNodes");

            migrationBuilder.DropTable(
                name: "GeneralProposalWorkflows");

            migrationBuilder.DropColumn(
                name: "DeliveredViaEmail",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "DeliveredViaSMS",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "DeliveredViaSignalR",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "DeliveryChannels",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "IsSent",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "LastErrorMessage",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "ScheduledAt",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "TriggerEntity",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "TriggerEntityId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "ConditionExpression",
                table: "NotificationCategories");

            migrationBuilder.DropColumn(
                name: "DelaySeconds",
                table: "NotificationCategories");

            migrationBuilder.DropColumn(
                name: "DeliveryChannels",
                table: "NotificationCategories");

            migrationBuilder.DropColumn(
                name: "MessageTemplate",
                table: "NotificationCategories");

            migrationBuilder.DropColumn(
                name: "TargetExpression",
                table: "NotificationCategories");

            migrationBuilder.DropColumn(
                name: "TargetType",
                table: "NotificationCategories");

            migrationBuilder.DropColumn(
                name: "TitleTemplate",
                table: "NotificationCategories");

            migrationBuilder.DropColumn(
                name: "TriggerEvent",
                table: "NotificationCategories");
        }
    }
}
