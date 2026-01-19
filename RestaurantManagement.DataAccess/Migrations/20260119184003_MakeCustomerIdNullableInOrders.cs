using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantManagement.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MakeCustomerIdNullableInOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RestaurantSettings",
                keyColumn: "Id",
                keyValue: new Guid("f1661fce-9e97-4181-be9d-30dbf2d0f32c"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("b81558f6-3f39-45bc-a509-9b3fb241dc77"));

            migrationBuilder.AlterColumn<Guid>(
                name: "CustomerId",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "IsActive", "MobileNumber", "Name", "Password", "Role" },
                values: new object[] { new Guid("d6143ffc-3a15-4bb6-9ddc-6f6e59db619c"), new DateTime(2026, 1, 19, 18, 40, 1, 825, DateTimeKind.Utc).AddTicks(797), "admin@gmail.com", true, "9999999999", "Admin", "$2a$11$s.rPwHKGzu8ivRg2f49u4.7oqm8ONPrh7xnsB7a6X87QJPIpLYNra", 1 });

            migrationBuilder.InsertData(
                table: "RestaurantSettings",
                columns: new[] { "Id", "DiscountPercent", "TaxPercent", "UpdatedAt", "UpdatedByAdminId" },
                values: new object[] { new Guid("5f8667dc-3ec9-40b7-98a5-ecb0d066c8a1"), 10m, 10m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("d6143ffc-3a15-4bb6-9ddc-6f6e59db619c") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RestaurantSettings",
                keyColumn: "Id",
                keyValue: new Guid("5f8667dc-3ec9-40b7-98a5-ecb0d066c8a1"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d6143ffc-3a15-4bb6-9ddc-6f6e59db619c"));

            migrationBuilder.AlterColumn<Guid>(
                name: "CustomerId",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "IsActive", "MobileNumber", "Name", "Password", "Role" },
                values: new object[] { new Guid("b81558f6-3f39-45bc-a509-9b3fb241dc77"), new DateTime(2026, 1, 19, 18, 21, 34, 370, DateTimeKind.Utc).AddTicks(7907), "admin@gmail.com", true, "9999999999", "Admin", "$2a$11$I7ePdsl03W/PvQlE.wS5Uuezd8v4NDp5bQyMPFl3neueUGhiQBNiW", 1 });

            migrationBuilder.InsertData(
                table: "RestaurantSettings",
                columns: new[] { "Id", "DiscountPercent", "TaxPercent", "UpdatedAt", "UpdatedByAdminId" },
                values: new object[] { new Guid("f1661fce-9e97-4181-be9d-30dbf2d0f32c"), 10m, 10m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("b81558f6-3f39-45bc-a509-9b3fb241dc77") });
        }
    }
}
