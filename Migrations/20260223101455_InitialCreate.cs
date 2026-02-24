using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Investment_Tracker.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Assets",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    code = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    name = table.Column<string>(type: "TEXT", nullable: false),
                    category = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assets", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Investments",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    assetId = table.Column<int>(type: "INTEGER", nullable: false),
                    quantity = table.Column<decimal>(type: "TEXT", precision: 18, scale: 8, nullable: false),
                    buyPrice = table.Column<decimal>(type: "TEXT", precision: 18, scale: 8, nullable: false),
                    buyDate = table.Column<DateOnly>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Investments", x => x.id);
                    table.ForeignKey(
                        name: "FK_Investments_Assets_assetId",
                        column: x => x.assetId,
                        principalTable: "Assets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assets_code",
                table: "Assets",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Investments_assetId",
                table: "Investments",
                column: "assetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Investments");

            migrationBuilder.DropTable(
                name: "Assets");
        }
    }
}
