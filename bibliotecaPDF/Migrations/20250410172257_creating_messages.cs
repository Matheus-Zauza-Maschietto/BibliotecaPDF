using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace bibliotecaPDF.Migrations
{
    /// <inheritdoc />
    public partial class creating_messages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PdfFile_AspNetUsers_UserId",
                table: "PdfFile");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PdfFile",
                table: "PdfFile");

            migrationBuilder.RenameTable(
                name: "PdfFile",
                newName: "PdfFiles");

            migrationBuilder.RenameIndex(
                name: "IX_PdfFile_UserId",
                table: "PdfFiles",
                newName: "IX_PdfFiles_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PdfFiles",
                table: "PdfFiles",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    Text = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: false),
                    DateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_DateTime",
                table: "Messages",
                column: "DateTime",
                descending: new bool[0])
                .Annotation("Npgsql:CreatedConcurrently", false);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_UserId",
                table: "Messages",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PdfFiles_AspNetUsers_UserId",
                table: "PdfFiles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PdfFiles_AspNetUsers_UserId",
                table: "PdfFiles");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PdfFiles",
                table: "PdfFiles");

            migrationBuilder.RenameTable(
                name: "PdfFiles",
                newName: "PdfFile");

            migrationBuilder.RenameIndex(
                name: "IX_PdfFiles_UserId",
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
    }
}
