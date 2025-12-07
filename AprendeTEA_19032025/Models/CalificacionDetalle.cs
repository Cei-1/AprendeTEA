using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AprendeTEA_19032025.Models
{
    public class CalificacionDetalle
    {
        [Key]
        public int IdCalificacion { get; set; }

        public int IdUsuario { get; set; }

        [StringLength(300)]
        public string? NombreCompleto { get; set; }

        public int IdUnidad { get; set; }

        [StringLength(200)]
        public string? Unidad { get; set; }

        public decimal? TiempoDedicado { get; set; }

        public decimal? Calificacion { get; set; }

        public DateTime? FechaRegistro { get; set; }

        // Property for GetAll results
[NotMapped]
public List<CalificacionDetalle>? Calificaciones { get; set; }

    }
}
