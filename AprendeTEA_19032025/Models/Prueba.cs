using System.ComponentModel.DataAnnotations;

namespace AprendeTEA_19032025.Models
{
    public class Prueba
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nombre necesario")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Nombre necesario")]
        public string Apellido { get; set; }
    }
}
