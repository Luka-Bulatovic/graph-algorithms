using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraphAlgorithms.Repository.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedGraphRelationshipWithActions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Graphs_ActionTypes_ActionTypeID",
                table: "Graphs");

            migrationBuilder.RenameColumn(
                name: "ActionTypeID",
                table: "Graphs",
                newName: "ActionID");

            migrationBuilder.RenameIndex(
                name: "IX_Graphs_ActionTypeID",
                table: "Graphs",
                newName: "IX_Graphs_ActionID");

            migrationBuilder.AddForeignKey(
                name: "FK_Graphs_Actions_ActionID",
                table: "Graphs",
                column: "ActionID",
                principalTable: "Actions",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Graphs_Actions_ActionID",
                table: "Graphs");

            migrationBuilder.RenameColumn(
                name: "ActionID",
                table: "Graphs",
                newName: "ActionTypeID");

            migrationBuilder.RenameIndex(
                name: "IX_Graphs_ActionID",
                table: "Graphs",
                newName: "IX_Graphs_ActionTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Graphs_ActionTypes_ActionTypeID",
                table: "Graphs",
                column: "ActionTypeID",
                principalTable: "ActionTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
