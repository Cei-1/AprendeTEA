using System.Data;
using Microsoft.Data.SqlClient;

namespace AprendeTEA_19032025.BL
{
    /// <summary>
    /// Business Logic para el progreso de planes de trabajo
    /// </summary>
    public class ProgresoPlanesTrabajo
    {
        /// <summary>
        /// Obtiene el progreso de todos los planes de trabajo para un usuario específico
        /// </summary>
        /// <param name="IdUsuario">ID del usuario</param>
        /// <returns>Result con lista de ProgresoPlanesTrabajo</returns>
        public static Models.Result GetProgresoByUsuarioId(int IdUsuario)
        {
            Models.Result result = new Models.Result();
            try
            {
                using (SqlConnection connection = new SqlConnection(Data.Conexion.GetConnectionString()))
                {
                    string query = "SP_Progreso_PlanesDeTrabajo";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@IdUsuario", IdUsuario);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable table = new DataTable();
                            adapter.Fill(table);
                            result.Objects = new List<object>();

                            foreach (DataRow row in table.Rows)
                            {
                                Models.ProgresoPlanesTrabajo progreso = new Models.ProgresoPlanesTrabajo
                                {
                                    IdPlanTrabajo = Convert.ToInt32(row["IdPlanTrabajo"]),
                                    NombrePlan = row["NombrePlan"] != DBNull.Value ? row["NombrePlan"].ToString() : null,
                                    FechaRegistro = row["FechaRegistro"] != DBNull.Value ? Convert.ToDateTime(row["FechaRegistro"]) : null,
                                    Estatus = row["Estatus"] != DBNull.Value && Convert.ToBoolean(row["Estatus"]),
                                    TotalUnidades = row["TotalUnidades"] != DBNull.Value ? Convert.ToInt32(row["TotalUnidades"]) : 0,
                                    UnidadesCompletadas = row["UnidadesCompletadas"] != DBNull.Value ? Convert.ToInt32(row["UnidadesCompletadas"]) : 0,
                                    PorcentajeProgreso = row["PorcentajeProgreso"] != DBNull.Value ? Convert.ToDecimal(row["PorcentajeProgreso"]) : 0
                                };
                                result.Objects.Add(progreso);
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
