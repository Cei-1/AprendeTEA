using AprendeTEA_19032025.Data;
using Microsoft.AspNetCore.Mvc;

namespace AprendeTEA_19032025.Controllers
{
    public class CalificacionesController : Controller
    {
        private readonly BL.Calificaciones _calificacionesBL;

        public CalificacionesController(ApplicationDbContext context)
        {
            _calificacionesBL = new BL.Calificaciones(context);
        }

        // Dashboard view showing grades detail for a user
        public IActionResult Index(int? IdUsuario)
        {
            // For now, use URL parameter. In production, get from session/auth
            if (!IdUsuario.HasValue)
            {
                // Default to user 1 for testing, or redirect to login
                IdUsuario = 1;
            }

            Models.CalificacionDetalle calificacionDetalle = new Models.CalificacionDetalle();
            Models.Result result = BL.Calificaciones.GetDetalleByUsuarioId(IdUsuario.Value);

            if (result.Correct)
            {
                calificacionDetalle.Calificaciones = result.Objects
    .Cast<Models.CalificacionDetalle>()
    .ToList();

                
                // Get user info from first record if available
                if (result.Objects.Count > 0)
                {
                    var firstRecord = (Models.CalificacionDetalle)result.Objects[0];
                    calificacionDetalle.NombreCompleto = firstRecord.NombreCompleto;
                    calificacionDetalle.IdUsuario = firstRecord.IdUsuario;
                }
            }
            else
            {
                TempData["Mensaje"] = $"Error al cargar calificaciones: {result.ErrorMessage}";
            }

            return View(calificacionDetalle);
        }

        // Insert new grade (optional, for future use)
        [HttpPost]
        public IActionResult Insert(Models.CalificacionDetalle calificacion)
        {
            Models.Result result = BL.Calificaciones.Insert(calificacion);

            if (result.Correct)
            {
                TempData["Mensaje"] = "Calificación registrada correctamente.";
                return RedirectToAction("Index", new { IdUsuario = calificacion.IdUsuario });
            }
            else
            {
                TempData["Mensaje"] = $"Error al registrar calificación: {result.ErrorMessage}";
                return RedirectToAction("Index", new { IdUsuario = calificacion.IdUsuario });
            }
        }
    }
}
