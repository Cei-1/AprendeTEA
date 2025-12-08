using AprendeTEA_19032025.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AprendeTEA_19032025.Controllers
{
    

    public class PlanTrabajoController : Controller
    {
        private readonly BL.PlanTrabajo _planTrabajoBL;

        public PlanTrabajoController(BL.PlanTrabajo planTrabajoBL)
        {
            _planTrabajoBL = planTrabajoBL;
        }
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Usuario")]
        private int GetCurrentUserId()
        {
            var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(idClaim);
        }

        /// <summary>
        /// Vista de progreso del usuario en los planes de trabajo
        /// Usa SP_Progreso_PlanesDeTrabajo
        /// </summary>
        /// 
        [Authorize(Roles = "Usuario")]
        public IActionResult Progreso()
        {
            int userId = GetCurrentUserId();

            Models.ProgresoPlanesTrabajo progreso = new Models.ProgresoPlanesTrabajo();
            Models.Result result = BL.ProgresoPlanesTrabajo.GetProgresoByUsuarioId(userId);

            if (result.Correct)
            {
                progreso.PlanesTrabajo = result.Objects?.Cast<Models.ProgresoPlanesTrabajo>().ToList();
            }

            return View(progreso);
        }
    }
}