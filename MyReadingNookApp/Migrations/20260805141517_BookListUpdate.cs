using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyReadingNookApp.Migrations
{
    /// <inheritdoc />
    public partial class BookListUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserModelId",
                table: "BookLists",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BookLists_UserModelId",
                table: "BookLists",
                column: "UserModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookLists_Users_UserModelId",
                table: "BookLists",
                column: "UserModelId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookLists_Users_UserModelId",
                table: "BookLists");

            migrationBuilder.DropIndex(
                name: "IX_BookLists_UserModelId",
                table: "BookLists");

            migrationBuilder.DropColumn(
                name: "UserModelId",
                table: "BookLists");
        }
    }
}
