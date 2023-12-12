using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GraphAlgorithms.Repository.Migrations
{
    /// <inheritdoc />
    public partial class ChangedSeederForGraphClasses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "GraphClasses",
                keyColumn: "ID",
                keyValue: 1,
                column: "Name",
                value: "Connected Graph");

            migrationBuilder.UpdateData(
                table: "GraphClasses",
                keyColumn: "ID",
                keyValue: 2,
                column: "Name",
                value: "Unicyclic Bipartite Graph");

            migrationBuilder.InsertData(
                table: "GraphClasses",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { 3, "Tree" },
                    { 4, "Unicyclic Graph" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "GraphClasses",
                keyColumn: "ID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "GraphClasses",
                keyColumn: "ID",
                keyValue: 4);

            migrationBuilder.UpdateData(
                table: "GraphClasses",
                keyColumn: "ID",
                keyValue: 1,
                column: "Name",
                value: "Tree");

            migrationBuilder.UpdateData(
                table: "GraphClasses",
                keyColumn: "ID",
                keyValue: 2,
                column: "Name",
                value: "Unicyclic Graph");
        }
    }
}
