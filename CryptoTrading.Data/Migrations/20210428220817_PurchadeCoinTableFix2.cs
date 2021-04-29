using Microsoft.EntityFrameworkCore.Migrations;

namespace CryptoTrading.Data.Migrations
{
    public partial class PurchadeCoinTableFix2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WalletCoins_Coins_CoinId",
                table: "WalletCoins");

            migrationBuilder.DropForeignKey(
                name: "FK_WalletCoins_Wallets_WalletId",
                table: "WalletCoins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WalletCoins",
                table: "WalletCoins");

            migrationBuilder.RenameTable(
                name: "WalletCoins",
                newName: "PurchasedCoin");

            migrationBuilder.RenameIndex(
                name: "IX_WalletCoins_CoinId",
                table: "PurchasedCoin",
                newName: "IX_PurchasedCoin_CoinId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PurchasedCoin",
                table: "PurchasedCoin",
                columns: new[] { "WalletId", "CoinId" });

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasedCoin_Coins_CoinId",
                table: "PurchasedCoin",
                column: "CoinId",
                principalTable: "Coins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasedCoin_Wallets_WalletId",
                table: "PurchasedCoin",
                column: "WalletId",
                principalTable: "Wallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchasedCoin_Coins_CoinId",
                table: "PurchasedCoin");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchasedCoin_Wallets_WalletId",
                table: "PurchasedCoin");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PurchasedCoin",
                table: "PurchasedCoin");

            migrationBuilder.RenameTable(
                name: "PurchasedCoin",
                newName: "WalletCoins");

            migrationBuilder.RenameIndex(
                name: "IX_PurchasedCoin_CoinId",
                table: "WalletCoins",
                newName: "IX_WalletCoins_CoinId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WalletCoins",
                table: "WalletCoins",
                columns: new[] { "WalletId", "CoinId" });

            migrationBuilder.AddForeignKey(
                name: "FK_WalletCoins_Coins_CoinId",
                table: "WalletCoins",
                column: "CoinId",
                principalTable: "Coins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WalletCoins_Wallets_WalletId",
                table: "WalletCoins",
                column: "WalletId",
                principalTable: "Wallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
