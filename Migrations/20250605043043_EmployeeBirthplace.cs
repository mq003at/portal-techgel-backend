using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace portal_techgel_api.Migrations
{
    /// <inheritdoc />
    public partial class EmployeeBirthplace : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PersonalInfo_Birthplace",
                table: "Employees",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PersonalInfo_EthnicGroup",
                table: "Employees",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PersonalInfo_IdCardIssuePlace",
                table: "Employees",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PersonalInfo_Birthplace",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "PersonalInfo_EthnicGroup",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "PersonalInfo_IdCardIssuePlace",
                table: "Employees");
        }
    }
}
