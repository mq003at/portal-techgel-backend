using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace portal_techgel_api.Migrations
{
    /// <inheritdoc />
    public partial class notificationUrgencyLevel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsUrgentByDefault",
                table: "NotificationCategories");

            migrationBuilder.AddColumn<string>(
                name: "DefaultUrgencyLevel",
                table: "NotificationCategories",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultUrgencyLevel",
                table: "NotificationCategories");

            migrationBuilder.AddColumn<bool>(
                name: "IsUrgentByDefault",
                table: "NotificationCategories",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
