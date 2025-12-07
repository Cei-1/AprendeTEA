using AprendeTEA_19032025.Data;
using AprendeTEA_19032025.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace AprendeTEA_19032025.BL
{
    public class Especialista
    {
        private readonly ApplicationDbContext _context;

        public Especialista(ApplicationDbContext context)
        {
            _context = context;
        }

        public static Models.Result GetAll()
        {
            Models.Result result = new Models.Result();
            try
            {
                using (SqlConnection connection = new SqlConnection(Data.Conexion.GetConnectionString()))
                {
                    string query = "SP_Especialistas_GetAll";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable table = new DataTable();
                            adapter.Fill(table);

                            result.Objects = new List<object>();

                            foreach (DataRow row in table.Rows)
                            {
                                Models.Especialista especialista = new Models.Especialista
                                {
                                    IdEspecialista = Convert.ToInt32(row["IdEspecialista"]),
                                    Nombre = row["Nombre"].ToString(),
                                    ApellidoPaterno = row["ApellidoPaterno"].ToString(),
                                    ApellidoMaterno = row["ApellidoMaterno"] != DBNull.Value ? row["ApellidoMaterno"].ToString() : null,
                                    Estado = row["Estado"] != DBNull.Value ? row["Estado"].ToString() : null,
                                    Municipio = row["Municipio"] != DBNull.Value ? row["Municipio"].ToString() : null,
                                    Colonia = row["Colonia"] != DBNull.Value ? row["Colonia"].ToString() : null,
                                    Telefono = row["Telefono"] != DBNull.Value ? row["Telefono"].ToString() : null,
                                    Email = row["Email"] != DBNull.Value ? row["Email"].ToString() : null,
                                    FechaRegistro = row["FechaRegistro"] != DBNull.Value ? Convert.ToDateTime(row["FechaRegistro"]) : null,
                                    FotografiaBase64 = row["FotografiaBase64"] != DBNull.Value ? row["FotografiaBase64"].ToString() : null
                                };
                                result.Objects.Add(especialista);
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

        public static Models.Result GetById(int id)
        {
            Models.Result result = new Models.Result();
            try
            {
                using (SqlConnection connection = new SqlConnection(Data.Conexion.GetConnectionString()))
                {
                    string query = "SP_Especialistas_GetById";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@IdEspecialista", id);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable table = new DataTable();
                            adapter.Fill(table);

                            if (table.Rows.Count > 0)
                            {
                                DataRow row = table.Rows[0];
                                Models.Especialista especialista = new Models.Especialista
                                {
                                    IdEspecialista = Convert.ToInt32(row["IdEspecialista"]),
                                    Nombre = row["Nombre"].ToString(),
                                    ApellidoPaterno = row["ApellidoPaterno"].ToString(),
                                    ApellidoMaterno = row["ApellidoMaterno"] != DBNull.Value ? row["ApellidoMaterno"].ToString() : null,
                                    Estado = row["Estado"] != DBNull.Value ? row["Estado"].ToString() : null,
                                    Municipio = row["Municipio"] != DBNull.Value ? row["Municipio"].ToString() : null,
                                    Colonia = row["Colonia"] != DBNull.Value ? row["Colonia"].ToString() : null,
                                    Telefono = row["Telefono"] != DBNull.Value ? row["Telefono"].ToString() : null,
                                    Email = row["Email"] != DBNull.Value ? row["Email"].ToString() : null,
                                    FechaRegistro = row["FechaRegistro"] != DBNull.Value ? Convert.ToDateTime(row["FechaRegistro"]) : null,
                                    FotografiaBase64 = row["FotografiaBase64"] != DBNull.Value ? row["FotografiaBase64"].ToString() : null
                                };

                                result.Object = especialista;
                                result.Correct = true;
                            }
                            else
                            {
                                result.Correct = false;
                                result.ErrorMessage = "No se encontró el especialista.";
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

        public static Models.Result Add(Models.Especialista especialista)
        {
            Models.Result result = new Models.Result();
            try
            {
                using (SqlConnection connection = new SqlConnection(Data.Conexion.GetConnectionString()))
                {
                    string query = "SP_Especialistas_Insert";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 120; // 2 minutes timeout
                        
                        cmd.Parameters.AddWithValue("@Nombre", especialista.Nombre);
                        cmd.Parameters.AddWithValue("@ApellidoPaterno", especialista.ApellidoPaterno);
                        cmd.Parameters.AddWithValue("@ApellidoMaterno", (object)especialista.ApellidoMaterno ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Estado", (object)especialista.Estado ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Municipio", (object)especialista.Municipio ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Colonia", (object)especialista.Colonia ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Telefono", (object)especialista.Telefono ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Email", (object)especialista.Email ?? DBNull.Value);
                        
                        // Use SqlParameter for large NVARCHAR(MAX) field
                        var fotoParam = new SqlParameter("@FotografiaBase64", SqlDbType.NVarChar, -1);
                        fotoParam.Value = (object)especialista.FotografiaBase64 ?? DBNull.Value;
                        cmd.Parameters.Add(fotoParam);

                        connection.Open();
                        cmd.ExecuteNonQuery();
                        
                        // Consider success if no exception occurred
                        result.Correct = true;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }

        public static Models.Result Update(Models.Especialista especialista)
        {
            Models.Result result = new Models.Result();
            try
            {
                using (SqlConnection connection = new SqlConnection(Data.Conexion.GetConnectionString()))
                {
                    string query = "SP_Especialistas_Update";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 120; // 2 minutes timeout
                        
                        cmd.Parameters.AddWithValue("@IdEspecialista", especialista.IdEspecialista);
                        cmd.Parameters.AddWithValue("@Nombre", especialista.Nombre);
                        cmd.Parameters.AddWithValue("@ApellidoPaterno", especialista.ApellidoPaterno);
                        cmd.Parameters.AddWithValue("@ApellidoMaterno", (object)especialista.ApellidoMaterno ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Estado", (object)especialista.Estado ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Municipio", (object)especialista.Municipio ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Colonia", (object)especialista.Colonia ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Telefono", (object)especialista.Telefono ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Email", (object)especialista.Email ?? DBNull.Value);
                        
                        // Use SqlParameter for large NVARCHAR(MAX) field
                        var fotoParam = new SqlParameter("@FotografiaBase64", SqlDbType.NVarChar, -1);
                        fotoParam.Value = (object)especialista.FotografiaBase64 ?? DBNull.Value;
                        cmd.Parameters.Add(fotoParam);

                        connection.Open();
                        cmd.ExecuteNonQuery();
                        
                        // Consider success if no exception occurred
                        result.Correct = true;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }

        public static Models.Result Delete(int idEspecialista)
        {
            Models.Result result = new Models.Result();
            try
            {
                using (SqlConnection connection = new SqlConnection(Data.Conexion.GetConnectionString()))
                {
                    string query = "SP_Especialistas_Delete";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@IdEspecialista", idEspecialista);

                        connection.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        
                        // Consider success if no exception occurred
                        // Some stored procedures don't return affected rows count
                        result.Correct = true;
                        result.ErrorMessage = rowsAffected > 0 
                            ? $"Eliminado correctamente ({rowsAffected} registro(s))." 
                            : "Operación completada.";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }
    }
}
