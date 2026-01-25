using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantManagement.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddingValidations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Customers_CustomerId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders");

            migrationBuilder.DeleteData(
                table: "RestaurantSettings",
                keyColumn: "Id",
                keyValue: new Guid("2f516c87-e573-4513-8eb0-9435ae6f0348"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("0ea73211-b510-43ec-938c-1189cc472bfc"));

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Orders");

            migrationBuilder.AlterColumn<string>(
                name: "MobileNumber",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "IsActive", "MobileNumber", "Name", "Password", "Role" },
                values: new object[] { new Guid("5c8ad58d-be6f-4a15-92b2-677481db4d1c"), new DateTime(2026, 1, 25, 20, 17, 12, 637, DateTimeKind.Utc).AddTicks(4505), "admin@gmail.com", true, "9999999999", "Admin", "$2a$11$YtrlF83W1L3LQTvETD3HoeOPdNwWW3kGYE9ynzoCh2L7O9RBSG5RC", 1 });

            migrationBuilder.InsertData(
                table: "RestaurantSettings",
                columns: new[] { "Id", "DiscountPercent", "TaxPercent", "UpdatedAt", "UpdatedByAdminId" },
                values: new object[] { new Guid("c4a2b86a-9087-494b-89c0-f1fd2da06236"), 10m, 10m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("5c8ad58d-be6f-4a15-92b2-677481db4d1c") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RestaurantSettings",
                keyColumn: "Id",
                keyValue: new Guid("c4a2b86a-9087-494b-89c0-f1fd2da06236"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("5c8ad58d-be6f-4a15-92b2-677481db4d1c"));

            migrationBuilder.AlterColumn<string>(
                name: "MobileNumber",
                table: "Users",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerId",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "IsActive", "MobileNumber", "Name", "Password", "Role" },
                values: new object[] { new Guid("0ea73211-b510-43ec-938c-1189cc472bfc"), new DateTime(2026, 1, 20, 18, 37, 9, 974, DateTimeKind.Utc).AddTicks(4024), "admin@gmail.com", true, "9999999999", "Admin", "$2a$11$wt0K5CokK82EvXbfLA/kgOpwc5vt83mNsu2rZv0d.tDoDNZHyF70K", 1 });

            migrationBuilder.InsertData(
                table: "RestaurantSettings",
                columns: new[] { "Id", "DiscountPercent", "TaxPercent", "UpdatedAt", "UpdatedByAdminId" },
                values: new object[] { new Guid("2f516c87-e573-4513-8eb0-9435ae6f0348"), 10m, 10m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("0ea73211-b510-43ec-938c-1189cc472bfc") });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Customers_CustomerId",
                table: "Orders",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id");
        }
    }
}
