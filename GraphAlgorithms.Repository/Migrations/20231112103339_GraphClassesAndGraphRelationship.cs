using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraphAlgorithms.Repository.Migrations
{
    /// <inheritdoc />
    public partial class GraphClassesAndGraphRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GraphClassGraphXRef",
                columns: table => new
                {
                    GraphClassID = table.Column<int>(type: "int", nullable: false),
                    GraphID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GraphClassGraph", x => new { x.GraphClassID, x.GraphID });
                    table.ForeignKey(
                        name: "FK_GraphClassGraph_GraphClasses_GraphClassID",
                        column: x => x.GraphClassID,
                        principalTable: "GraphClasses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GraphClassGraph_Graphs_GraphID",
                        column: x => x.GraphID,
                        principalTable: "Graphs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GraphClassGraph_GraphsID",
                table: "GraphClassGraphXRef",
                column: "GraphID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GraphClassGraphXRef");
        }
    }
}
