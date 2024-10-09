using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GraphAlgorithms.Repository.Migrations
{
    /// <inheritdoc />
    public partial class SeedRandomGenerationGraphClassPropertyXRef : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "RandomGenerationGraphClassPropertyXRef",
                columns: new[] { "GraphClassID", "GraphPropertyID" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 2, 4 },
                    { 2, 5 },
                    { 2, 6 },
                    { 6, 1 },
                    { 6, 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RandomGenerationGraphClassPropertyXRef",
                keyColumns: new[] { "GraphClassID", "GraphPropertyID" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "RandomGenerationGraphClassPropertyXRef",
                keyColumns: new[] { "GraphClassID", "GraphPropertyID" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "RandomGenerationGraphClassPropertyXRef",
                keyColumns: new[] { "GraphClassID", "GraphPropertyID" },
                keyValues: new object[] { 2, 4 });

            migrationBuilder.DeleteData(
                table: "RandomGenerationGraphClassPropertyXRef",
                keyColumns: new[] { "GraphClassID", "GraphPropertyID" },
                keyValues: new object[] { 2, 5 });

            migrationBuilder.DeleteData(
                table: "RandomGenerationGraphClassPropertyXRef",
                keyColumns: new[] { "GraphClassID", "GraphPropertyID" },
                keyValues: new object[] { 2, 6 });

            migrationBuilder.DeleteData(
                table: "RandomGenerationGraphClassPropertyXRef",
                keyColumns: new[] { "GraphClassID", "GraphPropertyID" },
                keyValues: new object[] { 6, 1 });

            migrationBuilder.DeleteData(
                table: "RandomGenerationGraphClassPropertyXRef",
                keyColumns: new[] { "GraphClassID", "GraphPropertyID" },
                keyValues: new object[] { 6, 3 });
        }
    }
}
