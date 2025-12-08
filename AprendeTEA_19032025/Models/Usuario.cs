using System.ComponentModel.DataAnnotations;

namespace AprendeTEA_19032025.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }

        [Required, EmailAddress, MaxLength(150)]
        public string Email { get; set; }

        //[Required]
        public string PasswordHash { get; set; }

        public bool EmailConfirmado { get; set; } = false;

        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        // -------------------------
        // CAMPOS RECOMENDADOS
        // -------------------------
        public DateTime? FechaActualizacion { get; set; }

        public DateTime? UltimoAcceso { get; set; }

        public bool Estatus { get; set; } = true;

        public int? UsuarioEdita { get; set; }

        public int IdPerfil { get; set; }

        // opcional, pero muy útil para claims y vistas
        public string NombrePerfil { get; set; } //NombrePerfil

        public string EmailConfirmToken { get; set; }
        public DateTime? EmailConfirmTokenExpira { get; set; }
    }
}
