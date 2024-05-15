using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stockify.Data.Migrations
{
    /// <inheritdoc />
    public partial class LoginTenant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tenants_Inventories_InventoryId",
                table: "Tenants");

            migrationBuilder.DropIndex(
                name: "IX_Tenants_InventoryId",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "InventoryId",
                table: "Tenants");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Tenants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "Inventories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_TenantId",
                table: "Inventories",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventories_Tenants_TenantId",
                table: "Inventories",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventories_Tenants_TenantId",
                table: "Inventories");

            migrationBuilder.DropIndex(
                name: "IX_Inventories_TenantId",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Inventories");

            migrationBuilder.AddColumn<int>(
                name: "InventoryId",
                table: "Tenants",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_InventoryId",
                table: "Tenants",
                column: "InventoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tenants_Inventories_InventoryId",
                table: "Tenants",
                column: "InventoryId",
                principalTable: "Inventories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
