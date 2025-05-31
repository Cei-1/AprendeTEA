using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AprendeTEA_19032025.Data.Migrations
{
    /// <inheritdoc />
    public partial class PlanTrabajo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlanTrabajo",
                columns: table => new
                {
                    IdPlanTrabajo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombrePlan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Estatus = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanTrabajo", x => x.IdPlanTrabajo);
                });

            migrationBuilder.CreateTable(
                name: "Unidad",
                columns: table => new
                {
                    IdUnidad = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdPlanTrabajo = table.Column<int>(type: "int", nullable: false),
                    NombreUnidad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Detalle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Responsable = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Estatus = table.Column<bool>(type: "bit", nullable: false),
                    PlanTrabajoIdPlanTrabajo = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Unidad", x => x.IdUnidad);
                    table.ForeignKey(
                        name: "FK_Unidad_PlanTrabajo_PlanTrabajoIdPlanTrabajo",
                        column: x => x.PlanTrabajoIdPlanTrabajo,
                        principalTable: "PlanTrabajo",
                        principalColumn: "IdPlanTrabajo");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Unidad_PlanTrabajoIdPlanTrabajo",
                table: "Unidad",
                column: "PlanTrabajoIdPlanTrabajo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Unidad");

            migrationBuilder.DropTable(
                name: "PlanTrabajo");
        }
    }
}
