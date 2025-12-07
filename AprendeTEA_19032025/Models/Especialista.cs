using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AprendeTEA_19032025.Models
{
    public class Especialista
    {
        [Key]
        public int IdEspecialista { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido paterno es requerido")]
        [StringLength(100)]
        public string ApellidoPaterno { get; set; }

        [StringLength(100)]
        public string? ApellidoMaterno { get; set; }

        [StringLength(100)]
        public string? Estado { get; set; }

        [StringLength(100)]
        public string? Municipio { get; set; }

        [StringLength(100)]
        public string? Colonia { get; set; }

        [StringLength(20)]
        public string? Telefono { get; set; }

        [EmailAddress(ErrorMessage = "Email inválido")]
        [StringLength(150)]
        public string? Email { get; set; }

        public DateTime? FechaRegistro { get; set; }

        public string? FotografiaBase64 { get; set; }

        // Property for GetAll results
        [NotMapped]
        public List<object>? Especialistas { get; set; }

        // Helper property for full name
        [NotMapped]
        public string NombreCompleto => $"{Nombre} {ApellidoPaterno} {ApellidoMaterno}".Trim();
    }
}
