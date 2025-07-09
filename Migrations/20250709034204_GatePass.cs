using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace portal_techgel_api.Migrations
{
    /// <inheritdoc />
    public partial class GatePass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GatePassWorkflows_Employees_EmployeeId",
                table: "GatePassWorkflows"
            );

            migrationBuilder.DropColumn(name: "EmployeeId", table: "GatePassWorkflows");

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "LeaveRequestWorkflows",
                type: "text",
                nullable: true
            );

            migrationBuilder.AddColumn<int>(
                name: "NodeApprovalLogic",
                table: "LeaveRequestNodes",
                type: "integer",
                nullable: false,
                defaultValue: 0
            );

            migrationBuilder.AddColumn<int>(
                name: "NodeApprovalLogic",
                table: "GeneralProposalNodes",
                type: "integer",
                nullable: false,
                defaultValue: 0
            );

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "GatePassWorkflows",
                type: "text",
                nullable: true
            );

            migrationBuilder.AddColumn<int>(
                name: "NodeApprovalLogic",
                table: "GatePassNodes",
                type: "integer",
                nullable: false,
                defaultValue: 0
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "Comment", table: "LeaveRequestWorkflows");

            migrationBuilder.DropColumn(name: "NodeApprovalLogic", table: "LeaveRequestNodes");

            migrationBuilder.DropColumn(name: "NodeApprovalLogic", table: "GeneralProposalNodes");

            migrationBuilder.DropColumn(name: "Comment", table: "GatePassWorkflows");

            migrationBuilder.DropColumn(name: "NodeApprovalLogic", table: "GatePassNodes");

            migrationBuilder.AddColumn<int>(
                name: "WorkflowNodeStepType",
                table: "WorkflowNodeParticipants",
                type: "integer",
                nullable: false,
                defaultValue: 0
            );

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "GatePassWorkflows",
                type: "integer",
                nullable: false,
                defaultValue: 0
            );

            migrationBuilder.CreateIndex(
                name: "IX_GatePassWorkflows_EmployeeId",
                table: "GatePassWorkflows",
                column: "EmployeeId"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_GatePassWorkflows_Employees_EmployeeId",
                table: "GatePassWorkflows",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict
            );
        }
    }
}
