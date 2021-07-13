using Microsoft.EntityFrameworkCore.Migrations;

namespace CryptoTrading.Data.Migrations
{
    public partial class AddedPictureToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Picture",
                table: "Users",
                newName: "ProfilePicture");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProfilePicture",
                table: "Users",
                newName: "Picture");
        }
    }
}
