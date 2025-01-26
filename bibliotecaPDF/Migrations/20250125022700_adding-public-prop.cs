using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bibliotecaPDF.Migrations
{
    /// <inheritdoc />
    public partial class addingpublicprop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileSurname",
                table: "PdfFile",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "PdfFile",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileSurname",
                table: "PdfFile");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "PdfFile");
        }
    }
}
