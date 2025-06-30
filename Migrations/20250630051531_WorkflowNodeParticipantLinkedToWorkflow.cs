using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace portal_techgel_api.Migrations
{
    /// <inheritdoc />
    public partial class WorkflowNodeParticipantLinkedToWorkflow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "WorkflowNodeParticipants",
                type: "integer",
                nullable: false,
                defaultValue: 0
            );

            migrationBuilder.AddColumn<int>(
                name: "SenderId",
                table: "GatePassWorkflows",
                type: "integer",
                nullable: false,
                defaultValue: 0
            );

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequestWorkflows_SenderId",
                table: "LeaveRequestWorkflows",
                column: "SenderId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_GatePassWorkflows_SenderId",
                table: "GatePassWorkflows",
                column: "SenderId"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_GatePassWorkflows_Employees_SenderId",
                table: "GatePassWorkflows",
                column: "SenderId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveRequestWorkflows_Employees_EmployeeId",
                table: "LeaveRequestWorkflows",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveRequestWorkflows_Employees_SenderId",
                table: "LeaveRequestWorkflows",
                column: "SenderId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GatePassWorkflows_Employees_SenderId",
                table: "GatePassWorkflows"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_LeaveRequestWorkflows_Employees_EmployeeId",
                table: "LeaveRequestWorkflows"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_LeaveRequestWorkflows_Employees_SenderId",
                table: "LeaveRequestWorkflows"
            );

            migrationBuilder.DropIndex(
                name: "IX_LeaveRequestWorkflows_SenderId",
                table: "LeaveRequestWorkflows"
            );

            migrationBuilder.DropIndex(
                name: "IX_GatePassWorkflows_SenderId",
                table: "GatePassWorkflows"
            );

            migrationBuilder.DropColumn(name: "WorkflowId", table: "WorkflowNodeParticipants");

            migrationBuilder.DropColumn(name: "ChildrenIds", table: "OrganizationEntities");

            migrationBuilder.DropColumn(name: "SenderId", table: "LeaveRequestWorkflows");

            migrationBuilder.DropColumn(name: "SenderId", table: "GatePassWorkflows");

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveRequestWorkflows_Employees_EmployeeId",
                table: "LeaveRequestWorkflows",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict
            );
        }
    }
}
