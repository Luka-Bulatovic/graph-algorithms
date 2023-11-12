using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraphAlgorithms.Repository.Migrations
{
    /// <inheritdoc />
    public partial class ActionTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Graphs",
                type: "nvarchar(500)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ActionTypes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActionTypeName = table.Column<string>(type: "nvarchar(250)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionTypes", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActionTypes");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Graphs");
        }
    }
}
