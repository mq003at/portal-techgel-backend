using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace portal_techgel_api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    MiddleName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Avatar = table.Column<string>(type: "text", nullable: true),
                    PersonalInfo_Gender = table.Column<int>(type: "integer", nullable: false),
                    PersonalInfo_Address = table.Column<string>(type: "text", nullable: false),
                    PersonalInfo_DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PersonalInfo_MaritalStatus = table.Column<int>(type: "integer", nullable: false),
                    PersonalInfo_Nationality = table.Column<string>(type: "text", nullable: false),
                    PersonalInfo_PersonalEmail = table.Column<string>(type: "text", nullable: true),
                    PersonalInfo_PersonalPhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PersonalInfo_IdCardNumber = table.Column<string>(type: "text", nullable: true),
                    PersonalInfo_IdCardIssueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PersonalInfo_IdCardExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CompanyInfo_CompanyEmail = table.Column<string>(type: "text", nullable: true),
                    CompanyInfo_CompanyPhoneNumber = table.Column<string>(type: "text", nullable: true),
                    CompanyInfo_EmploymentStatus = table.Column<int>(type: "integer", nullable: false),
                    CompanyInfo_Position = table.Column<string>(type: "text", nullable: true),
                    CompanyInfo_Department = table.Column<string>(type: "text", nullable: true),
                    CompanyInfo_StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CompanyInfo_EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CompanyInfo_ProbationStartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CompanyInfo_ProbationEndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CareerPathInfo_Degrees = table.Column<List<string>>(type: "text[]", nullable: true),
                    CareerPathInfo_Certifications = table.Column<List<string>>(type: "text[]", nullable: true),
                    CareerPathInfo_Specializations = table.Column<List<string>>(type: "text[]", nullable: true),
                    TaxInfo_TaxId = table.Column<string>(type: "text", nullable: true),
                    TaxInfo_TaxStatus = table.Column<string>(type: "text", nullable: true),
                    TaxInfo_Region = table.Column<string>(type: "text", nullable: true),
                    InsuranceInfo_InsuranceNumber = table.Column<string>(type: "text", nullable: true),
                    InsuranceInfo_Provider = table.Column<string>(type: "text", nullable: true),
                    InsuranceInfo_EffectiveDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    InsuranceInfo_ExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EmergencyContactInfo_Name = table.Column<string>(type: "text", nullable: true),
                    EmergencyContactInfo_Phone = table.Column<string>(type: "text", nullable: true),
                    EmergencyContactInfo_Relationship = table.Column<string>(type: "text", nullable: true),
                    EmergencyContactInfo_CurrentAddress = table.Column<string>(type: "text", nullable: true),
                    EmergencyContactInfo_PermanentAddress = table.Column<string>(type: "text", nullable: true),
                    ScheduleInfo_WorkSchedule = table.Column<string>(type: "text", nullable: true),
                    ScheduleInfo_IsRemoteStatus = table.Column<bool>(type: "boolean", nullable: true),
                    ScheduleInfo_ShiftType = table.Column<string>(type: "text", nullable: true),
                    MainID = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: true),
                    ParentId = table.Column<int>(type: "integer", nullable: true),
                    ChildrenIds = table.Column<int[]>(type: "integer[]", nullable: true),
                    ManagerId = table.Column<int>(type: "integer", nullable: true),
                    MainID = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganizationEntities_Employees_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrganizationEntities_OrganizationEntities_ParentId",
                        column: x => x.ParentId,
                        principalTable: "OrganizationEntities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EmployeeRoleDetails",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "integer", nullable: false),
                    OrganizationEntityId = table.Column<int>(type: "integer", nullable: false),
                    ManagesOrganizationEntityId = table.Column<int>(type: "integer", nullable: true),
                    SubordinateId = table.Column<int>(type: "integer", nullable: true),
                    GroupId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeRoleDetails", x => new { x.EmployeeId, x.OrganizationEntityId });
                    table.ForeignKey(
                        name: "FK_EmployeeRoleDetails_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeRoleDetails_Employees_SubordinateId",
                        column: x => x.SubordinateId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmployeeRoleDetails_OrganizationEntities_ManagesOrganizatio~",
                        column: x => x.ManagesOrganizationEntityId,
                        principalTable: "OrganizationEntities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmployeeRoleDetails_OrganizationEntities_OrganizationEntity~",
                        column: x => x.OrganizationEntityId,
                        principalTable: "OrganizationEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationEntityEmployees",
                columns: table => new
                {
                    OrganizationEntityId = table.Column<int>(type: "integer", nullable: false),
                    EmployeeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationEntityEmployees", x => new { x.OrganizationEntityId, x.EmployeeId });
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
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeRoleDetails_ManagesOrganizationEntityId",
                table: "EmployeeRoleDetails",
                column: "ManagesOrganizationEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeRoleDetails_OrganizationEntityId",
                table: "EmployeeRoleDetails",
                column: "OrganizationEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeRoleDetails_SubordinateId",
                table: "EmployeeRoleDetails",
                column: "SubordinateId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationEntities_ManagerId",
                table: "OrganizationEntities",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationEntities_ParentId",
                table: "OrganizationEntities",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationEntityEmployees_EmployeeId",
                table: "OrganizationEntityEmployees",
                column: "EmployeeId");
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
                name: "EmployeeRoleDetails");

            migrationBuilder.DropTable(
                name: "OrganizationEntityEmployees");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "OrganizationEntities");

            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
