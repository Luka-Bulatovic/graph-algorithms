using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GraphAlgorithms.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddMaxAndMinDegreeProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "GraphProperties",
                columns: new[] { "ID", "Description", "IsGeneralDisplayProperty", "Name" },
                values: new object[,]
                {
                    { 11, "", true, "Min. Node Degree" },
                    { 12, "", true, "Max. Node Degree" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "GraphProperties",
                keyColumn: "ID",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "GraphProperties",
                keyColumn: "ID",
                keyValue: 12);
        }
    }
}
