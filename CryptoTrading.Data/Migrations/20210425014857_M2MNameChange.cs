using Microsoft.EntityFrameworkCore.Migrations;

namespace CryptoTrading.Data.Migrations
{
    public partial class M2MNameChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoinUser_Coins_CoinsId",
                table: "CoinUser");

            migrationBuilder.DropForeignKey(
                name: "FK_CoinUser_Users_UsersId",
                table: "CoinUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CoinUser",
                table: "CoinUser");

            migrationBuilder.RenameTable(
                name: "CoinUser",
                newName: "WatchList");

            migrationBuilder.RenameIndex(
                name: "IX_CoinUser_UsersId",
                table: "WatchList",
                newName: "IX_WatchList_UsersId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WatchList",
                table: "WatchList",
                columns: new[] { "CoinsId", "UsersId" });

            migrationBuilder.AddForeignKey(
                name: "FK_WatchList_Coins_CoinsId",
                table: "WatchList",
                column: "CoinsId",
                principalTable: "Coins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WatchList_Users_UsersId",
                table: "WatchList",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WatchList_Coins_CoinsId",
                table: "WatchList");

            migrationBuilder.DropForeignKey(
                name: "FK_WatchList_Users_UsersId",
                table: "WatchList");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WatchList",
                table: "WatchList");

            migrationBuilder.RenameTable(
                name: "WatchList",
                newName: "CoinUser");

            migrationBuilder.RenameIndex(
                name: "IX_WatchList_UsersId",
                table: "CoinUser",
                newName: "IX_CoinUser_UsersId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CoinUser",
                table: "CoinUser",
                columns: new[] { "CoinsId", "UsersId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CoinUser_Coins_CoinsId",
                table: "CoinUser",
                column: "CoinsId",
                principalTable: "Coins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CoinUser_Users_UsersId",
                table: "CoinUser",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
