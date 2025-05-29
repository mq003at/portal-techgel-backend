using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace portal_techgel_api.Migrations
{
    /// <inheritdoc />
    public partial class AddSignatureMetadata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "Signatures",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256
            );

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "Signatures",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: ""
            );

            migrationBuilder.AddColumn<long>(
                name: "FileSize",
                table: "Signatures",
                type: "bigint",
                nullable: false,
                defaultValue: 0L
            );

            migrationBuilder.AddColumn<string>(
                name: "StoragePath",
                table: "Signatures",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: ""
            );

            migrationBuilder.AddColumn<DateTime>(
                name: "UploadedAt",
                table: "Signatures",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "ContentType", table: "Signatures");

            migrationBuilder.DropColumn(name: "FileSize", table: "Signatures");

            migrationBuilder.DropColumn(name: "StoragePath", table: "Signatures");

            migrationBuilder.DropColumn(name: "UploadedAt", table: "Signatures");

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "Signatures",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255
            );
        }
    }
}
