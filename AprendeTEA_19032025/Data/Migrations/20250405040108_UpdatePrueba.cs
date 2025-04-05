using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AprendeTEA_19032025.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePrueba : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Apellido",
                table: "Prueba",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Apellido",
                table: "Prueba");
        }
    }
}
