using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecomm_Project_2003.DataAccess.Migrations
{
    public partial class ApplyPendingChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TransactionId",
                table: "OrderHeaders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShoopingCarts_ProductId",
                table: "ShoopingCarts",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoopingCarts_Products_ProductId",
                table: "ShoopingCarts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoopingCarts_Products_ProductId",
                table: "ShoopingCarts");

            migrationBuilder.DropIndex(
                name: "IX_ShoopingCarts_ProductId",
                table: "ShoopingCarts");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "OrderHeaders");
        }
    }
}
