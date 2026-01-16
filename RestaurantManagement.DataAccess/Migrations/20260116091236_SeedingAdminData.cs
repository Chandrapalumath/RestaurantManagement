using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantManagement.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SeedingAdminData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "IsActive", "MobileNumber", "Name", "Password", "Role" },
                values: new object[] { 1, new DateTime(2026, 1, 16, 9, 12, 34, 755, DateTimeKind.Utc).AddTicks(4349), "admin@gmail.com", true, "9999999999", "Admin", "$2a$11$lzkWRxJP0JpuE0Rbk6wdP.dia5BCzxFr1UoDxB8IrZx6qHcKGhY7a", 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
