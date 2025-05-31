using AprendeTEA_19032025.Data;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace AprendeTEA_19032025.Controllers
{
    public class UnidadController : Controller
    {
        private readonly BL.Unidad _unidadBL;

        public UnidadController(ApplicationDbContext context)
        {
            _unidadBL = new BL.Unidad(context);
        }

        public IActionResult Index(int IdPlanTrabajo)
        {
            ViewBag.IdPlanTrabajo = IdPlanTrabajo;
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
                return View(unidad);
            }
            else
            {
                TempData["Mensaje"] = "No se pudo obtener el detalle de la unidad.";
                return RedirectToAction("Index", "PlanTrabajo");
            }
        }

    }
}
