using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyReadingNookApp.Migrations
{
    /// <inheritdoc />
    public partial class UserNotesMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserNotes",
                table: "UserBooks",
                type: "varchar(250)",
                maxLength: 250,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserNotes",
                table: "UserBooks");
        }
    }
}
