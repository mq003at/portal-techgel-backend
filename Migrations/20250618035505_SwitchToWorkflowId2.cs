using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace portal_techgel_api.Migrations
{
    /// <inheritdoc />
    public partial class SwitchToWorkflowId2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentAssociations_LeaveRequestWorkflows_LeaveRequestWork~",
                table: "DocumentAssociations");

            migrationBuilder.DropIndex(
                name: "IX_DocumentAssociations_LeaveRequestWorkflowId",
                table: "DocumentAssociations");

            migrationBuilder.DropColumn(
                name: "LeaveRequestWorkflowId",
                table: "DocumentAssociations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LeaveRequestWorkflowId",
                table: "DocumentAssociations",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentAssociations_LeaveRequestWorkflowId",
                table: "DocumentAssociations",
                column: "LeaveRequestWorkflowId");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentAssociations_LeaveRequestWorkflows_LeaveRequestWork~",
                table: "DocumentAssociations",
                column: "LeaveRequestWorkflowId",
                principalTable: "LeaveRequestWorkflows",
                principalColumn: "Id");
        }
    }
}
