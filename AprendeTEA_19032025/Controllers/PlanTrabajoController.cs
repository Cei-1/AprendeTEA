using AprendeTEA_19032025.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AprendeTEA_19032025.Controllers
{
    [Authorize(Roles = "Admin")]

    public class PlanTrabajoController : Controller
    {
        private readonly BL.PlanTrabajo _planTrabajoBL;

        public PlanTrabajoController(BL.PlanTrabajo planTrabajoBL)
        {
            _planTrabajoBL = planTrabajoBL;
        }

        public IActionResult Index()
        {
            Models.PlanTrabajo planTrabajo = new Models.PlanTrabajo();
            Models.Result result = BL.PlanTrabajo.GetAll();

            if (result.Correct)
            {
                planTrabajo.PlanesTrabajo = result.Objects.ToList();
            }

            return View(planTrabajo);
        }

        [HttpGet]
        public IActionResult Form(int? IdPlanTrabajo)
        {
            Models.PlanTrabajo planTrabajo = new Models.PlanTrabajo();

            if (IdPlanTrabajo == null)
            {
                return View(planTrabajo);
            }
            else
            {
                var result = BL.PlanTrabajo.GetById(IdPlanTrabajo.Value);

                if (result.Correct)
                {
                    planTrabajo = (Models.PlanTrabajo)result.Object;
                }

                return View(planTrabajo);
            }
        }

        [HttpPost]
        public IActionResult Form(Models.PlanTrabajo planTrabajo)
        {
            if (planTrabajo.IdPlanTrabajo == 0 || planTrabajo.IdPlanTrabajo == null)
            {
                Models.Result result = BL.PlanTrabajo.Add(planTrabajo);
                TempData["Mensaje"] = result.Correct ? "Plan registrado correctamente." : "Error al registrar el plan.";
            }
            else
            {
                var result = BL.PlanTrabajo.Update(planTrabajo);
                TempData["Mensaje"] = result.Correct ? "Plan actualizado correctamente." : "Error al actualizar el plan.";
            }

            return RedirectToAction("Index");
        }

        public IActionResult Detalle(int IdPlanTrabajo)
        {
            ViewBag.IdPlanTrabajo = IdPlanTrabajo;
            Models.Result result = BL.Unidad.GetByPlanTrabajo(IdPlanTrabajo);

            List<Models.Unidad> unidades = new List<Models.Unidad>();

            if (result.Correct)
            {
                unidades = result.Objects.Cast<Models.Unidad>().ToList();
            }

            return View(unidades);
        }

        // --- Nuevo método para eliminar ---
        [HttpGet] // Se usa HttpGet para simplificar por ahora, pero lo ideal es un HttpPost con un formulario.
        public IActionResult Delete(int IdPlanTrabajo)
        {
            Models.Result result = BL.PlanTrabajo.Delete(IdPlanTrabajo);

            if (result.Correct)
            {
                TempData["Mensaje"] = "Plan eliminado correctamente.";
            }
            else
            {
                TempData["Mensaje"] = "Error al eliminar el plan.";
            }

            return RedirectToAction("Index");
        }
    }
}