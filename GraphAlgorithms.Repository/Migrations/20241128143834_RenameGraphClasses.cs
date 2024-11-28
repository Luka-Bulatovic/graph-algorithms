using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraphAlgorithms.Repository.Migrations
{
    /// <inheritdoc />
    public partial class RenameGraphClasses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "GraphClasses",
                keyColumn: "ID",
                keyValue: 1,
                column: "Name",
                value: "Connected");

            migrationBuilder.UpdateData(
                table: "GraphClasses",
                keyColumn: "ID",
                keyValue: 2,
                column: "Name",
                value: "Unicyclic Bipartite");

            migrationBuilder.UpdateData(
                table: "GraphClasses",
                keyColumn: "ID",
                keyValue: 4,
                column: "Name",
                value: "Unicyclic");

            migrationBuilder.UpdateData(
                table: "GraphClasses",
                keyColumn: "ID",
                keyValue: 5,
                column: "Name",
                value: "Bipartite");

            migrationBuilder.UpdateData(
                table: "GraphClasses",
                keyColumn: "ID",
                keyValue: 6,
                column: "Name",
                value: "Acyclic With Fixed Diameter");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.UpdateData(
                table: "GraphClasses",
                keyColumn: "ID",
                keyValue: 4,
                column: "Name",
                value: "Unicyclic Graph");

            migrationBuilder.UpdateData(
                table: "GraphClasses",
                keyColumn: "ID",
                keyValue: 5,
                column: "Name",
                value: "Bipartite Graph");

            migrationBuilder.UpdateData(
                table: "GraphClasses",
                keyColumn: "ID",
                keyValue: 6,
                column: "Name",
                value: "Acyclic Graph With Fixed Diameter");
        }
    }
}
