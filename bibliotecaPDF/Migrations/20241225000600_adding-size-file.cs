using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bibliotecaPDF.Migrations
{
    /// <inheritdoc />
    public partial class addingsizefile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "FileSize",
                table: "PdfFile",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileSize",
                table: "PdfFile");
        }
    }
}
