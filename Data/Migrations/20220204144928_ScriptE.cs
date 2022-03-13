using Microsoft.EntityFrameworkCore.Migrations;

namespace Library_Managenent.Data.Migrations
{
    public partial class ScriptE : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Authore",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "Categories",
                table: "Book");

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "Book",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Book",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Author",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Book");

            migrationBuilder.AddColumn<string>(
                name: "Authore",
                table: "Book",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Categories",
                table: "Book",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
