using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace portal_techgel_api.Migrations
{
    /// <inheritdoc />
    public partial class ApprovalNodeArray : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_ApprovalWorkflowNodes_ApprovalWorkflowNodeId",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Documents_ApprovalWorkflowNodeId",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "ApprovalWorkflowNodeId",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "ApprovalComment",
                table: "ApprovalWorkflowNodes");

            migrationBuilder.DropColumn(
                name: "ReceiverId",
                table: "ApprovalWorkflowNodes");

            migrationBuilder.DropColumn(
                name: "ReceiverMessage",
                table: "ApprovalWorkflowNodes");

            migrationBuilder.DropColumn(
                name: "ReceiverName",
                table: "ApprovalWorkflowNodes");

            migrationBuilder.RenameColumn(
                name: "SortOrder",
                table: "ApprovalWorkflowNodes",
                newName: "Order");

            migrationBuilder.AlterColumn<string>(
                name: "SenderName",
                table: "ApprovalWorkflowNodes",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SenderMessage",
                table: "ApprovalWorkflowNodes",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000,
                oldNullable: true,
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "DocumentIds",
                table: "ApprovalWorkflowNodes",
                type: "text",
                nullable: false,
                oldClrType: typeof(List<int>),
                oldType: "integer[]");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ApprovalDate",
                table: "ApprovalWorkflowNodes",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApprovalCommentIds",
                table: "ApprovalWorkflowNodes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ApprovalComments",
                table: "ApprovalWorkflowNodes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReceiverIds",
                table: "ApprovalWorkflowNodes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ReceiverMessages",
                table: "ApprovalWorkflowNodes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReceiverNames",
                table: "ApprovalWorkflowNodes",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ApprovalWorkflowNodeDocuments",
                columns: table => new
                {
                    ApprovalWorkflowNodeId = table.Column<int>(type: "integer", nullable: false),
                    DocumentsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApprovalWorkflowNodeDocuments", x => new { x.ApprovalWorkflowNodeId, x.DocumentsId });
                    table.ForeignKey(
                        name: "FK_ApprovalWorkflowNodeDocuments_ApprovalWorkflowNodes_Approva~",
                        column: x => x.ApprovalWorkflowNodeId,
                        principalTable: "ApprovalWorkflowNodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApprovalWorkflowNodeDocuments_Documents_DocumentsId",
                        column: x => x.DocumentsId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalWorkflowNodeDocuments_DocumentsId",
                table: "ApprovalWorkflowNodeDocuments",
                column: "DocumentsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApprovalWorkflowNodeDocuments");

            migrationBuilder.DropColumn(
                name: "ApprovalCommentIds",
                table: "ApprovalWorkflowNodes");

            migrationBuilder.DropColumn(
                name: "ApprovalComments",
                table: "ApprovalWorkflowNodes");

            migrationBuilder.DropColumn(
                name: "ReceiverIds",
                table: "ApprovalWorkflowNodes");

            migrationBuilder.DropColumn(
                name: "ReceiverMessages",
                table: "ApprovalWorkflowNodes");

            migrationBuilder.DropColumn(
                name: "ReceiverNames",
                table: "ApprovalWorkflowNodes");

            migrationBuilder.RenameColumn(
                name: "Order",
                table: "ApprovalWorkflowNodes",
                newName: "SortOrder");

            migrationBuilder.AddColumn<int>(
                name: "ApprovalWorkflowNodeId",
                table: "Documents",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SenderName",
                table: "ApprovalWorkflowNodes",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SenderMessage",
                table: "ApprovalWorkflowNodes",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<List<int>>(
                name: "DocumentIds",
                table: "ApprovalWorkflowNodes",
                type: "integer[]",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ApprovalDate",
                table: "ApprovalWorkflowNodes",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApprovalComment",
                table: "ApprovalWorkflowNodes",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ReceiverId",
                table: "ApprovalWorkflowNodes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ReceiverMessage",
                table: "ApprovalWorkflowNodes",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ReceiverName",
                table: "ApprovalWorkflowNodes",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Documents_ApprovalWorkflowNodeId",
                table: "Documents",
                column: "ApprovalWorkflowNodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_ApprovalWorkflowNodes_ApprovalWorkflowNodeId",
                table: "Documents",
                column: "ApprovalWorkflowNodeId",
                principalTable: "ApprovalWorkflowNodes",
                principalColumn: "Id");
        }
    }
}
