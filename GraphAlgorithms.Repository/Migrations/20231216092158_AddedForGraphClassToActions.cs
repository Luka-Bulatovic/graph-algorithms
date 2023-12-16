using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraphAlgorithms.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddedForGraphClassToActions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ForGraphClassID",
                table: "Actions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Actions_ForGraphClassID",
                table: "Actions",
                column: "ForGraphClassID");

            migrationBuilder.AddForeignKey(
                name: "FK_Actions_GraphClasses_ForGraphClassID",
                table: "Actions",
                column: "ForGraphClassID",
                principalTable: "GraphClasses",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Actions_GraphClasses_ForGraphClassID",
                table: "Actions");

            migrationBuilder.DropIndex(
                name: "IX_Actions_ForGraphClassID",
                table: "Actions");

            migrationBuilder.DropColumn(
                name: "ForGraphClassID",
                table: "Actions");
        }
    }
}
