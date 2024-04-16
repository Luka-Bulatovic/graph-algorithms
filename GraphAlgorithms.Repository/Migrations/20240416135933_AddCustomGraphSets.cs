using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraphAlgorithms.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomGraphSets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomGraphSets",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomGraphSets", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CustomGraphSetGraphXRef",
                columns: table => new
                {
                    CustomGraphSetID = table.Column<int>(type: "int", nullable: false),
                    GraphID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomGraphSetGraphXRef", x => new { x.CustomGraphSetID, x.GraphID });
                    table.ForeignKey(
                        name: "FK_CustomGraphSetGraphXRef_CustomGraphSets_CustomGraphSetID",
                        column: x => x.CustomGraphSetID,
                        principalTable: "CustomGraphSets",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomGraphSetGraphXRef_Graphs_GraphID",
                        column: x => x.GraphID,
                        principalTable: "Graphs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomGraphSetGraphXRef_GraphID",
                table: "CustomGraphSetGraphXRef",
                column: "GraphID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomGraphSetGraphXRef");

            migrationBuilder.DropTable(
                name: "CustomGraphSets");
        }
    }
}
