using System.ComponentModel.DataAnnotations;

namespace AprendeTEA_19032025.Models
{
    public class Admin
    {
        [Key]
        public int IdAdmin { get; set; }

        [Required(ErrorMessage = "El correo electronico es obligatorio")]
        public string Email { get; set; }
    }
}
