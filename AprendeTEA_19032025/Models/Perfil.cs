namespace AprendeTEA_19032025.Models
{
    public class Perfil
    {
        // Datos de usuario
        public int IdUsuario { get; set; }
        public string Email { get; set; }
        public string NombrePerfil { get; set; } // Admin / Usuario (opcional)

        // Datos personales
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }

        public string Telefono { get; set; }
        public string Estado { get; set; }
        public string Municipio { get; set; }
        public string Colonia { get; set; }

        public DateTime FechaRegistro { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string Genero { get; set; }

        public string FotoBase64 { get; set; }

        // Propiedad comodín
        public string NombreCompleto =>
            $"{Nombre} {ApellidoPaterno} {ApellidoMaterno}".Trim();
    }
}
