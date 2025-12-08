using AprendeTEA_19032025.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Security.Claims;

namespace AprendeTEA_19032025.Controllers
{
    [Authorize]
    public class UnidadController : Controller
    {
        private readonly BL.Unidad _unidadBL;

        public UnidadController(ApplicationDbContext context)
        {
            _unidadBL = new BL.Unidad(context);
        }

        private int GetCurrentUserId()
        {
            var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(idClaim);
        }

        private string GetCurrentUserType()
        {
            var roleType = User.FindFirstValue(ClaimTypes.Role);
            return roleType;
        }

        public IActionResult Index(int IdPlanTrabajo, int? IdUsuario = null)
        {
            ViewBag.IdPlanTrabajo = IdPlanTrabajo;
            // Si viene IdUsuario (de Progreso), usarlo; si no, no lo pases
            ViewBag.IdUsuario = IdUsuario;
            Models.Result result = BL.Unidad.GetByPlanTrabajo(IdPlanTrabajo);

            Models.Unidad unidad = new Models.Unidad();

            if (result.Correct)
            {
                unidad.Unidades = result.Objects.ToList();
            }

            return View(unidad);
        }


        [HttpPost]
        public IActionResult CargarExcel(IFormFile file, int IdPlanTrabajo)
        {
            if (file != null && file.Length > 0)
            {
                var result = BL.Unidad.CargarDesdeExcel(file, IdPlanTrabajo);
                TempData["Mensaje"] = result.Correct ? "Archivo cargado correctamente." : $"Error: {result.ErrorMessage}";
            }
            else
            {
                TempData["Mensaje"] = "Selecciona un archivo válido.";
            }

            return RedirectToAction("Index", new { IdPlanTrabajo });
        }

        public IActionResult Form(int IdPlanTrabajo, int? IdUnidad)
        {
            Models.Unidad model = new Models.Unidad();
            model.IdPlanTrabajo = IdPlanTrabajo;

            // Obtener lista actual de unidades
            var resultUnidades = BL.Unidad.GetByPlanTrabajo(IdPlanTrabajo);
            if (resultUnidades.Correct)
            {
                model.Unidades = ((Models.Unidad)resultUnidades.Object).Unidades;
            }

            // Si se seleccionó una unidad, cargarla para edición
            if (IdUnidad != null)
            {
                var resultUnidad = BL.Unidad.GetByIdUnidad(IdUnidad.Value);
                if (resultUnidad.Correct)
                {
                    model.UnidadEdicion = (Models.Unidad)resultUnidad.Object;
                }
            }
            else
            {
                model.UnidadEdicion = new Models.Unidad { IdPlanTrabajo = IdPlanTrabajo };
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult GuardarUnidad(int IdUnidad, int IdPlanTrabajo, string Unidad, string Detalles, string Responsable)
        {
            Models.Unidad unidad = new Models.Unidad
            {
                IdUnidad = IdUnidad,
                IdPlanTrabajo = IdPlanTrabajo,
                NombreUnidad = Unidad,
                Detalle = Detalles,
                Responsable = Responsable
            };

            var result = BL.Unidad.GuardarUnidad(unidad);

            TempData["Mensaje"] = result.Correct
                ? (IdUnidad == 0 ? "Unidad agregada correctamente." : "Unidad actualizada correctamente.")
                : "Ocurrió un error al guardar la unidad.";

            return RedirectToAction("Form", new { IdPlanTrabajo });
        }




        public IActionResult DetalleUnidad(int IdUnidad)
        {
            var result = BL.Unidad.GetByIdUnidad(IdUnidad);

            if (result.Correct)
            {
                var unidad = (Models.Unidad)result.Object;
                
                // Verificar el tipo de usuario
                string userType = GetCurrentUserType();
                
                // Si es "Usuario" (estudiante), mostrar solo una actividad aleatoria
                if (userType == "Usuario")
                {
                    // Randomly select one available activity
                    List<string> available = new List<string>();
                    if (unidad.TieneSopaLetras) available.Add("Sopa");
                    if (unidad.TieneRelacionar) available.Add("Relacionar");
                    if (unidad.TieneAgrupacion) available.Add("Agrupacion");

                    if (available.Count > 0)
                    {
                        var random = new Random();
                        string selected = available[random.Next(available.Count)];

                        // Reset tags to hide others
                        unidad.TieneSopaLetras = selected == "Sopa";
                        unidad.TieneRelacionar = selected == "Relacionar";
                        unidad.TieneAgrupacion = selected == "Agrupacion";
                        
                        // Disable others not in the main 3 requested
                        unidad.TieneCrucigrama = false; 
                        unidad.TieneOrdenar = false;
                    }
                }
                // Si es "Admin", mostrar todas las actividades disponibles

                return View(unidad);
            }
            else
            {
                TempData["Mensaje"] = "No se pudo obtener el detalle de la unidad.";
                return RedirectToAction("Index", "PlanTrabajo");
            }
        }

        public IActionResult SopaLetras(int IdUnidad)
        {
            var result = BL.Unidad.GetByIdUnidad(IdUnidad);
            if (result.Correct)
            {
                var unidad = (Models.Unidad)result.Object;
                unidad.PalabrasSopa = BL.Unidad.GetSopaLetras(IdUnidad);
                ViewBag.IdUsuario = GetCurrentUserId();
                return View(unidad);
            }
            return RedirectToAction("Index", "PlanTrabajo");
        }

        public IActionResult Relacionar(int IdUnidad)
        {
            var result = BL.Unidad.GetByIdUnidad(IdUnidad);
            if (result.Correct)
            {
                var unidad = (Models.Unidad)result.Object;
                unidad.RelacionarColumnas = BL.Unidad.GetRelacionarColumnas(IdUnidad);
                ViewBag.IdUsuario = GetCurrentUserId();
                return View(unidad);
            }
            return RedirectToAction("Index", "PlanTrabajo");
        }

        public IActionResult Agrupacion(int IdUnidad)
        {
            var result = BL.Unidad.GetByIdUnidad(IdUnidad);
            if (result.Correct)
            {
                var unidad = (Models.Unidad)result.Object;
                unidad.Agrupacion = BL.Unidad.GetAgrupacion(IdUnidad);
                ViewBag.IdUsuario = GetCurrentUserId();
                return View(unidad);
            }
            return RedirectToAction("Index", "PlanTrabajo");
        }

        [HttpPost]
        public JsonResult GuardarCalificacion([FromBody] Models.CalificacionDetalle calificacion)
        {
            try
            {
                var result = BL.Calificaciones.Insert(calificacion);
                return Json(new { success = result.Correct, message = result.ErrorMessage });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Muestra las unidades de un plan con su estado de progreso/completado
        /// Usa SP_Unidades_ProgresoPorPlan
        /// </summary>
        public IActionResult ProgresoUnidades(int IdPlanTrabajo)
        {
            int userId = GetCurrentUserId();
            
            Models.Result result = BL.Unidad.GetProgresoPorPlan(IdPlanTrabajo, userId);

            Models.UnidadProgreso unidadProgreso = new Models.UnidadProgreso();

            if (result.Correct && result.Objects != null)
            {
                unidadProgreso.Unidades = result.Objects.Cast<Models.UnidadProgreso>().ToList();
                
                // Si hay al menos una unidad, tomar el IdPlanTrabajo de la primera
                if (unidadProgreso.Unidades.Any())
                {
                    unidadProgreso.IdPlanTrabajo = unidadProgreso.Unidades.First().IdPlanTrabajo;
                }
            }
            else
            {
                unidadProgreso.Unidades = new List<Models.UnidadProgreso>();
            }

            ViewBag.IdPlanTrabajo = IdPlanTrabajo;
            ViewBag.IdUsuario = userId;

            return View(unidadProgreso);
        }

    }
}
