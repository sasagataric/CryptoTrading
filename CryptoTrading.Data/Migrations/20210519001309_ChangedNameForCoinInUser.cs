using Microsoft.EntityFrameworkCore.Migrations;

namespace CryptoTrading.Data.Migrations
{
    public partial class ChangedNameForCoinInUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchasedCoin_WatchListCoins_CoinId",
                table: "PurchasedCoin");

            migrationBuilder.DropForeignKey(
                name: "FK_WalletHistorys_WatchListCoins_CoinId",
                table: "WalletHistorys");

            migrationBuilder.DropForeignKey(
                name: "FK_WatchList_WatchListCoins_CoinsId",
                table: "WatchList");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WatchList",
                table: "WatchList");

            migrationBuilder.DropIndex(
                name: "IX_WatchList_UsersId",
                table: "WatchList");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WatchListCoins",
                table: "Coins");

            migrationBuilder.RenameTable(
                name: "Coins",
                newName: "Coins");

            migrationBuilder.RenameColumn(
                name: "CoinsId",
                table: "WatchList",
                newName: "WatchListCoinsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WatchList",
                table: "WatchList",
                columns: new[] { "UsersId", "WatchListCoinsId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Coins",
                table: "Coins",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_WatchList_WatchListCoinsId",
                table: "WatchList",
                column: "WatchListCoinsId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasedCoin_Coins_CoinId",
                table: "PurchasedCoin",
                column: "CoinId",
                principalTable: "Coins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WalletHistorys_Coins_CoinId",
                table: "WalletHistorys",
                column: "CoinId",
                principalTable: "Coins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WatchList_Coins_WatchListCoinsId",
                table: "WatchList",
                column: "WatchListCoinsId",
                principalTable: "Coins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchasedCoin_Coins_CoinId",
                table: "PurchasedCoin");

            migrationBuilder.DropForeignKey(
                name: "FK_WalletHistorys_Coins_CoinId",
                table: "WalletHistorys");

            migrationBuilder.DropForeignKey(
                name: "FK_WatchList_Coins_WatchListCoinsId",
                table: "WatchList");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WatchList",
                table: "WatchList");

            migrationBuilder.DropIndex(
                name: "IX_WatchList_WatchListCoinsId",
                table: "WatchList");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Coins",
                table: "Coins");

            migrationBuilder.RenameTable(
                name: "Coins",
                newName: "Coins");

            migrationBuilder.RenameColumn(
                name: "WatchListCoinsId",
                table: "WatchList",
                newName: "CoinsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WatchList",
                table: "WatchList",
                columns: new[] { "CoinsId", "UsersId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_WatchListCoins",
                table: "Coins",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_WatchList_UsersId",
                table: "WatchList",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasedCoin_WatchListCoins_CoinId",
                table: "PurchasedCoin",
                column: "CoinId",
                principalTable: "Coins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WalletHistorys_WatchListCoins_CoinId",
                table: "WalletHistorys",
                column: "CoinId",
                principalTable: "Coins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WatchList_WatchListCoins_CoinsId",
                table: "WatchList",
                column: "CoinsId",
                principalTable: "Coins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
