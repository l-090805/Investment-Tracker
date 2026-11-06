using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Investment_Tracker.Migrations
{
    /// <inheritdoc />
    public partial class AddPriceHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "historyFrom",
                table: "Assets",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "historySyncedOn",
                table: "Assets",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "lastPrice",
                table: "Assets",
                type: "TEXT",
                precision: 18,
                scale: 8,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "lastPriceAt",
                table: "Assets",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PriceHistory",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    assetId = table.Column<int>(type: "INTEGER", nullable: false),
                    date = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    close = table.Column<decimal>(type: "TEXT", precision: 18, scale: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceHistory", x => x.id);
                    table.ForeignKey(
                        name: "FK_PriceHistory_Assets_assetId",
                        column: x => x.assetId,
                        principalTable: "Assets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PriceHistory_assetId_date",
                table: "PriceHistory",
                columns: new[] { "assetId", "date" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PriceHistory");

            migrationBuilder.DropColumn(
                name: "historyFrom",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "historySyncedOn",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "lastPrice",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "lastPriceAt",
                table: "Assets");
        }
    }
}
