using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GraphAlgorithms.Repository.Migrations
{
    /// <inheritdoc />
    public partial class SeedActionTypesAndGraphClasses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ActionTypes",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { 1, "Draw" },
                    { 2, "Import" },
                    { 3, "Generate Random" }
                });

            migrationBuilder.InsertData(
                table: "GraphClasses",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { 1, "Tree" },
                    { 2, "Unicyclic Graph" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ActionTypes",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ActionTypes",
                keyColumn: "ID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ActionTypes",
                keyColumn: "ID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "GraphClasses",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "GraphClasses",
                keyColumn: "ID",
                keyValue: 2);
        }
    }
}
