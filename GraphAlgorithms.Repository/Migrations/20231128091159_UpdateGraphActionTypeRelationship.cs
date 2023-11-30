using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraphAlgorithms.Repository.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGraphActionTypeRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActionGraphXRef");

            migrationBuilder.DropTable(
                name: "Actions");

            migrationBuilder.AddColumn<int>(
                name: "ActionTypeID",
                table: "Graphs",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Graphs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Graphs",
                type: "datetime2",
                nullable: true);


            migrationBuilder.AddForeignKey(
                name: "FK_Graphs_ActionTypes_ActionTypeID",
                table: "Graphs",
                column: "ActionTypeID",
                principalTable: "ActionTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.CreateIndex(
                name: "IX_Graphs_ActionTypeID",
                table: "Graphs",
                column: "ActionTypeID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Graphs_ActionTypes_ActionTypeID",
                table: "Graphs");

            migrationBuilder.DropIndex(
                name: "IX_Graphs_ActionTypeID",
                table: "Graphs");

            migrationBuilder.DropColumn(
                name: "ActionTypeID",
                table: "Graphs");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Graphs");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Graphs");

            migrationBuilder.CreateTable(
                name: "Actions",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActionTypeID = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Actions_ActionTypes_ActionTypeID",
                        column: x => x.ActionTypeID,
                        principalTable: "ActionTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_Actions_ActionTypeID",
                table: "Actions",
                column: "ActionTypeID");

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
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_ActionGraph_GraphID",
                table: "ActionGraphXRef",
                column: "GraphID");

            migrationBuilder.CreateIndex(
                name: "IX_Actions_ActionTypeID",
                table: "Actions",
                column: "ActionTypeID");
        }
    }
}
