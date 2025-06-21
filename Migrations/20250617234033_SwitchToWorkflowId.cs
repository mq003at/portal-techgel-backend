using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace portal_techgel_api.Migrations
{
    /// <inheritdoc />
    public partial class SwitchToWorkflowId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.CreateTable(
            //     name: "AspNetRoles",
            //     columns: table => new
            //     {
            //         Id = table.Column<string>(type: "text", nullable: false),
            //         Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
            //         NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
            //         ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_AspNetRoles", x => x.Id);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "AspNetUsers",
            //     columns: table => new
            //     {
            //         Id = table.Column<string>(type: "text", nullable: false),
            //         UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
            //         NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
            //         Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
            //         NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
            //         EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
            //         PasswordHash = table.Column<string>(type: "text", nullable: true),
            //         SecurityStamp = table.Column<string>(type: "text", nullable: true),
            //         ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
            //         PhoneNumber = table.Column<string>(type: "text", nullable: true),
            //         PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
            //         TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
            //         LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
            //         LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
            //         AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_AspNetUsers", x => x.Id);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "Documents",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
            //         Status = table.Column<string>(type: "text", nullable: false),
            //         Category = table.Column<string>(type: "text", nullable: false),
            //         Division = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
            //         FileExtension = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
            //         SizeInBytes = table.Column<long>(type: "bigint", nullable: false),
            //         TemplateKey = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
            //         Tag = table.Column<string>(type: "text", nullable: false),
            //         Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
            //         Url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
            //         Version = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
            //         MainId = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
            //         CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
            //         UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Documents", x => x.Id);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "Employees",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
            //         MiddleName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
            //         LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
            //         Avatar = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
            //         Password = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
            //         PersonalInfo_Gender = table.Column<int>(type: "integer", nullable: false),
            //         PersonalInfo_Address = table.Column<string>(type: "text", nullable: false),
            //         PersonalInfo_DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //         PersonalInfo_MaritalStatus = table.Column<int>(type: "integer", nullable: false),
            //         PersonalInfo_Nationality = table.Column<string>(type: "text", nullable: false),
            //         PersonalInfo_Birthplace = table.Column<string>(type: "text", nullable: false),
            //         PersonalInfo_EthnicGroup = table.Column<string>(type: "text", nullable: false),
            //         PersonalInfo_PersonalEmail = table.Column<string>(type: "text", nullable: true),
            //         PersonalInfo_PersonalPhoneNumber = table.Column<string>(type: "text", nullable: true),
            //         PersonalInfo_IdCardNumber = table.Column<string>(type: "text", nullable: true),
            //         PersonalInfo_IdCardIssueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
            //         PersonalInfo_IdCardExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
            //         PersonalInfo_IdCardIssuePlace = table.Column<string>(type: "text", nullable: true),
            //         CompanyInfo_CompanyEmail = table.Column<string>(type: "text", nullable: true),
            //         CompanyInfo_CompanyPhoneNumber = table.Column<string>(type: "text", nullable: true),
            //         CompanyInfo_EmploymentStatus = table.Column<int>(type: "integer", nullable: false),
            //         CompanyInfo_Position = table.Column<string>(type: "text", nullable: true),
            //         CompanyInfo_Department = table.Column<string>(type: "text", nullable: true),
            //         CompanyInfo_StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
            //         CompanyInfo_EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
            //         CompanyInfo_ProbationStartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
            //         CompanyInfo_ProbationEndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
            //         CompanyInfo_AnnualLeaveTotalDays = table.Column<double>(type: "real", nullable: false),
            //         CareerPathInfo_Degrees = table.Column<List<string>>(type: "text[]", nullable: true),
            //         CareerPathInfo_Certifications = table.Column<List<string>>(type: "text[]", nullable: true),
            //         CareerPathInfo_Specializations = table.Column<List<string>>(type: "text[]", nullable: true),
            //         TaxInfo_TaxId = table.Column<string>(type: "text", nullable: true),
            //         TaxInfo_TaxStatus = table.Column<string>(type: "text", nullable: true),
            //         TaxInfo_Region = table.Column<string>(type: "text", nullable: true),
            //         InsuranceInfo_InsuranceNumber = table.Column<string>(type: "text", nullable: true),
            //         InsuranceInfo_Provider = table.Column<string>(type: "text", nullable: true),
            //         InsuranceInfo_EffectiveDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
            //         InsuranceInfo_ExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
            //         EmergencyContactInfo_Name = table.Column<string>(type: "text", nullable: true),
            //         EmergencyContactInfo_Phone = table.Column<string>(type: "text", nullable: true),
            //         EmergencyContactInfo_Relationship = table.Column<string>(type: "text", nullable: true),
            //         EmergencyContactInfo_CurrentAddress = table.Column<string>(type: "text", nullable: true),
            //         EmergencyContactInfo_PermanentAddress = table.Column<string>(type: "text", nullable: true),
            //         ScheduleInfo_WorkSchedule = table.Column<string>(type: "text", nullable: true),
            //         ScheduleInfo_IsRemoteStatus = table.Column<bool>(type: "boolean", nullable: true),
            //         ScheduleInfo_ShiftType = table.Column<string>(type: "text", nullable: true),
            //         SupervisorId = table.Column<int>(type: "integer", nullable: true),
            //         SubordinateIds = table.Column<List<int>>(type: "integer[]", nullable: false),
            //         ManagedOrganizationEntityIds = table.Column<List<int>>(type: "integer[]", nullable: false),
            //         GroupId = table.Column<int>(type: "integer", nullable: true),
            //         MainId = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
            //         CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
            //         UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Employees", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_Employees_Employees_SupervisorId",
            //             column: x => x.SupervisorId,
            //             principalTable: "Employees",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Restrict);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "LeaveRequestWorkflows",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         Reason = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
            //         LeaveAprrovalCategory = table.Column<int>(type: "integer", nullable: false),
            //         StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //         StartDateDayNightType = table.Column<int>(type: "integer", nullable: false),
            //         EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //         EndDateDayNightType = table.Column<int>(type: "integer", nullable: false),
            //         TotalDays = table.Column<double>(type: "real", nullable: false),
            //         EmployeeAnnualLeaveTotalDays = table.Column<double>(type: "real", nullable: false),
            //         FinalEmployeeAnnualLeaveTotalDays = table.Column<double>(type: "real", nullable: false),
            //         MainId = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
            //         CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
            //         UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
            //         Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
            //         Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
            //         Status = table.Column<int>(type: "integer", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_LeaveRequestWorkflows", x => x.Id);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "AspNetRoleClaims",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         RoleId = table.Column<string>(type: "text", nullable: false),
            //         ClaimType = table.Column<string>(type: "text", nullable: true),
            //         ClaimValue = table.Column<string>(type: "text", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
            //             column: x => x.RoleId,
            //             principalTable: "AspNetRoles",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "AspNetUserClaims",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         UserId = table.Column<string>(type: "text", nullable: false),
            //         ClaimType = table.Column<string>(type: "text", nullable: true),
            //         ClaimValue = table.Column<string>(type: "text", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_AspNetUserClaims_AspNetUsers_UserId",
            //             column: x => x.UserId,
            //             principalTable: "AspNetUsers",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "AspNetUserLogins",
            //     columns: table => new
            //     {
            //         LoginProvider = table.Column<string>(type: "text", nullable: false),
            //         ProviderKey = table.Column<string>(type: "text", nullable: false),
            //         ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
            //         UserId = table.Column<string>(type: "text", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
            //         table.ForeignKey(
            //             name: "FK_AspNetUserLogins_AspNetUsers_UserId",
            //             column: x => x.UserId,
            //             principalTable: "AspNetUsers",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "AspNetUserRoles",
            //     columns: table => new
            //     {
            //         UserId = table.Column<string>(type: "text", nullable: false),
            //         RoleId = table.Column<string>(type: "text", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
            //         table.ForeignKey(
            //             name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
            //             column: x => x.RoleId,
            //             principalTable: "AspNetRoles",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //         table.ForeignKey(
            //             name: "FK_AspNetUserRoles_AspNetUsers_UserId",
            //             column: x => x.UserId,
            //             principalTable: "AspNetUsers",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "AspNetUserTokens",
            //     columns: table => new
            //     {
            //         UserId = table.Column<string>(type: "text", nullable: false),
            //         LoginProvider = table.Column<string>(type: "text", nullable: false),
            //         Name = table.Column<string>(type: "text", nullable: false),
            //         Value = table.Column<string>(type: "text", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
            //         table.ForeignKey(
            //             name: "FK_AspNetUserTokens_AspNetUsers_UserId",
            //             column: x => x.UserId,
            //             principalTable: "AspNetUsers",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "DocumentSignatures",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         DocumentId = table.Column<int>(type: "integer", nullable: false),
            //         EmployeeId = table.Column<int>(type: "integer", nullable: false),
            //         SignedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //         SignatureUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_DocumentSignatures", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_DocumentSignatures_Documents_DocumentId",
            //             column: x => x.DocumentId,
            //             principalTable: "Documents",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //         table.ForeignKey(
            //             name: "FK_DocumentSignatures_Employees_EmployeeId",
            //             column: x => x.EmployeeId,
            //             principalTable: "Employees",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Restrict);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "OrganizationEntities",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         Level = table.Column<int>(type: "integer", nullable: false),
            //         Name = table.Column<string>(type: "text", nullable: false),
            //         Description = table.Column<string>(type: "text", nullable: false),
            //         Status = table.Column<int>(type: "integer", nullable: false),
            //         SortOrder = table.Column<int>(type: "integer", nullable: true),
            //         ParentId = table.Column<int>(type: "integer", nullable: true),
            //         ChildrenIds = table.Column<List<int>>(type: "integer[]", nullable: true),
            //         ManagerId = table.Column<int>(type: "integer", nullable: true),
            //         MainId = table.Column<string>(type: "text", nullable: false),
            //         CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //         UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_OrganizationEntities", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_OrganizationEntities_Employees_ManagerId",
            //             column: x => x.ManagerId,
            //             principalTable: "Employees",
            //             principalColumn: "Id");
            //         table.ForeignKey(
            //             name: "FK_OrganizationEntities_OrganizationEntities_ParentId",
            //             column: x => x.ParentId,
            //             principalTable: "OrganizationEntities",
            //             principalColumn: "Id");
            //     });

            // migrationBuilder.CreateTable(
            //     name: "Signatures",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         EmployeeId = table.Column<int>(type: "integer", nullable: false),
            //         FileName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
            //         ContentType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
            //         FileSize = table.Column<long>(type: "bigint", nullable: false),
            //         StoragePath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
            //         UploadedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
            //         MainId = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
            //         CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
            //         UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Signatures", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_Signatures_Employees_EmployeeId",
            //             column: x => x.EmployeeId,
            //             principalTable: "Employees",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "LeaveRequestNodes",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         StepType = table.Column<int>(type: "integer", nullable: false),
            //         WorkflowId1 = table.Column<int>(type: "integer", nullable: false),
            //         MainId = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
            //         CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
            //         UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
            //         Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
            //         Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
            //         Status = table.Column<int>(type: "integer", nullable: false),
            //         WorkflowId = table.Column<int>(type: "integer", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_LeaveRequestNodes", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_LeaveRequestNodes_LeaveRequestWorkflows_WorkflowId",
            //             column: x => x.WorkflowId,
            //             principalTable: "LeaveRequestWorkflows",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //         table.ForeignKey(
            //             name: "FK_LeaveRequestNodes_LeaveRequestWorkflows_WorkflowId1",
            //             column: x => x.WorkflowId1,
            //             principalTable: "LeaveRequestWorkflows",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "OrganizationEntityEmployees",
            //     columns: table => new
            //     {
            //         OrganizationEntityId = table.Column<int>(type: "integer", nullable: false),
            //         EmployeeId = table.Column<int>(type: "integer", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_OrganizationEntityEmployees", x => new { x.OrganizationEntityId, x.EmployeeId });
            //         table.ForeignKey(
            //             name: "FK_OrganizationEntityEmployees_Employees_EmployeeId",
            //             column: x => x.EmployeeId,
            //             principalTable: "Employees",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //         table.ForeignKey(
            //             name: "FK_OrganizationEntityEmployees_OrganizationEntities_Organizati~",
            //             column: x => x.OrganizationEntityId,
            //             principalTable: "OrganizationEntities",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "DocumentAssociations",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         DocumentId = table.Column<int>(type: "integer", nullable: false),
            //         EntityType = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
            //         EntityId = table.Column<int>(type: "integer", nullable: false),
            //         LeaveRequestWorkflowId = table.Column<int>(type: "integer", nullable: true),
            //         WorkflowNodeId = table.Column<int>(type: "integer", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_DocumentAssociations", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_DocumentAssociations_Documents_DocumentId",
            //             column: x => x.DocumentId,
            //             principalTable: "Documents",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //         table.ForeignKey(
            //             name: "FK_DocumentAssociations_LeaveRequestNodes_WorkflowNodeId",
            //             column: x => x.WorkflowNodeId,
            //             principalTable: "LeaveRequestNodes",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //         table.ForeignKey(
            //             name: "FK_DocumentAssociations_LeaveRequestWorkflows_LeaveRequestWork~",
            //             column: x => x.LeaveRequestWorkflowId,
            //             principalTable: "LeaveRequestWorkflows",
            //             principalColumn: "Id");
            //     });

            // migrationBuilder.CreateTable(
            //     name: "WorkflowNodeParticipants",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         EmployeeId = table.Column<int>(type: "integer", nullable: false),
            //         WorkflowNodeId = table.Column<int>(type: "integer", nullable: false),
            //         Order = table.Column<int>(type: "integer", nullable: false),
            //         WorkflowNodeStepType = table.Column<int>(type: "integer", nullable: false),
            //         ApprovalDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
            //         ApprovalDeadline = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
            //         HasApproved = table.Column<bool>(type: "boolean", nullable: true),
            //         HasRejected = table.Column<bool>(type: "boolean", nullable: true),
            //         TAT = table.Column<TimeSpan>(type: "interval", nullable: true, defaultValue: new TimeSpan(0, 0, 0, 0, 0)),
            //         WorkflowId = table.Column<int>(type: "integer", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_WorkflowNodeParticipants", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_WorkflowNodeParticipants_Employees_EmployeeId",
            //             column: x => x.EmployeeId,
            //             principalTable: "Employees",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //         table.ForeignKey(
            //             name: "FK_WorkflowNodeParticipants_LeaveRequestNodes_WorkflowNodeId",
            //             column: x => x.WorkflowNodeId,
            //             principalTable: "LeaveRequestNodes",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //         table.ForeignKey(
            //             name: "FK_WorkflowNodeParticipants_LeaveRequestWorkflows_WorkflowId",
            //             column: x => x.WorkflowId,
            //             principalTable: "LeaveRequestWorkflows",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            // migrationBuilder.CreateIndex(
            //     name: "IX_AspNetRoleClaims_RoleId",
            //     table: "AspNetRoleClaims",
            //     column: "RoleId");

            // migrationBuilder.CreateIndex(
            //     name: "RoleNameIndex",
            //     table: "AspNetRoles",
            //     column: "NormalizedName",
            //     unique: true);

            // migrationBuilder.CreateIndex(
            //     name: "IX_AspNetUserClaims_UserId",
            //     table: "AspNetUserClaims",
            //     column: "UserId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_AspNetUserLogins_UserId",
            //     table: "AspNetUserLogins",
            //     column: "UserId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_AspNetUserRoles_RoleId",
            //     table: "AspNetUserRoles",
            //     column: "RoleId");

            // migrationBuilder.CreateIndex(
            //     name: "EmailIndex",
            //     table: "AspNetUsers",
            //     column: "NormalizedEmail");

            // migrationBuilder.CreateIndex(
            //     name: "UserNameIndex",
            //     table: "AspNetUsers",
            //     column: "NormalizedUserName",
            //     unique: true);

            // migrationBuilder.CreateIndex(
            //     name: "IX_DocumentAssociations_DocumentId",
            //     table: "DocumentAssociations",
            //     column: "DocumentId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_DocumentAssociations_LeaveRequestWorkflowId",
            //     table: "DocumentAssociations",
            //     column: "LeaveRequestWorkflowId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_DocumentAssociations_WorkflowNodeId",
            //     table: "DocumentAssociations",
            //     column: "WorkflowNodeId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_DocumentSignatures_DocumentId",
            //     table: "DocumentSignatures",
            //     column: "DocumentId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_DocumentSignatures_EmployeeId",
            //     table: "DocumentSignatures",
            //     column: "EmployeeId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Employees_SupervisorId",
            //     table: "Employees",
            //     column: "SupervisorId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_LeaveRequestNodes_WorkflowId",
            //     table: "LeaveRequestNodes",
            //     column: "WorkflowId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_LeaveRequestNodes_WorkflowId1",
            //     table: "LeaveRequestNodes",
            //     column: "WorkflowId1");

            // migrationBuilder.CreateIndex(
            //     name: "IX_OrganizationEntities_ManagerId",
            //     table: "OrganizationEntities",
            //     column: "ManagerId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_OrganizationEntities_ParentId",
            //     table: "OrganizationEntities",
            //     column: "ParentId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_OrganizationEntityEmployees_EmployeeId",
            //     table: "OrganizationEntityEmployees",
            //     column: "EmployeeId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Signatures_EmployeeId",
            //     table: "Signatures",
            //     column: "EmployeeId",
            //     unique: true);

            // migrationBuilder.CreateIndex(
            //     name: "IX_WorkflowNodeParticipants_EmployeeId",
            //     table: "WorkflowNodeParticipants",
            //     column: "EmployeeId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_WorkflowNodeParticipants_WorkflowId",
            //     table: "WorkflowNodeParticipants",
            //     column: "WorkflowId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_WorkflowNodeParticipants_WorkflowNodeId",
            //     table: "WorkflowNodeParticipants",
            //     column: "WorkflowNodeId");
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
                name: "DocumentAssociations");

            migrationBuilder.DropTable(
                name: "DocumentSignatures");

            migrationBuilder.DropTable(
                name: "OrganizationEntityEmployees");

            migrationBuilder.DropTable(
                name: "Signatures");

            migrationBuilder.DropTable(
                name: "WorkflowNodeParticipants");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "OrganizationEntities");

            migrationBuilder.DropTable(
                name: "LeaveRequestNodes");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "LeaveRequestWorkflows");
        }
    }
}
