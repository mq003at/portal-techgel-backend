using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace portal_techgel_api.Migrations
{
    /// <inheritdoc />
    public partial class createUpdateAtFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.CreateTable(
                name: "NotificationCategories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    IsUrgentByDefault = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationCategories", x => x.Id);
                });

           migrationBuilder.CreateTable(
                name: "GatePassWorkflows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmployeeId = table.Column<int>(type: "integer", nullable: false),
                    Reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    GatePassStartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GatePassEndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MainId = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    RejectReason = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GatePassWorkflows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GatePassWorkflows_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

          
           
            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmployeeId = table.Column<int>(type: "integer", nullable: false),
                    NotificationCategoryId = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Message = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    Url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ReadAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FinishedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UrgencyLevel = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    MainId = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notifications_NotificationCategories_NotificationCategoryId",
                        column: x => x.NotificationCategoryId,
                        principalTable: "NotificationCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GatePassNodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StepType = table.Column<string>(type: "text", nullable: false),
                    MainId = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    WorkflowId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GatePassNodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GatePassNodes_GatePassWorkflows_WorkflowId",
                        column: x => x.WorkflowId,
                        principalTable: "GatePassWorkflows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

           
            migrationBuilder.CreateTable(
                name: "OnlyForOrganizationEntities",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NotificationCategoryId = table.Column<long>(type: "bigint", nullable: false),
                    OrganizationEntityId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnlyForOrganizationEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OnlyForOrganizationEntities_NotificationCategories_Notifica~",
                        column: x => x.NotificationCategoryId,
                        principalTable: "NotificationCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OnlyForOrganizationEntities_OrganizationEntities_Organizati~",
                        column: x => x.OrganizationEntityId,
                        principalTable: "OrganizationEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

           
             migrationBuilder.CreateIndex(
                name: "IX_Notifications_EmployeeId",
                table: "Notifications",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_NotificationCategoryId",
                table: "Notifications",
                column: "NotificationCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_OnlyForOrganizationEntities_NotificationCategoryId_Organiza~",
                table: "OnlyForOrganizationEntities",
                columns: new[] { "NotificationCategoryId", "OrganizationEntityId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OnlyForOrganizationEntities_OrganizationEntityId",
                table: "OnlyForOrganizationEntities",
                column: "OrganizationEntityId");

            
        

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
                name: "OrganizationEntities");

            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
