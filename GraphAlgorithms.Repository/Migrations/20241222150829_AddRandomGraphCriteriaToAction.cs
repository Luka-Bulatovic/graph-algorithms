using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GraphAlgorithms.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddRandomGraphCriteriaToAction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RandomGraphCriteriaID",
                table: "Actions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RandomGraphCriteria",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RandomGraphCriteria", x => x.ID);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b05a5e47-8a72-4838-a53a-2b04222858fb",
                column: "ConcurrencyStamp",
                value: "bd0e4238-e054-4585-ad84-dbef5d92a465");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c9b1c1ae-76aa-45cf-93e1-7b54c6446a01",
                column: "ConcurrencyStamp",
                value: "6d5b261d-f2c1-4d1a-9804-e782b98dad82");

            migrationBuilder.InsertData(
                table: "RandomGraphCriteria",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { 1, "Min. Wiener Index" },
                    { 2, "Max. Wiener Index" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Actions_RandomGraphCriteriaID",
                table: "Actions",
                column: "RandomGraphCriteriaID");

            migrationBuilder.AddForeignKey(
                name: "FK_Actions_RandomGraphCriteria_RandomGraphCriteriaID",
                table: "Actions",
                column: "RandomGraphCriteriaID",
                principalTable: "RandomGraphCriteria",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Actions_RandomGraphCriteria_RandomGraphCriteriaID",
                table: "Actions");

            migrationBuilder.DropTable(
                name: "RandomGraphCriteria");

            migrationBuilder.DropIndex(
                name: "IX_Actions_RandomGraphCriteriaID",
                table: "Actions");

            migrationBuilder.DropColumn(
                name: "RandomGraphCriteriaID",
                table: "Actions");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b05a5e47-8a72-4838-a53a-2b04222858fb",
                column: "ConcurrencyStamp",
                value: "25bcb6ec-43f2-4184-9634-36dccf83fdc9");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c9b1c1ae-76aa-45cf-93e1-7b54c6446a01",
                column: "ConcurrencyStamp",
                value: "73e1ae7a-3f32-44d3-bde2-e21103cada84");
        }
    }
}
