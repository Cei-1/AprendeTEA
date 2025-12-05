using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AprendeTEA_19032025.Models
{
    public class PlanTrabajo
    {
        [Key]
        public int IdPlanTrabajo { get; set; }
        public string NombrePlan { get; set; }
        public string Objetivo { get; set; }

        public DateTime? FechaRegistro { get; set; }
        public bool Estatus { get; set; }

        // Relación con unidades
        public List<Unidad> Unidades { get; set; }
        
        [NotMapped]
        public List<object>? PlanesTrabajo { get; set; }

    }
}   
