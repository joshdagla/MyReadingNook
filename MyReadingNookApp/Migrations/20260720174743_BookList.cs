using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyReadingNookApp.Migrations
{
    /// <inheritdoc />
    public partial class BookList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BookListModelId",
                table: "Books",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BookLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ListName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookLists", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Books_BookListModelId",
                table: "Books",
                column: "BookListModelId");

            migrationBuilder.CreateIndex(
                name: "IX_BookLists_ListName",
                table: "BookLists",
                column: "ListName",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_BookLists_BookListModelId",
                table: "Books",
                column: "BookListModelId",
                principalTable: "BookLists",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_BookLists_BookListModelId",
                table: "Books");

            migrationBuilder.DropTable(
                name: "BookLists");

            migrationBuilder.DropIndex(
                name: "IX_Books_BookListModelId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "BookListModelId",
                table: "Books");
        }
    }
}
