using Microsoft.EntityFrameworkCore.Migrations;

namespace Library_Managenent.Data.Migrations
{
    public partial class ScriptB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_Book_Status_Book_StatusId",
                schema: "Identity",
                table: "Book");

            migrationBuilder.DropIndex(
                name: "IX_Book_Book_StatusId",
                schema: "Identity",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "Book_StatusId",
                schema: "Identity",
                table: "Book");

            migrationBuilder.AddColumn<int>(
                name: "Book_StatusId",
                schema: "Identity",
                table: "BookCode",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BookCode_Book_StatusId",
                schema: "Identity",
                table: "BookCode",
                column: "Book_StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookCode_Book_Status_Book_StatusId",
                schema: "Identity",
                table: "BookCode",
                column: "Book_StatusId",
                principalSchema: "Identity",
                principalTable: "Book_Status",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookCode_Book_Status_Book_StatusId",
                schema: "Identity",
                table: "BookCode");

            migrationBuilder.DropIndex(
                name: "IX_BookCode_Book_StatusId",
                schema: "Identity",
                table: "BookCode");

            migrationBuilder.DropColumn(
                name: "Book_StatusId",
                schema: "Identity",
                table: "BookCode");

            migrationBuilder.AddColumn<int>(
                name: "Book_StatusId",
                schema: "Identity",
                table: "Book",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Book_Book_StatusId",
                schema: "Identity",
                table: "Book",
                column: "Book_StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Book_Book_Status_Book_StatusId",
                schema: "Identity",
                table: "Book",
                column: "Book_StatusId",
                principalSchema: "Identity",
                principalTable: "Book_Status",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
