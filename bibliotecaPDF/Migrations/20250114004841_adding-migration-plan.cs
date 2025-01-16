using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace bibliotecaPDF.Migrations
{
    /// <inheritdoc />
    public partial class addingmigrationplan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserByteAmounts",
                table: "AspNetUsers",
                newName: "ByteAmounts");

            migrationBuilder.AddColumn<int>(
                name: "CapacityPlanId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CapacityPlan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BytesCapacity = table.Column<long>(type: "bigint", nullable: false),
                    PlanName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CapacityPlan", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "CapacityPlan",
                columns: new[] { "Id", "BytesCapacity", "PlanName", "Value" },
                values: new object[,]
                {
                    { 1, 52428800L, "Plano Gratuito", 0.0 },
                    { 2, 1073741824L, "Plano Inicial", 5.0 },
                    { 3, 10737418240L, "Plano Intermediario", 20.0 },
                    { 4, 53687091200L, "Plano Executivo", 50.0 },
                    { 5, 1099511627776L, "Plano Admin", 1000.0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CapacityPlanId",
                table: "AspNetUsers",
                column: "CapacityPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_CapacityPlan_CapacityPlanId",
                table: "AspNetUsers",
                column: "CapacityPlanId",
                principalTable: "CapacityPlan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_CapacityPlan_CapacityPlanId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "CapacityPlan");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CapacityPlanId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CapacityPlanId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "ByteAmounts",
                table: "AspNetUsers",
                newName: "UserByteAmounts");
        }
    }
}
