using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bibliotecaPDF.Migrations
{
    /// <inheritdoc />
    public partial class addingblobstorage2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BackBlazeId",
                table: "PdfFile",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "UserByteAmounts",
                table: "AspNetUsers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackBlazeId",
                table: "PdfFile");

            migrationBuilder.DropColumn(
                name: "UserByteAmounts",
                table: "AspNetUsers");
        }
    }
}
