using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace portal_techgel_api.Migrations
{
    /// <inheritdoc />
    public partial class revertshadowstate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentAssociations_Documents_DocumentId",
                table: "DocumentAssociations");

            migrationBuilder.AlterColumn<int>(
                name: "DocumentId",
                table: "DocumentAssociations",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EntityType",
                table: "DocumentAssociations",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");





            migrationBuilder.AddForeignKey(
                name: "FK_DocumentAssociations_Documents_DocumentId",
                table: "DocumentAssociations",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentAssociations_Documents_DocumentId",
                table: "DocumentAssociations");

            migrationBuilder.DropIndex(
                name: "IX_DocumentAssociations_NodeId_EntityType",
                table: "DocumentAssociations");

            migrationBuilder.DropColumn(
                name: "EntityType",
                table: "DocumentAssociations");

            migrationBuilder.DropColumn(
                name: "NodeId",
                table: "DocumentAssociations");

            migrationBuilder.AlterColumn<int>(
                name: "DocumentId",
                table: "DocumentAssociations",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentAssociations_Documents_DocumentId",
                table: "DocumentAssociations",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id");
        }
    }
}
