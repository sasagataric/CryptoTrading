using Microsoft.EntityFrameworkCore.Migrations;

namespace CryptoTrading.Data.Migrations
{
    public partial class AddedPurchadeCoinTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_WalletCoins",
                table: "WalletCoins");

            migrationBuilder.DropIndex(
                name: "IX_WalletCoins_WalletId",
                table: "WalletCoins");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WalletCoins",
                table: "WalletCoins",
                columns: new[] { "WalletId", "CoinId" });

            migrationBuilder.CreateIndex(
                name: "IX_WalletCoins_CoinId",
                table: "WalletCoins",
                column: "CoinId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_WalletCoins",
                table: "WalletCoins");

            migrationBuilder.DropIndex(
                name: "IX_WalletCoins_CoinId",
                table: "WalletCoins");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WalletCoins",
                table: "WalletCoins",
                columns: new[] { "CoinId", "WalletId" });

            migrationBuilder.CreateIndex(
                name: "IX_WalletCoins_WalletId",
                table: "WalletCoins",
                column: "WalletId");
        }
    }
}
