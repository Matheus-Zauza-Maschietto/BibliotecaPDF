using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bibliotecaPDF.Migrations
{
    /// <inheritdoc />
    public partial class NewCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_AspNetUsers_UserId",
                table: "Files");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Files",
                table: "Files");

            migrationBuilder.RenameTable(
                name: "Files",
                newName: "PdfFile");

            migrationBuilder.RenameIndex(
                name: "IX_Files_UserId",
                table: "PdfFile",
                newName: "IX_PdfFile_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PdfFile",
                table: "PdfFile",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PdfFile_AspNetUsers_UserId",
                table: "PdfFile",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PdfFile_AspNetUsers_UserId",
                table: "PdfFile");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PdfFile",
                table: "PdfFile");

            migrationBuilder.RenameTable(
                name: "PdfFile",
                newName: "Files");

            migrationBuilder.RenameIndex(
                name: "IX_PdfFile_UserId",
                table: "Files",
                newName: "IX_Files_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Files",
                table: "Files",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_AspNetUsers_UserId",
                table: "Files",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
