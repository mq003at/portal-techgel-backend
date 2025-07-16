using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace portal_techgel_api.Migrations
{
    /// <inheritdoc />
    public partial class fix0StockConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "Stocks",
                newName: "TotalQuantity");

            migrationBuilder.CreateTable(
                name: "WarehouseLocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WarehouseId = table.Column<int>(type: "integer", nullable: false),
                    Zone = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Aisle = table.Column<int>(type: "integer", nullable: false),
                    Rack = table.Column<int>(type: "integer", nullable: false),
                    Shelf = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    MainId = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarehouseLocations_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockLocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StockId = table.Column<int>(type: "integer", nullable: false),
                    WarehouseLocationId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric", nullable: false),
                    MainId = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockLocations_Stocks_StockId",
                        column: x => x.StockId,
                        principalTable: "Stocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockLocations_WarehouseLocations_WarehouseLocationId",
                        column: x => x.WarehouseLocationId,
                        principalTable: "WarehouseLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockLocations_StockId_WarehouseLocationId",
                table: "StockLocations",
                columns: new[] { "StockId", "WarehouseLocationId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StockLocations_WarehouseLocationId",
                table: "StockLocations",
                column: "WarehouseLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseLocations_WarehouseId_Zone_Aisle_Rack_Shelf",
                table: "WarehouseLocations",
                columns: new[] { "WarehouseId", "Zone", "Aisle", "Rack", "Shelf" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockLocations");

            migrationBuilder.DropTable(
                name: "WarehouseLocations");

            migrationBuilder.RenameColumn(
                name: "TotalQuantity",
                table: "Stocks",
                newName: "Quantity");
        }
    }
}
