using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantManagement.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RenamingBillIdToBillingId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Bills_BillId",
                table: "Orders");

            migrationBuilder.DeleteData(
                table: "RestaurantSettings",
                keyColumn: "Id",
                keyValue: new Guid("5f8667dc-3ec9-40b7-98a5-ecb0d066c8a1"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d6143ffc-3a15-4bb6-9ddc-6f6e59db619c"));

            migrationBuilder.RenameColumn(
                name: "BillId",
                table: "Orders",
                newName: "BillingId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_BillId",
                table: "Orders",
                newName: "IX_Orders_BillingId");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "IsActive", "MobileNumber", "Name", "Password", "Role" },
                values: new object[] { new Guid("7c794889-d79a-408e-8cf4-0e4d7164ef65"), new DateTime(2026, 1, 19, 19, 28, 27, 864, DateTimeKind.Utc).AddTicks(4737), "admin@gmail.com", true, "9999999999", "Admin", "$2a$11$52o8iy6aLShT2l1ttTeNCOFyLrRpIvcywZmuGsWX8mj6.ERBHByOi", 1 });

            migrationBuilder.InsertData(
                table: "RestaurantSettings",
                columns: new[] { "Id", "DiscountPercent", "TaxPercent", "UpdatedAt", "UpdatedByAdminId" },
                values: new object[] { new Guid("d1856804-0a13-4b47-b1b2-8857a610be9d"), 10m, 10m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("7c794889-d79a-408e-8cf4-0e4d7164ef65") });

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Bills_BillingId",
                table: "Orders",
                column: "BillingId",
                principalTable: "Bills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Bills_BillingId",
                table: "Orders");

            migrationBuilder.DeleteData(
                table: "RestaurantSettings",
                keyColumn: "Id",
                keyValue: new Guid("d1856804-0a13-4b47-b1b2-8857a610be9d"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("7c794889-d79a-408e-8cf4-0e4d7164ef65"));

            migrationBuilder.RenameColumn(
                name: "BillingId",
                table: "Orders",
                newName: "BillId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_BillingId",
                table: "Orders",
                newName: "IX_Orders_BillId");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "IsActive", "MobileNumber", "Name", "Password", "Role" },
                values: new object[] { new Guid("d6143ffc-3a15-4bb6-9ddc-6f6e59db619c"), new DateTime(2026, 1, 19, 18, 40, 1, 825, DateTimeKind.Utc).AddTicks(797), "admin@gmail.com", true, "9999999999", "Admin", "$2a$11$s.rPwHKGzu8ivRg2f49u4.7oqm8ONPrh7xnsB7a6X87QJPIpLYNra", 1 });

            migrationBuilder.InsertData(
                table: "RestaurantSettings",
                columns: new[] { "Id", "DiscountPercent", "TaxPercent", "UpdatedAt", "UpdatedByAdminId" },
                values: new object[] { new Guid("5f8667dc-3ec9-40b7-98a5-ecb0d066c8a1"), 10m, 10m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("d6143ffc-3a15-4bb6-9ddc-6f6e59db619c") });

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Bills_BillId",
                table: "Orders",
                column: "BillId",
                principalTable: "Bills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
