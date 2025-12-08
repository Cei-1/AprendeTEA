using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AprendeTEA_19032025.Models
{
    public class Registro
    {
        public Usuario Usuario { get; set; } = new Usuario();
        public InfoPersonal InfoPersonal { get; set; } = new InfoPersonal();

        // Para contraseña en texto plano (antes de hacer hash)
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmPassword { get; set; }

        public List<SelectListItem> Estados { get; set; } = new();

    }
}
