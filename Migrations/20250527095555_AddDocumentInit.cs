using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace portal_techgel_api.Migrations
{
    /// <inheritdoc />
    public partial class AddDocumentInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GeneralDocumentInfo_Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    GeneralDocumentInfo_Type = table.Column<int>(type: "integer", nullable: false),
                    GeneralDocumentInfo_Status = table.Column<int>(type: "integer", nullable: false),
                    GeneralDocumentInfo_SubType = table.Column<int>(type: "integer", nullable: false),
                    GeneralDocumentInfo_Category = table.Column<int>(type: "integer", nullable: false),
                    GeneralDocumentInfo_OwnerId = table.Column<int>(type: "integer", nullable: false),
                    GeneralDocumentInfo_OwnerName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    GeneralDocumentInfo_OrganizationEntityResponsibleId = table.Column<int>(type: "integer", nullable: false),
                    GeneralDocumentInfo_OrganizationEntityResponsibleName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    GeneralDocumentInfo_Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false, defaultValue: ""),
                    GeneralDocumentInfo_Url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false, defaultValue: ""),
                    GeneralDocumentInfo_Version = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: ""),
                    LegalDocumentInfo_DraftDate = table.Column<DateTime>(type: "date", nullable: true),
                    LegalDocumentInfo_PublishDate = table.Column<DateTime>(type: "date", nullable: true),
                    LegalDocumentInfo_EffectiveDate = table.Column<DateTime>(type: "date", nullable: true),
                    LegalDocumentInfo_ExpiredDate = table.Column<DateTime>(type: "date", nullable: true),
                    LegalDocumentInfo_FinalApprovalDate = table.Column<DateTime>(type: "date", nullable: true),
                    LegalDocumentInfo_InspectionDate = table.Column<DateTime>(type: "date", nullable: true),
                    LegalDocumentInfo_IsLegalDocument = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    MainId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Documents");
        }
    }
}
