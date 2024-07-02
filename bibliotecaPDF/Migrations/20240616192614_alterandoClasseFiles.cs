using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bibliotecaPDF.Migrations
{
    /// <inheritdoc />
    public partial class alterandoClasseFiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileBase64",
                table: "Files");

            migrationBuilder.AddColumn<byte[]>(
                name: "Content",
                table: "Files",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "Files");

            migrationBuilder.AddColumn<string>(
                name: "FileBase64",
                table: "Files",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
