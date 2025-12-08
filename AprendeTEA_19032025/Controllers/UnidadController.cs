using AprendeTEA_19032025.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

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

        public IActionResult Index(int IdPlanTrabajo, int? IdUsuario = null)
        {
            ViewBag.IdPlanTrabajo = IdPlanTrabajo;
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




        public IActionResult DetalleUnidad(int IdUnidad, int? IdUsuario = null)
        {
            var result = BL.Unidad.GetByIdUnidad(IdUnidad);

            if (result.Correct)
            {
                var unidad = (Models.Unidad)result.Object;
                ViewBag.IdUsuario = IdUsuario;

                if (IdUsuario != null)
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

                return View(unidad);
            }
            else
            {
                TempData["Mensaje"] = "No se pudo obtener el detalle de la unidad.";
                return RedirectToAction("Index", "PlanTrabajo");
            }
        }

        public IActionResult SopaLetras(int IdUnidad, int? IdUsuario = null)
        {
            var result = BL.Unidad.GetByIdUnidad(IdUnidad);
            if (result.Correct)
            {
                var unidad = (Models.Unidad)result.Object;
                unidad.PalabrasSopa = BL.Unidad.GetSopaLetras(IdUnidad); // Explicitly use new BL method
                ViewBag.IdUsuario = IdUsuario;
                return View(unidad);
            }
            return RedirectToAction("Index", "PlanTrabajo");
        }

        public IActionResult Relacionar(int IdUnidad, int? IdUsuario = null)
        {
            var result = BL.Unidad.GetByIdUnidad(IdUnidad);
            if (result.Correct)
            {
                var unidad = (Models.Unidad)result.Object;
                unidad.RelacionarColumnas = BL.Unidad.GetRelacionarColumnas(IdUnidad);
                ViewBag.IdUsuario = IdUsuario;
                return View(unidad);
            }
            return RedirectToAction("Index", "PlanTrabajo");
        }

        public IActionResult Agrupacion(int IdUnidad, int? IdUsuario = null)
        {
            var result = BL.Unidad.GetByIdUnidad(IdUnidad);
            if (result.Correct)
            {
                var unidad = (Models.Unidad)result.Object;
                unidad.Agrupacion = BL.Unidad.GetAgrupacion(IdUnidad);
                ViewBag.IdUsuario = IdUsuario;
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

    }
}
