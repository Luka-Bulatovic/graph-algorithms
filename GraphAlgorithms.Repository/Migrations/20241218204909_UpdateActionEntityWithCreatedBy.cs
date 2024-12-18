using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraphAlgorithms.Repository.Migrations
{
    /// <inheritdoc />
    public partial class UpdateActionEntityWithCreatedBy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CreatedByID",
                table: "Actions",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

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

            migrationBuilder.CreateIndex(
                name: "IX_Actions_CreatedByID",
                table: "Actions",
                column: "CreatedByID");

            migrationBuilder.AddForeignKey(
                name: "FK_Actions_AspNetUsers_CreatedByID",
                table: "Actions",
                column: "CreatedByID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Actions_AspNetUsers_CreatedByID",
                table: "Actions");

            migrationBuilder.DropIndex(
                name: "IX_Actions_CreatedByID",
                table: "Actions");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedByID",
                table: "Actions",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b05a5e47-8a72-4838-a53a-2b04222858fb",
                column: "ConcurrencyStamp",
                value: "1527f655-de75-48cf-8560-5f28a4721a00");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c9b1c1ae-76aa-45cf-93e1-7b54c6446a01",
                column: "ConcurrencyStamp",
                value: "c9f1d1be-8905-43ff-b7d5-a83e4916e0f4");
        }
    }
}
