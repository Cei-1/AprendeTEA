using AprendeTEA_19032025.Data;
using Microsoft.Data.SqlClient;
using OfficeOpenXml;
using System.Data;
using System.Numerics;

namespace AprendeTEA_19032025.BL
{
    public class Unidad
    {
        private readonly ApplicationDbContext _context;

        public Unidad(ApplicationDbContext context)
        {
            _context = context;
        }

        public static Models.Result GetByPlanTrabajo(int IdPlanTrabajo)
        {
            Models.Result result = new Models.Result();
            try
            {
                using (SqlConnection connection = new SqlConnection(Data.Conexion.GetConnectionString()))
                {
                    string query = "GetUnidadesByPlanTrabajo";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@IdPlanTrabajo", IdPlanTrabajo);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable table = new DataTable();
                            adapter.Fill(table);

                            result.Objects = new List<object>();
                            foreach (DataRow row in table.Rows)
                            {
                                Models.Unidad unidad = new Models.Unidad
                                {
                                    IdUnidad = Convert.ToInt32(row["IdUnidad"]),
                                    IdPlanTrabajo = Convert.ToInt32(row["IdPlanTrabajo"]),
                                    NombreUnidad = row["Unidad"].ToString(),
                                    Detalle = row["Detalles"].ToString(),
                                    FechaRegistro = Convert.ToDateTime(row["FechaRegistro"]),
                                    Estatus = Convert.ToBoolean(row["Estatus"])
                                };
                                result.Objects.Add(unidad);
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

        public static Models.Result CargarDesdeExcel(IFormFile file, int IdPlanTrabajo)
        {
            Models.Result result = new Models.Result();
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var stream = new MemoryStream())
                {
                    file.CopyTo(stream);
                    using (var package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                        int rowCount = worksheet.Dimension.Rows;

                        using (SqlConnection connection = new SqlConnection(Data.Conexion.GetConnectionString()))
                        {
                            connection.Open();
                            Models.Unidad unidad= new Models.Unidad();
                            for (int row = 2; row <= rowCount; row++)
                            {
                                unidad.NombreUnidad = worksheet.Cells[row, 1]?.Text;
                                unidad.Objetivo = worksheet.Cells[row, 2]?.Text;
                                unidad.Detalle = worksheet.Cells[row, 3]?.Text;

                                unidad.PalabrasSopa = worksheet.Cells[row, 4]?.Text;
                                unidad.PreguntasCrucigrama = worksheet.Cells[row, 5]?.Text;
                                unidad.RelacionarColumnas = worksheet.Cells[row, 6]?.Text;
                                unidad.Agrupacion = worksheet.Cells[row, 7]?.Text;
                                unidad.OrdenarPasos = worksheet.Cells[row, 8]?.Text;

                                unidad.TieneSopaLetras = !string.IsNullOrWhiteSpace(unidad.PalabrasSopa);
                                unidad.TieneCrucigrama = !string.IsNullOrWhiteSpace(unidad.PreguntasCrucigrama);
                                unidad.TieneRelacionar = !string.IsNullOrWhiteSpace(unidad.RelacionarColumnas);
                                unidad.TieneAgrupacion = !string.IsNullOrWhiteSpace(unidad.Agrupacion);
                                unidad.TieneOrdenar = !string.IsNullOrWhiteSpace(unidad.OrdenarPasos);                        

                                using (SqlCommand cmd = new SqlCommand("SP_CRUD_Unidad_PT", connection))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@IdPlanTrabajo", IdPlanTrabajo);
                                    cmd.Parameters.AddWithValue("@Unidad", unidad.NombreUnidad ?? "");
                                    cmd.Parameters.AddWithValue("@Objetivo", unidad.Objetivo ?? "");
                                    cmd.Parameters.AddWithValue("@Detalle", unidad.Detalle ?? "");

                                    cmd.Parameters.AddWithValue("@TieneSopaLetras", unidad.TieneSopaLetras);
                                    cmd.Parameters.AddWithValue("@PalabrasSopa", unidad.PalabrasSopa ?? (object)DBNull.Value);

                                    cmd.Parameters.AddWithValue("@TieneCrucigrama", unidad.TieneCrucigrama);
                                    cmd.Parameters.AddWithValue("@PreguntasCrucigrama", unidad.PreguntasCrucigrama ?? (object)DBNull.Value);

                                    cmd.Parameters.AddWithValue("@TieneRelacionar", unidad.TieneRelacionar);
                                    cmd.Parameters.AddWithValue("@RelacionarColumnas", unidad.RelacionarColumnas ?? (object)DBNull.Value);

                                    cmd.Parameters.AddWithValue("@TieneAgrupacion", unidad.TieneAgrupacion);
                                    cmd.Parameters.AddWithValue("@Agrupacion", unidad.Agrupacion ?? (object)DBNull.Value);

                                    cmd.Parameters.AddWithValue("@TieneOrdenar", unidad.TieneOrdenar);
                                    cmd.Parameters.AddWithValue("@OrdenarPasos", unidad.OrdenarPasos ?? (object)DBNull.Value);

                                    cmd.Parameters.AddWithValue("@Opcion", 1);


                                    cmd.ExecuteNonQuery();
                                }
                            }
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

        public static Models.Result GetByIdUnidad(int IdUnidad)
        {
            Models.Result result = new Models.Result();
            try
            {
                using (SqlConnection connection = new SqlConnection(Data.Conexion.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("SP_CRUD_Unidad_PT", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdUnidad", IdUnidad);
                    cmd.Parameters.AddWithValue("@Opcion", 2);


                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    if (table.Rows.Count > 0)
                    {
                        DataRow row = table.Rows[0];

                        Models.Unidad unidad = new Models.Unidad
                        {
                            IdUnidad = Convert.ToInt32(row["IdUnidad"]),
                            IdPlanTrabajo = Convert.ToInt32(row["IdPlanTrabajo"]),
                            NombreUnidad = row["Unidad"].ToString(),
                            Objetivo = row["Objetivo"].ToString(),
                            Detalle = row["Detalles"].ToString(),
                            FechaRegistro = row["FechaRegistro"] != DBNull.Value ? Convert.ToDateTime(row["FechaRegistro"]) : null,
                            Estatus = row["Estatus"] != DBNull.Value && Convert.ToBoolean(row["Estatus"]),

                            // Actividades
                            TieneSopaLetras = row["TieneSopaLetras"] != DBNull.Value && Convert.ToBoolean(row["TieneSopaLetras"]),
                            PalabrasSopa = row["PalabrasSopa"]?.ToString(),

                            TieneCrucigrama = row["TieneCrucigrama"] != DBNull.Value && Convert.ToBoolean(row["TieneCrucigrama"]),
                            PreguntasCrucigrama = row["PreguntasCrucigrama"]?.ToString(),

                            TieneRelacionar = row["TieneRelacionar"] != DBNull.Value && Convert.ToBoolean(row["TieneRelacionar"]),
                            RelacionarColumnas = row["RelacionarColumnas"]?.ToString(),

                            TieneAgrupacion = row["TieneAgrupacion"] != DBNull.Value && Convert.ToBoolean(row["TieneAgrupacion"]),
                            Agrupacion = row["Agrupacion"]?.ToString(),

                            TieneOrdenar = row["TieneOrdenar"] != DBNull.Value && Convert.ToBoolean(row["TieneOrdenar"]),
                            OrdenarPasos = row["OrdenarPasos"]?.ToString()
                        };

                        result.Object = unidad;
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


        public static Models.Result GuardarUnidad(Models.Unidad unidad)
        {
            Models.Result result = new Models.Result();
            try
            {
                using (SqlConnection connection = new SqlConnection(Data.Conexion.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("ControlR_AddOrUpdateUnidad", connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdUnidad", unidad.IdUnidad);
                    cmd.Parameters.AddWithValue("@IdPlanTrabajo", unidad.IdPlanTrabajo);
                    cmd.Parameters.AddWithValue("@Unidad", unidad.NombreUnidad ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Detalle", unidad.Detalle ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Responsable", unidad.Responsable ?? (object)DBNull.Value);

                    connection.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    result.Correct = rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }
        public Models.Result DeleteEF(int idUnidad)
        {
            Models.Result result = new Models.Result();

            try
            {
                var entidad = _context.Unidad.FirstOrDefault(u => u.IdUnidad == idUnidad);

                if (entidad == null)
                {
                    result.Correct = false;
                    result.ErrorMessage = "Unidad no encontrada.";
                    return result;
                }

                _context.Unidad.Remove(entidad);
                _context.SaveChanges();

                result.Correct = true;
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        public static string GetSopaLetras(int IdUnidad)
        {
            string palabras = "";
            try
            {
                using (SqlConnection connection = new SqlConnection(Data.Conexion.GetConnectionString()))
                {
                    string query = "SP_UnidadPT_GetSopaLetras";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@IdUnidad", IdUnidad);
                        connection.Open();
                        var result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            palabras = result.ToString();
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Manejar excepción o loguear
            }
            return palabras;
        }

        public static string GetRelacionarColumnas(int IdUnidad)
        {
            string relacionar = "";
            try
            {
                using (SqlConnection connection = new SqlConnection(Data.Conexion.GetConnectionString()))
                {
                    string query = "SP_UnidadPT_GetRelacionarColumnas";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@IdUnidad", IdUnidad);
                        connection.Open();
                        var result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            relacionar = result.ToString();
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Manejar excepción
            }
            return relacionar;
        }

        public static string GetAgrupacion(int IdUnidad)
        {
            string agrupacion = "";
            try
            {
                using (SqlConnection connection = new SqlConnection(Data.Conexion.GetConnectionString()))
                {
                    string query = "SP_UnidadPT_GetAgrupacion";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@IdUnidad", IdUnidad);
                        connection.Open();
                        var result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            agrupacion = result.ToString();
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Manejar excepción
            }
            return agrupacion;
        }

        /// <summary>
        /// Obtiene las unidades de un plan con su estado de progreso/completado
        /// Usa SP_Unidades_ProgresoPorPlan
        /// </summary>
        public static Models.Result GetProgresoPorPlan(int idPlanTrabajo, int idUsuario)
        {
            Models.Result result = new Models.Result();
            try
            {
                using (SqlConnection connection = new SqlConnection(Data.Conexion.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_Unidades_ProgresoPorPlan", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@IdPlanTrabajo", idPlanTrabajo);
                        cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable table = new DataTable();
                            adapter.Fill(table);

                            result.Objects = new List<object>();
                            foreach (DataRow row in table.Rows)
                            {
                                Models.UnidadProgreso unidad = new Models.UnidadProgreso
                                {
                                    IdUnidad = Convert.ToInt32(row["IdUnidad"]),
                                    IdPlanTrabajo = Convert.ToInt32(row["IdPlanTrabajo"]),
                                    Objetivo = row["Objetivo"]?.ToString() ?? "",
                                    Unidad = row["Unidad"]?.ToString() ?? "",
                                    Detalles = row["Detalles"]?.ToString() ?? "",
                                    FechaRegistro = row["FechaRegistro"] != DBNull.Value ? Convert.ToDateTime(row["FechaRegistro"]) : (DateTime?)null,
                                    Estatus = Convert.ToBoolean(row["Estatus"]),
                                    Completada = Convert.ToBoolean(row["Completada"]),
                                    CalificacionObtenida = row["CalificacionObtenida"] != DBNull.Value ? Convert.ToDecimal(row["CalificacionObtenida"]) : (decimal?)null,
                                    TiempoDedicado = row["TiempoDedicado"] != DBNull.Value ? Convert.ToDecimal(row["TiempoDedicado"]) : (decimal?)null
                                };
                                result.Objects.Add(unidad);
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
    }
}
