using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraphAlgorithms.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddActionPropertyValueEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActionPropertyValues",
                columns: table => new
                {
                    ActionID = table.Column<int>(type: "int", nullable: false),
                    GraphPropertyID = table.Column<int>(type: "int", nullable: false),
                    PropertyValue = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionPropertyValues", x => new { x.ActionID, x.GraphPropertyID });
                    table.ForeignKey(
                        name: "FK_ActionPropertyValues_Actions_ActionID",
                        column: x => x.ActionID,
                        principalTable: "Actions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActionPropertyValues_GraphProperties_GraphPropertyID",
                        column: x => x.GraphPropertyID,
                        principalTable: "GraphProperties",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActionPropertyValues_GraphPropertyID",
                table: "ActionPropertyValues",
                column: "GraphPropertyID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActionPropertyValues");
        }
    }
}
