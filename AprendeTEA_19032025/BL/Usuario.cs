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

        public static Result ConfirmarEmail(int idUsuario, string token)
        {
            Result result = new Result();

            try
            {
                using (SqlConnection conn = new SqlConnection(Data.Conexion.GetConnectionString()))
                {
                    string query = "SP_Usuario_ConfirmEmail";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                    cmd.Parameters.AddWithValue("@Token", token);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    result.Correct = true;
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        public static Result GetPendientesActivar()
        {
            Result result = new Result();

            try
            {
                using (SqlConnection conn = new SqlConnection(Data.Conexion.GetConnectionString()))
                {
                    string query = "SP_Usuario_GetPendientesActivar";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);

                            result.Objects = new List<object>();

                            foreach (DataRow row in dt.Rows)
                            {
                                var usuario = new Models.Usuario
                                {
                                    IdUsuario = Convert.ToInt32(row["IdUsuario"]),
                                    Email = row["Email"].ToString(),
                                    FechaRegistro = Convert.ToDateTime(row["FechaRegistro"]),
                                    EmailConfirmado = Convert.ToBoolean(row["EmailConfirmado"]),
                                    EmailConfirmToken = row["EmailConfirmToken"] as string,
                                    EmailConfirmTokenExpira = row["EmailConfirmTokenExpira"] == DBNull.Value
                                        ? (DateTime?)null
                                        : Convert.ToDateTime(row["EmailConfirmTokenExpira"])
                                };

                                result.Objects.Add(usuario);
                            }

                            result.Correct = true;
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


        public static void LimpiarTokensExpirados()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Data.Conexion.GetConnectionString()))
                using (SqlCommand cmd = new SqlCommand("SP_LimpiarTokensExpirados", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                // Puedes loguear si quieres
                // pero nunca lanzar la excepción porque Hangfire sigue ejecutando jobs
            }
        }


        //
        // 🔹 Obtener un usuario por Id
        public static Result GetById(int idUsuario)
        {
            Result result = new Result();

            try
            {
                using (SqlConnection conn = new SqlConnection(Data.Conexion.GetConnectionString()))
                using (SqlCommand cmd = new SqlCommand("SP_Usuario_GetById", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);

                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var usuario = new Models.Usuario
                            {
                                IdUsuario = (int)reader["IdUsuario"],
                                Email = reader["Email"].ToString(),
                                PasswordHash = reader["PasswordHash"].ToString(),
                                EmailConfirmado = Convert.ToBoolean(reader["EmailConfirmado"]),
                                Estatus = Convert.ToBoolean(reader["Estatus"]),
                                NombrePerfil = reader["Nombre"]?.ToString()
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
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        // 🔹 Lista de usuarios pendientes de activar
        public static Result GetPendientesActivarPrueba()
        {
            Result result = new Result();

            try
            {
                using (SqlConnection conn = new SqlConnection(Data.Conexion.GetConnectionString()))
                using (SqlCommand cmd = new SqlCommand("SP_Usuarios_PendientesActivar", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable table = new DataTable();
                        da.Fill(table);

                        result.Objects = new List<object>();

                        foreach (DataRow row in table.Rows)
                        {
                            var usuario = new Models.Usuario
                            {
                                IdUsuario = Convert.ToInt32(row["IdUsuario"]),
                                Email = row["Email"].ToString(),
                                EmailConfirmado = Convert.ToBoolean(row["EmailConfirmado"]),
                                Estatus = Convert.ToBoolean(row["Estatus"]),
                                NombrePerfil = row.Table.Columns.Contains("Nombre")
                                    ? row["Nombre"]?.ToString()
                                    : null,
                                FechaRegistro = row.Table.Columns.Contains("FechaRegistro")
                                    ? Convert.ToDateTime(row["FechaRegistro"])
                                    : DateTime.MinValue
                            };

                            result.Objects.Add(usuario);
                        }

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

        // 🔹 Confirmar email manual (admin)
        public static Result ConfirmarEmailManual(int idUsuario)
        {
            Result result = new Result();

            try
            {
                using (SqlConnection conn = new SqlConnection(Data.Conexion.GetConnectionString()))
                using (SqlCommand cmd = new SqlCommand("SP_Usuario_ConfirmEmailManual", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);

                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();

                    //result.Correct = rows > 0;
                    //if (!result.Correct)
                    //    result.ErrorMessage = "No se pudo activar el usuario.";
                    result.Correct = true;

                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        // 🔹 Actualizar / regenerar token de confirmación
        public static Result ActualizarTokenConfirmacion(int idUsuario, string token)
        {
            Result result = new Result();

            try
            {
                using (SqlConnection conn = new SqlConnection(Data.Conexion.GetConnectionString()))
                using (SqlCommand cmd = new SqlCommand("SP_Usuario_ActualizarTokenConfirmacion", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                    cmd.Parameters.AddWithValue("@Token", token);

                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();

                    //result.Correct = rows > 0;
                    //if (!result.Correct)
                    //    result.ErrorMessage = "No se pudo actualizar el token de confirmación.";
                    result.Correct = true;
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
