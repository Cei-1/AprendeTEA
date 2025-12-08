using AprendeTEA_19032025.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace AprendeTEA_19032025.BL
{
    public class Perfil
    {
        public static Result GetPerfilByIdUsuario(int idUsuario)
        {
            Result result = new Result();

            try
            {
                using (SqlConnection connection = new SqlConnection(Data.Conexion.GetConnectionString()))
                {
                    string query = "SP_Perfil_GetByIdUsuario";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable table = new DataTable();
                            adapter.Fill(table);

                            if (table.Rows.Count > 0)
                            {
                                var row = table.Rows[0];

                                var perfil = new Models.Perfil
                                {
                                    IdUsuario = idUsuario,
                                    Email = row["Email"].ToString(),
                                    NombrePerfil = row["NombrePerfil"].ToString(),
                                    Nombre = row["Nombre"].ToString(),
                                    ApellidoPaterno = row["ApellidoPaterno"].ToString(),
                                    ApellidoMaterno = row["ApellidoMaterno"].ToString(),
                                    Telefono = row["Telefono"].ToString(),
                                    Estado = row["Estado"].ToString(),
                                    Municipio = row["Municipio"].ToString(),
                                    Colonia = row["Colonia"].ToString(),
                                    Genero = row["Genero"].ToString(),
                                    FotoBase64 = row["FotoBase64"] == DBNull.Value ? null : row["FotoBase64"].ToString(),
                                    FechaNacimiento = row["FechaNacimiento"] == DBNull.Value
                                        ? (DateTime?)null
                                        : Convert.ToDateTime(row["FechaNacimiento"]),
                                    FechaRegistro = Convert.ToDateTime(row["FechaRegistro"])
                                };

                                result.Object = perfil;
                                result.Correct = true;
                            }
                            else
                            {
                                result.Correct = false;
                                result.ErrorMessage = "No se encontró información de perfil.";
                            }
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

        public static Result UpdatePerfil(Models.Perfil perfil)
        {
            Result result = new Result();

            try
            {
                using (SqlConnection connection = new SqlConnection(Data.Conexion.GetConnectionString()))
                {
                    string query = "SP_Perfil_UpdateInfo";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@IdUsuario", perfil.IdUsuario);
                        cmd.Parameters.AddWithValue("@Telefono", (object?)perfil.Telefono ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Estado", (object?)perfil.Estado ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Municipio", (object?)perfil.Municipio ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Colonia", (object?)perfil.Colonia ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@FotoBase64", (object?)perfil.FotoBase64 ?? DBNull.Value); // 🔹 NUEVO

                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();

                        // Si NO hubo excepción y llega aquí, la operación fue exitosa.
                        result.Correct = true;
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
