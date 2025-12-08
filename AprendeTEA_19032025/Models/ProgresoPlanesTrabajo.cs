using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AprendeTEA_19032025.Models
{
    /// <summary>
    /// Modelo para el progreso de planes de trabajo por usuario
    /// Mapea los resultados de SP_Progreso_PlanesDeTrabajo
    /// </summary>
    public class ProgresoPlanesTrabajo
    {
        [Key]
        public int IdPlanTrabajo { get; set; }
        public string? NombrePlan { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public bool Estatus { get; set; }
        public int TotalUnidades { get; set; }
        public int UnidadesCompletadas { get; set; }
        public decimal PorcentajeProgreso { get; set; }

        /// <summary>
        /// Lista de planes de trabajo con progreso (para la vista)
        /// </summary>
        [NotMapped]
        public List<ProgresoPlanesTrabajo>? PlanesTrabajo { get; set; }
    }
}
