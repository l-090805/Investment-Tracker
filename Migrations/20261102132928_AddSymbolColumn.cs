using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Investment_Tracker.Migrations
{
    /// <inheritdoc />
    public partial class AddSymbolColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "symbol",
                table: "Assets",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "symbol",
                table: "Assets");
        }
    }
}
