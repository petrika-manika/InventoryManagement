using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixNIPTUniqueIndexFilter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BusinessClients_NIPT_Unique",
                table: "Clients");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessClients_NIPT_Unique",
                table: "Clients",
                column: "NIPT",
                unique: true,
                filter: "[NIPT] IS NOT NULL AND [IsActive] = 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BusinessClients_NIPT_Unique",
                table: "Clients");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessClients_NIPT_Unique",
                table: "Clients",
                column: "NIPT",
                unique: true,
                filter: "[IsActive] = 1");
        }
    }
}
