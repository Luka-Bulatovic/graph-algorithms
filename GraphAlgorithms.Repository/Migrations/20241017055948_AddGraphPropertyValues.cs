using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraphAlgorithms.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddGraphPropertyValues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GraphPropertyValues",
                columns: table => new
                {
                    GraphID = table.Column<int>(type: "int", nullable: false),
                    GraphPropertyID = table.Column<int>(type: "int", nullable: false),
                    PropertyValue = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GraphPropertyValues", x => new { x.GraphID, x.GraphPropertyID });
                    table.ForeignKey(
                        name: "FK_GraphPropertyValues_GraphProperties_GraphPropertyID",
                        column: x => x.GraphPropertyID,
                        principalTable: "GraphProperties",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GraphPropertyValues_Graphs_GraphID",
                        column: x => x.GraphID,
                        principalTable: "Graphs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GraphPropertyValues_GraphPropertyID",
                table: "GraphPropertyValues",
                column: "GraphPropertyID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GraphPropertyValues");
        }
    }
}
