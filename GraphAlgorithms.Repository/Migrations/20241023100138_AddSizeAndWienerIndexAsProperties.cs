using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GraphAlgorithms.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddSizeAndWienerIndexAsProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "GraphProperties",
                columns: new[] { "ID", "Description", "IsGeneralDisplayProperty", "Name" },
                values: new object[,]
                {
                    { 9, "Number of edges in graph", true, "Size" },
                    { 10, "Wiener Index of a graph", true, "Wiener Index" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "GraphProperties",
                keyColumn: "ID",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "GraphProperties",
                keyColumn: "ID",
                keyValue: 10);
        }
    }
}
