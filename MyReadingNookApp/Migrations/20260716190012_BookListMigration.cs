using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyReadingNookApp.Migrations
{
    /// <inheritdoc />
    public partial class BookListMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserModelId",
                table: "Books",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Books_UserModelId",
                table: "Books",
                column: "UserModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Users_UserModelId",
                table: "Books",
                column: "UserModelId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Users_UserModelId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_UserModelId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "UserModelId",
                table: "Books");
        }
    }
}
