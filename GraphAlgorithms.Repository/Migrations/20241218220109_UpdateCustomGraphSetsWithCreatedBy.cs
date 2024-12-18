using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraphAlgorithms.Repository.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCustomGraphSetsWithCreatedBy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedByID",
                table: "CustomGraphSets",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b05a5e47-8a72-4838-a53a-2b04222858fb",
                column: "ConcurrencyStamp",
                value: "25bcb6ec-43f2-4184-9634-36dccf83fdc9");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c9b1c1ae-76aa-45cf-93e1-7b54c6446a01",
                column: "ConcurrencyStamp",
                value: "73e1ae7a-3f32-44d3-bde2-e21103cada84");

            migrationBuilder.CreateIndex(
                name: "IX_CustomGraphSets_CreatedByID",
                table: "CustomGraphSets",
                column: "CreatedByID");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomGraphSets_AspNetUsers_CreatedByID",
                table: "CustomGraphSets",
                column: "CreatedByID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomGraphSets_AspNetUsers_CreatedByID",
                table: "CustomGraphSets");

            migrationBuilder.DropIndex(
                name: "IX_CustomGraphSets_CreatedByID",
                table: "CustomGraphSets");

            migrationBuilder.DropColumn(
                name: "CreatedByID",
                table: "CustomGraphSets");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b05a5e47-8a72-4838-a53a-2b04222858fb",
                column: "ConcurrencyStamp",
                value: "ac7bf699-df6b-4e9a-8acd-5f92bd7e332b");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c9b1c1ae-76aa-45cf-93e1-7b54c6446a01",
                column: "ConcurrencyStamp",
                value: "ae2e9a45-75f0-4d29-821e-ff1a44aa4704");
        }
    }
}
