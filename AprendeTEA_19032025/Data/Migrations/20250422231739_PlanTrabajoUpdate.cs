using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AprendeTEA_19032025.Data.Migrations
{
    /// <inheritdoc />
    public partial class PlanTrabajoUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FechaRegistro",
                table: "PlanTrabajo",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaRegistro",
                table: "PlanTrabajo");
        }
    }
}
