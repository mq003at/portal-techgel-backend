using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace portal_techgel_api.Migrations
{
    /// <inheritdoc />
    public partial class IsDocGenerated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDocumentGenerated",
                table: "LeaveRequestWorkflows",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDocumentGenerated",
                table: "GeneralProposalWorkflows",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDocumentGenerated",
                table: "GatePassWorkflows",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDocumentGenerated",
                table: "LeaveRequestWorkflows");

            migrationBuilder.DropColumn(
                name: "IsDocumentGenerated",
                table: "GeneralProposalWorkflows");

            migrationBuilder.DropColumn(
                name: "IsDocumentGenerated",
                table: "GatePassWorkflows");
        }
    }
}
