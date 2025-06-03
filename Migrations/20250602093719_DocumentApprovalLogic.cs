using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace portal_techgel_api.Migrations
{
    /// <inheritdoc />
    public partial class DocumentApprovalLogic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LegalDocumentInfo_DocumentApprovalLogic",
                table: "Documents",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "LegalDocumentInfo_DraftByIds",
                table: "Documents",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LegalDocumentInfo_HaveApprovedByIds",
                table: "Documents",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LegalDocumentInfo_InspectionByIds",
                table: "Documents",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LegalDocumentInfo_PublishByIds",
                table: "Documents",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LegalDocumentInfo_RequestApprovalByIds",
                table: "Documents",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LegalDocumentInfo_DocumentApprovalLogic",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "LegalDocumentInfo_DraftByIds",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "LegalDocumentInfo_HaveApprovedByIds",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "LegalDocumentInfo_InspectionByIds",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "LegalDocumentInfo_PublishByIds",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "LegalDocumentInfo_RequestApprovalByIds",
                table: "Documents");
        }
    }
}
