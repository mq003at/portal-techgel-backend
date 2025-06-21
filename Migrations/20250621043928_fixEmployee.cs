using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace portal_techgel_api.Migrations
{
    /// <inheritdoc />
    public partial class fixEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationEntities_RoleInfo_RoleInfoId",
                table: "OrganizationEntities");

            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationEntityEmployees_RoleInfo_RoleInfoId",
                table: "OrganizationEntityEmployees");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleInfo_Employees_EmployeeId",
                table: "RoleInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleInfo_Employees_SupervisorId",
                table: "RoleInfo");

            migrationBuilder.DropIndex(
                name: "IX_OrganizationEntities_RoleInfoId",
                table: "OrganizationEntities");

            migrationBuilder.DropColumn(
                name: "SubordinateIds",
                table: "RoleInfo");

            migrationBuilder.DropColumn(
                name: "RoleInfoId",
                table: "OrganizationEntities");

            migrationBuilder.DropColumn(
                name: "CareerPathInfo_Certifications",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "CareerPathInfo_Degrees",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "CareerPathInfo_Specializations",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "CompanyInfo_AnnualLeaveTotalDays",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "CompanyInfo_CompanyEmail",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "CompanyInfo_CompanyPhoneNumber",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "CompanyInfo_CompensatoryLeaveTotalDays",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "CompanyInfo_Department",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "CompanyInfo_EmploymentStatus",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "CompanyInfo_EndDate",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "CompanyInfo_IsOnProbation",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "CompanyInfo_Position",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "CompanyInfo_ProbationEndDate",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "CompanyInfo_ProbationStartDate",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "CompanyInfo_StartDate",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "EmergencyContactInfo_CurrentAddress",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "EmergencyContactInfo_Name",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "EmergencyContactInfo_PermanentAddress",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "EmergencyContactInfo_Phone",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "EmergencyContactInfo_Relationship",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "InsuranceInfo_EffectiveDate",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "InsuranceInfo_ExpiryDate",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "InsuranceInfo_InsuranceNumber",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "InsuranceInfo_Provider",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "PersonalInfo_Address",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "PersonalInfo_Birthplace",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "PersonalInfo_DateOfBirth",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "PersonalInfo_EthnicGroup",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "PersonalInfo_Gender",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "PersonalInfo_IdCardExpiryDate",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "PersonalInfo_IdCardIssueDate",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "PersonalInfo_IdCardIssuePlace",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "PersonalInfo_IdCardNumber",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "PersonalInfo_MaritalStatus",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "PersonalInfo_Nationality",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "PersonalInfo_PersonalEmail",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "PersonalInfo_PersonalPhoneNumber",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ScheduleInfo_IsRemoteStatus",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ScheduleInfo_ShiftType",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ScheduleInfo_WorkSchedule",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "TaxInfo_Region",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "TaxInfo_TaxId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "TaxInfo_TaxStatus",
                table: "Employees");

            migrationBuilder.AlterColumn<int>(
                name: "SupervisorId",
                table: "RoleInfo",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "DeputySupervisorId",
                table: "RoleInfo",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "RoleInfo",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "RoleInfoId",
                table: "Employees",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CareerPathInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmployeeId = table.Column<int>(type: "integer", nullable: false),
                    CurrentPosition = table.Column<string>(type: "text", nullable: true),
                    NextPosition = table.Column<string>(type: "text", nullable: true),
                    ExpectedPromotionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PromotionReason = table.Column<string>(type: "text", nullable: true),
                    DemotionReason = table.Column<string>(type: "text", nullable: true),
                    LastPromotionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastDemotionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CareerPathInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CareerPathInfo_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanyInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    EmployeeId = table.Column<int>(type: "integer", nullable: false),
                    CompanyEmail = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CompanyPhoneNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    EmploymentStatus = table.Column<string>(type: "text", nullable: false),
                    Position = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Department = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ProbationStartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ProbationEndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsOnProbation = table.Column<bool>(type: "boolean", nullable: false),
                    CompensatoryLeaveTotalDays = table.Column<double>(type: "double precision", nullable: false, defaultValue: 0.0),
                    AnnualLeaveTotalDays = table.Column<double>(type: "double precision", nullable: false, defaultValue: 0.0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyInfo_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmergencyContactInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    EmployeeId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Relationship = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CurrentAddress = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmergencyContactInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmergencyContactInfo_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeQualificationInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    EmployeeId = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Institution = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    GraduationOrIssueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ExpirationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CertificateNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    FileUrl = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    Note = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeQualificationInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeQualificationInfo_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InsuranceInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    EmployeeId = table.Column<int>(type: "integer", nullable: false),
                    InsuranceCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RegistrationLocation = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    EffectiveDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TerminationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    InsuranceStatus = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Note = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsuranceInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InsuranceInfo_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonalInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    EmployeeId = table.Column<int>(type: "integer", nullable: false),
                    Gender = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MaritalStatus = table.Column<string>(type: "text", nullable: false),
                    Nationality = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Birthplace = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    EthnicGroup = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PersonalEmail = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    PersonalPhoneNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    IdCardNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    IdCardIssueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IdCardExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IdCardIssuePlace = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonalInfo_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleInfoManagedOrganizationEntities",
                columns: table => new
                {
                    ManagedOrganizationEntitiesId = table.Column<int>(type: "integer", nullable: false),
                    RoleInfoId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleInfoManagedOrganizationEntities", x => new { x.ManagedOrganizationEntitiesId, x.RoleInfoId });
                    table.ForeignKey(
                        name: "FK_RoleInfoManagedOrganizationEntities_OrganizationEntities_Ma~",
                        column: x => x.ManagedOrganizationEntitiesId,
                        principalTable: "OrganizationEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleInfoManagedOrganizationEntities_RoleInfo_RoleInfoId",
                        column: x => x.RoleInfoId,
                        principalTable: "RoleInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    EmployeeId = table.Column<int>(type: "integer", nullable: false),
                    WeekdayStartTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    WeekdayEndTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    WorksOnSaturday = table.Column<bool>(type: "boolean", nullable: false),
                    SaturdayStartTime = table.Column<TimeSpan>(type: "interval", nullable: true),
                    SaturdayEndTime = table.Column<TimeSpan>(type: "interval", nullable: true),
                    IsRemoteEligible = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScheduleInfo_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaxInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    EmployeeId = table.Column<int>(type: "integer", nullable: false),
                    TaxCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    NumberOfDependents = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    IsFamilyStatusRegistered = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Note = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaxInfo_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_RoleInfoId",
                table: "Employees",
                column: "RoleInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_CareerPathInfo_EmployeeId",
                table: "CareerPathInfo",
                column: "EmployeeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyInfo_EmployeeId",
                table: "CompanyInfo",
                column: "EmployeeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmergencyContactInfo_EmployeeId",
                table: "EmergencyContactInfo",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeQualificationInfo_EmployeeId",
                table: "EmployeeQualificationInfo",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_InsuranceInfo_EmployeeId",
                table: "InsuranceInfo",
                column: "EmployeeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonalInfo_EmployeeId",
                table: "PersonalInfo",
                column: "EmployeeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoleInfoManagedOrganizationEntities_RoleInfoId",
                table: "RoleInfoManagedOrganizationEntities",
                column: "RoleInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleInfo_EmployeeId",
                table: "ScheduleInfo",
                column: "EmployeeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaxInfo_EmployeeId",
                table: "TaxInfo",
                column: "EmployeeId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_RoleInfo_RoleInfoId",
                table: "Employees",
                column: "RoleInfoId",
                principalTable: "RoleInfo",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationEntityEmployees_RoleInfo_RoleInfoId",
                table: "OrganizationEntityEmployees",
                column: "RoleInfoId",
                principalTable: "RoleInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleInfo_Employees_EmployeeId",
                table: "RoleInfo",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleInfo_Employees_SupervisorId",
                table: "RoleInfo",
                column: "SupervisorId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_RoleInfo_RoleInfoId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationEntityEmployees_RoleInfo_RoleInfoId",
                table: "OrganizationEntityEmployees");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleInfo_Employees_EmployeeId",
                table: "RoleInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleInfo_Employees_SupervisorId",
                table: "RoleInfo");

            migrationBuilder.DropTable(
                name: "CareerPathInfo");

            migrationBuilder.DropTable(
                name: "CompanyInfo");

            migrationBuilder.DropTable(
                name: "EmergencyContactInfo");

            migrationBuilder.DropTable(
                name: "EmployeeQualificationInfo");

            migrationBuilder.DropTable(
                name: "InsuranceInfo");

            migrationBuilder.DropTable(
                name: "PersonalInfo");

            migrationBuilder.DropTable(
                name: "RoleInfoManagedOrganizationEntities");

            migrationBuilder.DropTable(
                name: "ScheduleInfo");

            migrationBuilder.DropTable(
                name: "TaxInfo");

            migrationBuilder.DropIndex(
                name: "IX_Employees_RoleInfoId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "RoleInfoId",
                table: "Employees");

            migrationBuilder.AlterColumn<int>(
                name: "SupervisorId",
                table: "RoleInfo",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DeputySupervisorId",
                table: "RoleInfo",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "RoleInfo",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<List<int>>(
                name: "SubordinateIds",
                table: "RoleInfo",
                type: "integer[]",
                nullable: false);

            migrationBuilder.AddColumn<int>(
                name: "RoleInfoId",
                table: "OrganizationEntities",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<List<string>>(
                name: "CareerPathInfo_Certifications",
                table: "Employees",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<List<string>>(
                name: "CareerPathInfo_Degrees",
                table: "Employees",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<List<string>>(
                name: "CareerPathInfo_Specializations",
                table: "Employees",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CompanyInfo_AnnualLeaveTotalDays",
                table: "Employees",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "CompanyInfo_CompanyEmail",
                table: "Employees",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyInfo_CompanyPhoneNumber",
                table: "Employees",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CompanyInfo_CompensatoryLeaveTotalDays",
                table: "Employees",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "CompanyInfo_Department",
                table: "Employees",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanyInfo_EmploymentStatus",
                table: "Employees",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CompanyInfo_EndDate",
                table: "Employees",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "CompanyInfo_IsOnProbation",
                table: "Employees",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CompanyInfo_Position",
                table: "Employees",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CompanyInfo_ProbationEndDate",
                table: "Employees",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CompanyInfo_ProbationStartDate",
                table: "Employees",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CompanyInfo_StartDate",
                table: "Employees",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmergencyContactInfo_CurrentAddress",
                table: "Employees",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmergencyContactInfo_Name",
                table: "Employees",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmergencyContactInfo_PermanentAddress",
                table: "Employees",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmergencyContactInfo_Phone",
                table: "Employees",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmergencyContactInfo_Relationship",
                table: "Employees",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "InsuranceInfo_EffectiveDate",
                table: "Employees",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "InsuranceInfo_ExpiryDate",
                table: "Employees",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InsuranceInfo_InsuranceNumber",
                table: "Employees",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InsuranceInfo_Provider",
                table: "Employees",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PersonalInfo_Address",
                table: "Employees",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PersonalInfo_Birthplace",
                table: "Employees",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "PersonalInfo_DateOfBirth",
                table: "Employees",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "PersonalInfo_EthnicGroup",
                table: "Employees",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PersonalInfo_Gender",
                table: "Employees",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "PersonalInfo_IdCardExpiryDate",
                table: "Employees",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PersonalInfo_IdCardIssueDate",
                table: "Employees",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PersonalInfo_IdCardIssuePlace",
                table: "Employees",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PersonalInfo_IdCardNumber",
                table: "Employees",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PersonalInfo_MaritalStatus",
                table: "Employees",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PersonalInfo_Nationality",
                table: "Employees",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PersonalInfo_PersonalEmail",
                table: "Employees",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PersonalInfo_PersonalPhoneNumber",
                table: "Employees",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ScheduleInfo_IsRemoteStatus",
                table: "Employees",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ScheduleInfo_ShiftType",
                table: "Employees",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ScheduleInfo_WorkSchedule",
                table: "Employees",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TaxInfo_Region",
                table: "Employees",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TaxInfo_TaxId",
                table: "Employees",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TaxInfo_TaxStatus",
                table: "Employees",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationEntities_RoleInfoId",
                table: "OrganizationEntities",
                column: "RoleInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationEntities_RoleInfo_RoleInfoId",
                table: "OrganizationEntities",
                column: "RoleInfoId",
                principalTable: "RoleInfo",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationEntityEmployees_RoleInfo_RoleInfoId",
                table: "OrganizationEntityEmployees",
                column: "RoleInfoId",
                principalTable: "RoleInfo",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleInfo_Employees_EmployeeId",
                table: "RoleInfo",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleInfo_Employees_SupervisorId",
                table: "RoleInfo",
                column: "SupervisorId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
