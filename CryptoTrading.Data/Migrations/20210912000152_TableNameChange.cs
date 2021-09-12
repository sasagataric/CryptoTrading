using Microsoft.EntityFrameworkCore.Migrations;

namespace CryptoTrading.Data.Migrations
{
    public partial class TableNameChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                newName: "Holdings");

            migrationBuilder.RenameIndex(
                name: "IX_PurchasedCoin_CoinId",
                table: "Holdings",
                newName: "IX_Holdings_CoinId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Holdings",
                table: "Holdings",
                columns: new[] { "WalletId", "CoinId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Holdings_Coins_CoinId",
                table: "Holdings",
                column: "CoinId",
                principalTable: "Coins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Holdings_Wallets_WalletId",
                table: "Holdings",
                column: "WalletId",
                principalTable: "Wallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Holdings_Coins_CoinId",
                table: "Holdings");

            migrationBuilder.DropForeignKey(
                name: "FK_Holdings_Wallets_WalletId",
                table: "Holdings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Holdings",
                table: "Holdings");

            migrationBuilder.RenameTable(
                name: "Holdings",
                newName: "PurchasedCoin");

            migrationBuilder.RenameIndex(
                name: "IX_Holdings_CoinId",
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
    }
}
