namespace AprendeTEA_19032025.Data
{
    public class Conexion
    {
        public static string GetConnectionString()
        {
            var builer = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string cadenaConexion = builer.GetSection("ConnectionStrings:ConexionSQL").Value;
            return cadenaConexion;
        }
    }
}
