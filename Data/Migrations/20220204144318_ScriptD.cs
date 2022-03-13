using Microsoft.EntityFrameworkCore.Migrations;

namespace Library_Managenent.Data.Migrations
{
    public partial class ScriptD : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Book_Title",
                table: "Book",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300);

            migrationBuilder.AddColumn<string>(
                name: "Authore",
                table: "Book",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Categories",
                table: "Book",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Details",
                table: "Book",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Picture",
                table: "Book",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Authore",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "Categories",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "Details",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "Picture",
                table: "Book");

            migrationBuilder.AlterColumn<string>(
                name: "Book_Title",
                table: "Book",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 300,
                oldNullable: true);
        }
    }
}
