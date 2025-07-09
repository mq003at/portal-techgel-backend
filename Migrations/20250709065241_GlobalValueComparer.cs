using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace portal_techgel_api.Migrations
{
    /// <inheritdoc />
    public partial class GlobalValueComparer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Division",
                table: "Documents",
                newName: "Location");

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "LeaveRequestWorkflows",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ParticipantIds",
                table: "LeaveRequestWorkflows",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ParticipantIds",
                table: "GeneralProposalWorkflows",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "GatePassWorkflows",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ParticipantIds",
                table: "GatePassWorkflows",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "DownloadCount",
                table: "Documents",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "RestrictedEmployeeIds",
                table: "Documents",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParticipantIds",
                table: "LeaveRequestWorkflows");

            migrationBuilder.DropColumn(
                name: "ParticipantIds",
                table: "GeneralProposalWorkflows");

            migrationBuilder.DropColumn(
                name: "ParticipantIds",
                table: "GatePassWorkflows");

            migrationBuilder.DropColumn(
                name: "DownloadCount",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "RestrictedEmployeeIds",
                table: "Documents");

            migrationBuilder.RenameColumn(
                name: "Location",
                table: "Documents",
                newName: "Division");

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "LeaveRequestWorkflows",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "GatePassWorkflows",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000,
                oldNullable: true);
        }
    }
}
