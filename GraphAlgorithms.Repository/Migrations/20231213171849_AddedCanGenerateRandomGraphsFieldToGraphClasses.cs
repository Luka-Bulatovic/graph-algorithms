using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraphAlgorithms.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddedCanGenerateRandomGraphsFieldToGraphClasses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CanGenerateRandomGraphs",
                table: "GraphClasses",
                type: "bit",
                nullable: false,
                defaultValue: false);

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
                keyValue: 3,
                column: "CanGenerateRandomGraphs",
                value: false);

            migrationBuilder.UpdateData(
                table: "GraphClasses",
                keyColumn: "ID",
                keyValue: 4,
                column: "CanGenerateRandomGraphs",
                value: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanGenerateRandomGraphs",
                table: "GraphClasses");
        }
    }
}
