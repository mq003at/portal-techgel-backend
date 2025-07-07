using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace portal_techgel_api.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDocumentAssociationFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentAssociations_LeaveRequestWorkflows_EntityId",
                table: "DocumentAssociations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_DocumentAssociations_LeaveRequestWorkflows_EntityId",
                table: "DocumentAssociations",
                column: "EntityId",
                principalTable: "LeaveRequestWorkflows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
