using System.ComponentModel.DataAnnotations;

namespace AprendeTEA_19032025.Models
{
    /// <summary>
    /// Modelo para representar el progreso de unidades por plan de trabajo
    /// Mapea el resultado de SP_Unidades_ProgresoPorPlan
    /// </summary>
    public class UnidadProgreso
    {
        public int IdUnidad { get; set; }
        public int IdPlanTrabajo { get; set; }
        public string Objetivo { get; set; }
        public string Unidad { get; set; } // Nombre de la unidad
        public string Detalles { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public bool Estatus { get; set; }
        
        // Campos de progreso
        public bool Completada { get; set; }
        public decimal? CalificacionObtenida { get; set; }
        public decimal? TiempoDedicado { get; set; }

        // Campos de actividades (para mostrar qué actividades tiene disponibles)
        public bool TieneSopaLetras { get; set; }
        public bool TieneCrucigrama { get; set; }
        public bool TieneRelacionar { get; set; }
        public bool TieneAgrupacion { get; set; }
        public bool TieneOrdenar { get; set; }

        // Lista para el ViewBag
        public List<UnidadProgreso> Unidades { get; set; }
    }
}
