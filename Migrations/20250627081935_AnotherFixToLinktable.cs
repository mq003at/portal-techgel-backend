using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace portal_techgel_api.Migrations
{
    /// <inheritdoc />
    public partial class AnotherFixToLinktable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.CreateTable(
                name: "OrganizationEntityEmployees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrganizationEntityId = table.Column<int>(type: "integer", nullable: false),
                    EmployeeId = table.Column<int>(type: "integer", nullable: false),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Marks the primary association of this employee to the org entity."),
                    OrganizationRelationType = table.Column<string>(type: "text", nullable: false),
                    MainId = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationEntityEmployees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganizationEntityEmployees_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrganizationEntityEmployees_OrganizationEntities_Organizati~",
                        column: x => x.OrganizationEntityId,
                        principalTable: "OrganizationEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });



            migrationBuilder.CreateIndex(
                name: "IX_OrganizationEntityEmployees_EmployeeId",
                table: "OrganizationEntityEmployees",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationEntityEmployees_OrganizationEntityId_EmployeeId",
                table: "OrganizationEntityEmployees",
                columns: new[] { "OrganizationEntityId", "EmployeeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationEntityEmployees_OrganizationEntityId_IsPrimary",
                table: "OrganizationEntityEmployees",
                columns: new[] { "OrganizationEntityId", "IsPrimary" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CareerPathInfos");

            migrationBuilder.DropTable(
                name: "CompanyInfo");

            migrationBuilder.DropTable(
                name: "DocumentAssociations");

            migrationBuilder.DropTable(
                name: "DocumentSignatures");

            migrationBuilder.DropTable(
                name: "EmergencyContactInfo");

            migrationBuilder.DropTable(
                name: "EmployeeQualificationInfo");

            migrationBuilder.DropTable(
                name: "GatePassNodes");

            migrationBuilder.DropTable(
                name: "InsuranceInfo");

            migrationBuilder.DropTable(
                name: "LeaveRequestNodes");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "OnlyForOrganizationEntities");

            migrationBuilder.DropTable(
                name: "OrganizationEntityEmployees");

            migrationBuilder.DropTable(
                name: "PersonalInfo");

            migrationBuilder.DropTable(
                name: "ScheduleInfo");

            migrationBuilder.DropTable(
                name: "Signatures");

            migrationBuilder.DropTable(
                name: "TaxInfo");

            migrationBuilder.DropTable(
                name: "WorkflowNodeParticipants");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "GatePassWorkflows");

            migrationBuilder.DropTable(
                name: "LeaveRequestWorkflows");

            migrationBuilder.DropTable(
                name: "NotificationCategories");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "OrganizationEntities");
        }
    }
}
