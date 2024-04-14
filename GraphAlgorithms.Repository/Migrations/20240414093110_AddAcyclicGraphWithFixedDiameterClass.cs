using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraphAlgorithms.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddAcyclicGraphWithFixedDiameterClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "GraphClasses",
                columns: new[] { "ID", "CanGenerateRandomGraphs", "Name" },
                values: new object[] { 6, true, "Acyclic Graph With Fixed Diameter" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "GraphClasses",
                keyColumn: "ID",
                keyValue: 6);
        }
    }
}
