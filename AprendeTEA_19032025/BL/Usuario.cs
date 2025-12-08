using AprendeTEA_19032025.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace AprendeTEA_19032025.BL
{
    public class Usuario
    {
        public static Result GetByEmail(string email)
        {
            Result result = new Result();

            try
            {
                using (SqlConnection connection = new SqlConnection(Data.Conexion.GetConnectionString()))
                {
                    string query = "SP_Usuario_GetByEmail"; // SP que consulta por email

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Email", email);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable table = new DataTable();
                            adapter.Fill(table);

                            if (table.Rows.Count > 0)
                            {
                                DataRow row = table.Rows[0];

                                Models.Usuario usuario = new Models.Usuario
                                {
                                    IdUsuario = Convert.ToInt32(row["IdUsuario"]),
                                    Email = row["Email"].ToString(),
                                    PasswordHash = row["PasswordHash"].ToString(),
                                    EmailConfirmado = Convert.ToBoolean(row["EmailConfirmado"]),
                                    FechaRegistro = Convert.ToDateTime(row["FechaRegistro"]),
                                    Estatus = Convert.ToBoolean(row["Estatus"]),
                                    IdPerfil = Convert.ToInt32(row["IdPerfil"]),
                                    NombrePerfil = row["NombrePerfil"].ToString()
                                };

                                result.Object = usuario;
                                result.Correct = true;
                            }
                            else
                            {
                                result.Correct = false;
                                result.ErrorMessage = "Usuario no encontrado.";
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
    }
}
