using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace portal_techgel_api.Migrations
{
    /// <inheritdoc />
    public partial class stringstatusall : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasApproved",
                table: "WorkflowNodeParticipants");

            migrationBuilder.DropColumn(
                name: "HasRejected",
                table: "WorkflowNodeParticipants");

            migrationBuilder.AddColumn<string>(
                name: "ApprovalStatus",
                table: "WorkflowNodeParticipants",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "LeaveRequestWorkflows",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "LeaveRequestNodes",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovalStatus",
                table: "WorkflowNodeParticipants");

            migrationBuilder.AddColumn<bool>(
                name: "HasApproved",
                table: "WorkflowNodeParticipants",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasRejected",
                table: "WorkflowNodeParticipants",
                type: "boolean",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "LeaveRequestWorkflows",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "LeaveRequestNodes",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
