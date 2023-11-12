using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraphAlgorithms.Repository.Migrations
{
    /// <inheritdoc />
    public partial class ActionGraphRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActionGraphXRef",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActionID = table.Column<int>(type: "int", nullable: false),
                    GraphID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionGraph", x => new { x.ActionID, x.GraphID });
                    table.ForeignKey(
                        name: "FK_ActionGraph_Actions_ActionID",
                        column: x => x.ActionID,
                        principalTable: "Actions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActionGraph_Graphs_GraphID",
                        column: x => x.GraphID,
                        principalTable: "Graphs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActionGraph_GraphID",
                table: "ActionGraphXRef",
                column: "GraphID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActionGraphXRef");
        }
    }
}
