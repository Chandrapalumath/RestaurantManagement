using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantManagement.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddingWaiterIdOnTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RestaurantSettings",
                keyColumn: "Id",
                keyValue: new Guid("c4a2b86a-9087-494b-89c0-f1fd2da06236"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("5c8ad58d-be6f-4a15-92b2-677481db4d1c"));

            migrationBuilder.AddColumn<Guid>(
                name: "OccupiedByWaiterId",
                table: "Tables",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "IsActive", "MobileNumber", "Name", "Password", "Role" },
                values: new object[] { new Guid("f52175ae-2fd3-47c2-9cdb-938ba799bc70"), new DateTime(2026, 2, 1, 10, 10, 41, 195, DateTimeKind.Utc).AddTicks(9216), "admin@gmail.com", true, "9999999999", "Admin", "$2a$11$NMnQ3GC/R3MMX8OjnIv.Be3aRoL8ULT.uKE7OLRfITe9kdkP3Td0m", 1 });

            migrationBuilder.InsertData(
                table: "RestaurantSettings",
                columns: new[] { "Id", "DiscountPercent", "TaxPercent", "UpdatedAt", "UpdatedByAdminId" },
                values: new object[] { new Guid("4f6a6598-8ccf-47f8-a548-2d19fe19960f"), 10m, 10m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("f52175ae-2fd3-47c2-9cdb-938ba799bc70") });

            migrationBuilder.CreateIndex(
                name: "IX_Tables_OccupiedByWaiterId",
                table: "Tables",
                column: "OccupiedByWaiterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tables_Users_OccupiedByWaiterId",
                table: "Tables",
                column: "OccupiedByWaiterId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tables_Users_OccupiedByWaiterId",
                table: "Tables");

            migrationBuilder.DropIndex(
                name: "IX_Tables_OccupiedByWaiterId",
                table: "Tables");

            migrationBuilder.DeleteData(
                table: "RestaurantSettings",
                keyColumn: "Id",
                keyValue: new Guid("4f6a6598-8ccf-47f8-a548-2d19fe19960f"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("f52175ae-2fd3-47c2-9cdb-938ba799bc70"));

            migrationBuilder.DropColumn(
                name: "OccupiedByWaiterId",
                table: "Tables");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "IsActive", "MobileNumber", "Name", "Password", "Role" },
                values: new object[] { new Guid("5c8ad58d-be6f-4a15-92b2-677481db4d1c"), new DateTime(2026, 1, 25, 20, 17, 12, 637, DateTimeKind.Utc).AddTicks(4505), "admin@gmail.com", true, "9999999999", "Admin", "$2a$11$YtrlF83W1L3LQTvETD3HoeOPdNwWW3kGYE9ynzoCh2L7O9RBSG5RC", 1 });

            migrationBuilder.InsertData(
                table: "RestaurantSettings",
                columns: new[] { "Id", "DiscountPercent", "TaxPercent", "UpdatedAt", "UpdatedByAdminId" },
                values: new object[] { new Guid("c4a2b86a-9087-494b-89c0-f1fd2da06236"), 10m, 10m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("5c8ad58d-be6f-4a15-92b2-677481db4d1c") });
        }
    }
}
