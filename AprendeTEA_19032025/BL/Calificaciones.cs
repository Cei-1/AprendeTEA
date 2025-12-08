using AprendeTEA_19032025.Data;
using AprendeTEA_19032025.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace AprendeTEA_19032025.BL
{
    public class Calificaciones
    {
        private readonly ApplicationDbContext _context;

        public Calificaciones(ApplicationDbContext context)
        {
            _context = context;
        }

        public static Models.Result Insert(Models.CalificacionDetalle calificacion)
        {
            Models.Result result = new Models.Result();
            try
            {
                using (SqlConnection connection = new SqlConnection(Data.Conexion.GetConnectionString()))
                {
                    string query = "SP_Calificaciones_Insert_Update";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@IdUsuario", calificacion.IdUsuario);
                        cmd.Parameters.AddWithValue("@IdUnidad", calificacion.IdUnidad);
                        cmd.Parameters.AddWithValue("@TiempoDedicado", (object)calificacion.TiempoDedicado ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Calificacion", (object)calificacion.Calificacion ?? DBNull.Value);

                        connection.Open();
                        var idCalificacion = cmd.ExecuteScalar();

                        result.Object = idCalificacion;
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

        public static Models.Result GetDetalleByUsuarioId(int idUsuario)
        {
            Models.Result result = new Models.Result();
            try
            {
                using (SqlConnection connection = new SqlConnection(Data.Conexion.GetConnectionString()))
                {
                    string query = "SP_Calificaciones_GetDetalleByUsuarioId";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable table = new DataTable();
                            adapter.Fill(table);

                            result.Objects = new List<object>();

                            foreach (DataRow row in table.Rows)
                            {
                                Models.CalificacionDetalle calificacion = new Models.CalificacionDetalle
                                {
                                    IdCalificacion = Convert.ToInt32(row["IdCalificacion"]),
                                    IdUsuario = Convert.ToInt32(row["IdUsuario"]),
                                    NombreCompleto = row["NombreCompleto"] != DBNull.Value ? row["NombreCompleto"].ToString() : null,
                                    IdUnidad = Convert.ToInt32(row["IdUnidad"]),
                                    Unidad = row["Unidad"] != DBNull.Value ? row["Unidad"].ToString() : null,
                                    TiempoDedicado = row["TiempoDedicado"] != DBNull.Value ? Convert.ToDecimal(row["TiempoDedicado"]) : null,
                                    Calificacion = row["Calificacion"] != DBNull.Value ? Convert.ToDecimal(row["Calificacion"]) : null,
                                    FechaRegistro = row["FechaRegistro"] != DBNull.Value ? Convert.ToDateTime(row["FechaRegistro"]) : null
                                };
                                result.Objects.Add(calificacion);
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
                result.Ex = ex;
            }
            return result;
        }
    }
}
