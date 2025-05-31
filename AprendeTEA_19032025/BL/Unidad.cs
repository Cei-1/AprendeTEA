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

        public static Models.Result CargarDesdeExcel1(IFormFile file, int IdPlanTrabajo)
        {
            Models.Result result = new Models.Result();
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var stream = new MemoryStream())
                {
                    file.CopyTo(stream);
                    using (var package = new OfficeOpenXml.ExcelPackage(stream))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Asume que es la primera hoja
                        int rowCount = worksheet.Dimension.Rows;

                        using (SqlConnection connection = new SqlConnection(Data.Conexion.GetConnectionString()))
                        {
                            connection.Open();
                            for (int row = 2; row <= rowCount; row++) // Empieza desde la fila 2 (asumiendo encabezados)
                            {
                                string unidad = worksheet.Cells[row, 1]?.Text;
                                string detalle = worksheet.Cells[row, 2]?.Text;
                                //string responsable = worksheet.Cells[row, 3]?.Text;

                                using (SqlCommand cmd = new SqlCommand("AddUnidad", connection))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@IdPlanTrabajo", IdPlanTrabajo);
                                    cmd.Parameters.AddWithValue("@Unidad", unidad ?? "");
                                    cmd.Parameters.AddWithValue("@Detalle", detalle ?? "");
                                    //cmd.Parameters.AddWithValue("@Responsable", responsable ?? "");
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
                            for (int row = 2; row <= rowCount; row++)
                            {
                                string unidad = worksheet.Cells[row, 1]?.Text;
                                string detalle = worksheet.Cells[row, 2]?.Text;

                                string sopa = worksheet.Cells[row, 3]?.Text;
                                string crucigrama = worksheet.Cells[row, 4]?.Text;
                                string relacionar = worksheet.Cells[row, 5]?.Text;
                                string memorama = worksheet.Cells[row, 6]?.Text; 
                                string juegoRol = worksheet.Cells[row, 7]?.Text;

                                bool tieneSopa = !string.IsNullOrWhiteSpace(sopa);
                                bool tieneCrucigrama = !string.IsNullOrWhiteSpace(crucigrama);
                                bool tieneRelacionar = !string.IsNullOrWhiteSpace(relacionar);
                                bool tieneMemorama = !string.IsNullOrWhiteSpace(memorama);
                                bool tieneJuegoRol = !string.IsNullOrWhiteSpace(juegoRol);

                                using (SqlCommand cmd = new SqlCommand("AddUnidad", connection))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@IdPlanTrabajo", IdPlanTrabajo);
                                    cmd.Parameters.AddWithValue("@Unidad", unidad ?? "");
                                    cmd.Parameters.AddWithValue("@Detalle", detalle ?? "");

                                    cmd.Parameters.AddWithValue("@TieneSopaLetras", tieneSopa);
                                    cmd.Parameters.AddWithValue("@PalabrasSopa", sopa ?? (object)DBNull.Value);

                                    cmd.Parameters.AddWithValue("@TieneCrucigrama", tieneCrucigrama);
                                    cmd.Parameters.AddWithValue("@PreguntasCrucigrama", crucigrama ?? (object)DBNull.Value);

                                    cmd.Parameters.AddWithValue("@TieneRelacionar", tieneRelacionar);
                                    cmd.Parameters.AddWithValue("@RelacionarColumnas", relacionar ?? (object)DBNull.Value);
                                    cmd.Parameters.AddWithValue("@TieneMemorama", tieneMemorama);
                                    cmd.Parameters.AddWithValue("@TieneJuegoRol", tieneJuegoRol);
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


        //public static Models.Result GetByPlanTrabajo(int IdPlanTrabajo)
        //{
        //    Models.Result result = new Models.Result();
        //    try
        //    {
        //        using (SqlConnection connection = new SqlConnection(Data.Conexion.GetConnectionString()))
        //        {
        //            SqlCommand cmd = new SqlCommand("GetUnidadesByPlanTrabajo", connection);
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.AddWithValue("@IdPlanTrabajo", IdPlanTrabajo);

        //            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        //            DataTable table = new DataTable();
        //            adapter.Fill(table);

        //            Models.Unidad model = new Models.Unidad
        //            {
        //                IdPlanTrabajo = IdPlanTrabajo,
        //                Unidades = new List<Models.UnidadPlanTrabajo>()
        //            };

        //            foreach (DataRow row in table.Rows)
        //            {
        //                model.Unidades.Add(new Models.UnidadPlanTrabajo
        //                {
        //                    IdUnidad = Convert.ToInt32(row["IdUnidad"]),
        //                    IdPlanTrabajo = IdPlanTrabajo,
        //                    Unidad = row["Unidad"].ToString(),
        //                    Detalles = row["Detalles"].ToString(),
        //                    Responsable = row["Responsable"].ToString(),
        //                    FechaRegistro = Convert.ToDateTime(row["FechaRegistro"]),
        //                    Estatus = Convert.ToBoolean(row["Estatus"])
        //                });
        //            }

        //            result.Object = model;
        //            result.Correct = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result.Correct = false;
        //        result.ErrorMessage = ex.Message;
        //    }

        //    return result;
        //}

        public static Models.Result GetByIdUnidad1(int IdUnidad)
        {
            Models.Result result = new Models.Result();
            try
            {
                using (SqlConnection connection = new SqlConnection(Data.Conexion.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("GetByIdUnidad", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdUnidad", IdUnidad);

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
                            Detalle = row["Detalles"].ToString(),
                            //Responsable = row["Responsable"].ToString(),
                            FechaRegistro = Convert.ToDateTime(row["FechaRegistro"]),
                            Estatus = Convert.ToBoolean(row["Estatus"])
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
        public static Models.Result GetByIdUnidad(int IdUnidad)
        {
            Models.Result result = new Models.Result();
            try
            {
                using (SqlConnection connection = new SqlConnection(Data.Conexion.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("GetByIdUnidad", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdUnidad", IdUnidad);

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

                            TieneMemorama = row.Table.Columns.Contains("TieneMemorama") && row["TieneMemorama"] != DBNull.Value && Convert.ToBoolean(row["TieneMemorama"]),
                            TieneJuegoRol = row.Table.Columns.Contains("TieneJuegoRol") && row["TieneJuegoRol"] != DBNull.Value && Convert.ToBoolean(row["TieneJuegoRol"])
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

    }
}
