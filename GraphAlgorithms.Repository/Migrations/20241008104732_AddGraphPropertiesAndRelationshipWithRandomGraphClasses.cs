using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GraphAlgorithms.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddGraphPropertiesAndRelationshipWithRandomGraphClasses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GraphProperties",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(250)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: ""),
                    IsGeneralDisplayProperty = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GraphProperties", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "RandomGenerationGraphClassPropertyXRef",
                columns: table => new
                {
                    GraphClassID = table.Column<int>(type: "int", nullable: false),
                    GraphPropertyID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RandomGenerationGraphClassPropertyXRef", x => new { x.GraphClassID, x.GraphPropertyID });
                    table.ForeignKey(
                        name: "FK_RandomGenerationGraphClassPropertyXRef_GraphClasses_GraphClassID",
                        column: x => x.GraphClassID,
                        principalTable: "GraphClasses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RandomGenerationGraphClassPropertyXRef_GraphProperties_GraphPropertyID",
                        column: x => x.GraphPropertyID,
                        principalTable: "GraphProperties",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "GraphProperties",
                columns: new[] { "ID", "Description", "IsGeneralDisplayProperty", "Name" },
                values: new object[,]
                {
                    { 1, "", true, "Order" },
                    { 2, "Minimum percentage of all possible edges", false, "Min. Edges Coefficient" },
                    { 3, "", true, "Diameter" },
                    { 4, "Length of cycle in unicyclic graph", false, "Cycle Length" },
                    { 5, "", false, "First Partition Size" },
                    { 6, "", false, "Second Partition Size" },
                    { 7, "", true, "Radius" },
                    { 8, "", true, "Size/Order Ratio" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_RandomGenerationGraphClassPropertyXRef_GraphPropertyID",
                table: "RandomGenerationGraphClassPropertyXRef",
                column: "GraphPropertyID");
            
            migrationBuilder.CreateIndex(
                name: "IX_RandomGenerationGraphClassPropertyXRef_GraphClassID",
                table: "RandomGenerationGraphClassPropertyXRef",
                column: "GraphClassID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RandomGenerationGraphClassPropertyXRef");

            migrationBuilder.DropTable(
                name: "GraphProperties");
        }
    }
}
