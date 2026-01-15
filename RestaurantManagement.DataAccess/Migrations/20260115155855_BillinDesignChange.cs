using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantManagement.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class BillinDesignChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bills_Orders_OrderId",
                table: "Bills");

            migrationBuilder.DropIndex(
                name: "IX_Bills_OrderId",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Bills");

            migrationBuilder.AddColumn<int>(
                name: "BillId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsBilled",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_BillId",
                table: "Orders",
                column: "BillId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Bills_BillId",
                table: "Orders",
                column: "BillId",
                principalTable: "Bills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Bills_BillId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_BillId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "BillId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "IsBilled",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Bills",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Bills_OrderId",
                table: "Bills",
                column: "OrderId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_Orders_OrderId",
                table: "Bills",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
