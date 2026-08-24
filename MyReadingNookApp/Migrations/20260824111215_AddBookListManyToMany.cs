using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyReadingNookApp.Migrations
{
    /// <inheritdoc />
    public partial class AddBookListManyToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_BookLists_BookListModelId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_BookListModelId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "BookListModelId",
                table: "Books");

            migrationBuilder.CreateTable(
                name: "BookListModelBookModel",
                columns: table => new
                {
                    BookItemsId = table.Column<int>(type: "int", nullable: false),
                    BookListId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookListModelBookModel", x => new { x.BookItemsId, x.BookListId });
                    table.ForeignKey(
                        name: "FK_BookListModelBookModel_BookLists_BookListId",
                        column: x => x.BookListId,
                        principalTable: "BookLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookListModelBookModel_Books_BookItemsId",
                        column: x => x.BookItemsId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_BookListModelBookModel_BookListId",
                table: "BookListModelBookModel",
                column: "BookListId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookListModelBookModel");

            migrationBuilder.AddColumn<int>(
                name: "BookListModelId",
                table: "Books",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Books_BookListModelId",
                table: "Books",
                column: "BookListModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_BookLists_BookListModelId",
                table: "Books",
                column: "BookListModelId",
                principalTable: "BookLists",
                principalColumn: "Id");
        }
    }
}
