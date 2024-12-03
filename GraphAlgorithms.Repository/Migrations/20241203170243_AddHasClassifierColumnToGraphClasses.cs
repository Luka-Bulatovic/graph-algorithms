using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraphAlgorithms.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddHasClassifierColumnToGraphClasses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasClassifier",
                table: "GraphClasses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "GraphClasses",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CanGenerateRandomGraphs", "HasClassifier" },
                values: new object[] { true, true });

            migrationBuilder.UpdateData(
                table: "GraphClasses",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "CanGenerateRandomGraphs", "HasClassifier" },
                values: new object[] { true, false });

            migrationBuilder.UpdateData(
                table: "GraphClasses",
                keyColumn: "ID",
                keyValue: 3,
                column: "HasClassifier",
                value: true);

            migrationBuilder.UpdateData(
                table: "GraphClasses",
                keyColumn: "ID",
                keyValue: 4,
                column: "HasClassifier",
                value: true);

            migrationBuilder.UpdateData(
                table: "GraphClasses",
                keyColumn: "ID",
                keyValue: 5,
                column: "HasClassifier",
                value: true);

            migrationBuilder.UpdateData(
                table: "GraphClasses",
                keyColumn: "ID",
                keyValue: 6,
                columns: new[] { "CanGenerateRandomGraphs", "HasClassifier" },
                values: new object[] { true, false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasClassifier",
                table: "GraphClasses");

            migrationBuilder.UpdateData(
                table: "GraphClasses",
                keyColumn: "ID",
                keyValue: 1,
                column: "CanGenerateRandomGraphs",
                value: false);

            migrationBuilder.UpdateData(
                table: "GraphClasses",
                keyColumn: "ID",
                keyValue: 2,
                column: "CanGenerateRandomGraphs",
                value: false);

            migrationBuilder.UpdateData(
                table: "GraphClasses",
                keyColumn: "ID",
                keyValue: 6,
                column: "CanGenerateRandomGraphs",
                value: false);
        }
    }
}
