using AprendeTEA_19032025.Data;
using AprendeTEA_19032025.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace AprendeTEA_19032025.BL
{
    public class PlanTrabajo
    {
        private readonly ApplicationDbContext _context;

        public PlanTrabajo(ApplicationDbContext context)
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
                    string query = "GetAllPlanTrabajo";
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
                                Models.PlanTrabajo plan = new Models.PlanTrabajo
                                {
                                    IdPlanTrabajo = Convert.ToInt32(row["IdPlanTrabajo"]),
                                    NombrePlan = row["NombrePlan"].ToString(),
                                    FechaRegistro = Convert.ToDateTime(row["FechaRegistro"]),
                                    Estatus = Convert.ToBoolean(row["Estatus"])
                                };
                                result.Objects.Add(plan);
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
                    string query = "GetByIdPlanTrabajo";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@IdPlanTrabajo", id);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable table = new DataTable();
                            adapter.Fill(table);

                            if (table.Rows.Count > 0)
                            {
                                DataRow row = table.Rows[0];
                                Models.PlanTrabajo plan = new Models.PlanTrabajo
                                {
                                    IdPlanTrabajo = Convert.ToInt32(row["IdPlanTrabajo"]),
                                    NombrePlan = row["NombrePlan"].ToString(),
                                    FechaRegistro = Convert.ToDateTime(row["FechaRegistro"]),
                                    Estatus = Convert.ToBoolean(row["Estatus"])
                                };

                                result.Object = plan;
                                result.Correct = true;
                            }
                            else
                            {
                                result.Correct = false;
                                result.ErrorMessage = "No se encontró el plan de trabajo.";
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

        public static Models.Result Add(Models.PlanTrabajo plan)
        {
            Models.Result result = new Models.Result();
            try
            {
                using (SqlConnection connection = new SqlConnection(Data.Conexion.GetConnectionString()))
                {
                    string query = "AddPlanTrabajo";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@NombrePlan", plan.NombrePlan);

                        connection.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        result.Correct = rowsAffected > 0;
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

        public static Models.Result Update(Models.PlanTrabajo plan)
        {
            Models.Result result = new Models.Result();
            try
            {
                using (SqlConnection connection = new SqlConnection(Data.Conexion.GetConnectionString()))
                {
                    string query = "UpdatePlanTrabajo";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@IdPlanTrabajo", plan.IdPlanTrabajo);
                        cmd.Parameters.AddWithValue("@NombrePlan", plan.NombrePlan);

                        connection.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        result.Correct = rowsAffected > 0;
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

        public static Models.Result Delete(int idPlanTrabajo)
        {
            Models.Result result = new Models.Result();
            try
            {
                using (SqlConnection connection = new SqlConnection(Data.Conexion.GetConnectionString()))
                {
                    // Asume que tienes un procedimiento almacenado llamado "DeletePlanTrabajo"
                    // que toma @IdPlanTrabajo como parámetro.
                    string query = "DeletePlanTrabajo";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@IdPlanTrabajo", idPlanTrabajo);

                        connection.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        result.Correct = rowsAffected > 0;
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