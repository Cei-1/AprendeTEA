using System.ComponentModel.DataAnnotations;

namespace AprendeTEA_19032025.Models
{
    public class User
    {
        [Key]
        public int IdUser { get; set; }

        [Required(ErrorMessage = "El correo electronico es obligatorio")]
        public string Email { get; set; }
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Pass { get; set; }
    }
}
