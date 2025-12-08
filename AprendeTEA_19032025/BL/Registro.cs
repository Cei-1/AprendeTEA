using AprendeTEA_19032025.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace AprendeTEA_19032025.BL
{
    public class Registro
    {
        public static Result Add(Models.Registro registro)
        {
            Result result = new Result();

            try
            {
                using (SqlConnection connection = new SqlConnection(Data.Conexion.GetConnectionString()))
                {
                    string query = "SP_Usuario_Registro"; // Nombre del SP

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // --------- Parámetros de Tbl_Usuarios ----------
                        cmd.Parameters.AddWithValue("@Email", registro.Usuario.Email);
                        cmd.Parameters.AddWithValue("@PasswordHash", registro.Usuario.PasswordHash);
                        cmd.Parameters.AddWithValue("@EmailConfirmado", registro.Usuario.EmailConfirmado);
                        cmd.Parameters.AddWithValue("@EstatusUsuario", registro.Usuario.Estatus);

                        // --------- Parámetros de Tbl_InfoPersonal ----------
                        cmd.Parameters.AddWithValue("@Nombre", registro.InfoPersonal.Nombre);
                        cmd.Parameters.AddWithValue("@ApellidoPaterno", registro.InfoPersonal.ApellidoPaterno ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@ApellidoMaterno", registro.InfoPersonal.ApellidoMaterno ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Telefono", (object?)registro.InfoPersonal.Telefono ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Estado", (object?)registro.InfoPersonal.Estado ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Municipio", (object?)registro.InfoPersonal.Municipio ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Colonia", (object?)registro.InfoPersonal.Colonia ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@FechaNacimiento",
                            registro.InfoPersonal.FechaNacimiento.HasValue
                                ? registro.InfoPersonal.FechaNacimiento.Value
                                : (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Genero", (object?)registro.InfoPersonal.Genero ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@FotoBase64",
                            string.IsNullOrEmpty(registro.InfoPersonal.FotoBase64)
                                ? (object)DBNull.Value
                                : registro.InfoPersonal.FotoBase64);
                        cmd.Parameters.AddWithValue("@EstatusInfo", registro.InfoPersonal.Estatus);

                        // --------- Parámetro OUTPUT para IdUsuario ----------
                        SqlParameter outputIdUsuario = new SqlParameter("@IdUsuarioGenerado", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputIdUsuario);

                        connection.Open();
                        cmd.ExecuteNonQuery();

                        // Leer el valor del OUTPUT
                        int idGenerado = 0;
                        if (outputIdUsuario.Value != DBNull.Value)
                        {
                            idGenerado = Convert.ToInt32(outputIdUsuario.Value);
                        }

                        if (idGenerado > 0)
                        {
                            result.Correct = true;
                            result.Object = idGenerado; // IdUsuario generado
                        }
                        else
                        {
                            result.Correct = false;
                            result.ErrorMessage = "No se insertaron registros o no se generó el IdUsuario.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }
    }
}

