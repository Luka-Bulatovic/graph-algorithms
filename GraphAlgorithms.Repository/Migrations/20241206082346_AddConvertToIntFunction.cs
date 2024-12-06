using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraphAlgorithms.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddConvertToIntFunction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Ovdje unesite SQL za kreiranje funkcije
            migrationBuilder.Sql(@"
            CREATE FUNCTION [dbo].[ConvertToInt](@value nvarchar(max))
            RETURNS int
            AS
            BEGIN
                RETURN CAST(@value as int);
            END;
        ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Ovdje obrišite funkciju u slučaju roll-backa migracije
            migrationBuilder.Sql("DROP FUNCTION [dbo].[ConvertToInt];");
        }
    }
}
