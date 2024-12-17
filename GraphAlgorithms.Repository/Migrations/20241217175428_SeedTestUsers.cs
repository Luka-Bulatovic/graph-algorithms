using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GraphAlgorithms.Repository.Migrations
{
    /// <inheritdoc />
    public partial class SeedTestUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "b05a5e47-8a72-4838-a53a-2b04222858fb", 0, "1527f655-de75-48cf-8560-5f28a4721a00", "zana@graphs.com", true, false, null, "ZANA@GRAPHS.COM", "ZANA", "AQAAAAIAAYagAAAAEOsVGxeoyKOkqR1GbtTt66R1Y7sZjXvvXvh1pjjwVlzxlLQBU3NZpPWffgauQPGPYw==", null, false, "", false, "zana" },
                    { "c9b1c1ae-76aa-45cf-93e1-7b54c6446a01", 0, "c9f1d1be-8905-43ff-b7d5-a83e4916e0f4", "luka@graphs.com", true, false, null, "LUKA@GRAPHS.COM", "LUKA", "AQAAAAIAAYagAAAAEPywUphZYSOThwHJm+zMnei4mrv0rF+QKd91ePCk+CeC0k3yBPDCtcYvf56LzC7fBQ==", null, false, "", false, "luka" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b05a5e47-8a72-4838-a53a-2b04222858fb");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c9b1c1ae-76aa-45cf-93e1-7b54c6446a01");
        }
    }
}
