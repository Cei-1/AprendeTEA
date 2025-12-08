using System.ComponentModel.DataAnnotations;

namespace AprendeTEA_19032025.Models
{
    public class InfoPersonal
    {
        public int IdInfo { get; set; }

        [Required]
        public int IdUsuario { get; set; }

        [Required, MaxLength(100)]
        public string Nombre { get; set; }

        [Required, MaxLength(100)]
        public string ApellidoPaterno { get; set; }

        [MaxLength(100)]
        public string ApellidoMaterno { get; set; }

        [MaxLength(20)]
        public string Telefono { get; set; }

        [MaxLength(100)]
        public string Estado { get; set; }

        [MaxLength(100)]
        public string Municipio { get; set; }

        [MaxLength(100)]
        public string Colonia { get; set; }

        [DataType(DataType.Date)]
        public DateTime? FechaNacimiento { get; set; }

        [MaxLength(20)]
        public string Genero { get; set; }

        // -------------------------
        // NUEVO CAMPO PARA LA FOTO
        // -------------------------
        public string FotoBase64 { get; set; }


        // -------------------------
        // CAMPOS DE AUDITORÍA
        // -------------------------
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        public DateTime? FechaEdicion { get; set; }

        public bool Estatus { get; set; } = true;

        // Campo sugerido
        public int? UsuarioEdita { get; set; }

        public Usuario usuario{ get; set; }

    }
}
