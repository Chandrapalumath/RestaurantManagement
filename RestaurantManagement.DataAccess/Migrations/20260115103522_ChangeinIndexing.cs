using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantManagement.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ChangeinIndexing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_Name",
                table: "MenuItems",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_MobileNumber",
                table: "Customers",
                column: "MobileNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MenuItems_Name",
                table: "MenuItems");

            migrationBuilder.DropIndex(
                name: "IX_Customers_MobileNumber",
                table: "Customers");
        }
    }
}
