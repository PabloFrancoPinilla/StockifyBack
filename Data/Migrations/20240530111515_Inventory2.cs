using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stockify.Data.Migrations
{
    /// <inheritdoc />
    public partial class Inventory2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Inventories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Inventories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Inventories");
        }
    }
}
