using AprendeTEA_19032025.Helpers;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AprendeTEA_19032025.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminUsuariosController : Controller
    {
        public IActionResult PendientesActivar()
        {
            var result = BL.Usuario.GetPendientesActivar();

            if (!result.Correct)
            {
                ViewBag.Error = result.ErrorMessage;
                return View(new List<Models.Usuario>());
            }

            var lista = result.Objects.Cast<Models.Usuario>().ToList();
            return View(lista);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ActivarManual(int idUsuario)
        {
            var result = BL.Usuario.ConfirmarEmailManual(idUsuario);

            if (!result.Correct)
                TempData["Error"] = result.ErrorMessage;
            else
                TempData["Mensaje"] = "Usuario activado manualmente.";

            return RedirectToAction("PendientesActivar");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ReenviarConfirmacion(int idUsuario)
        {
            // ✅ Aquí sí necesitamos el usuario (para obtener el email)
            var result = BL.Usuario.GetById(idUsuario);

            if (!result.Correct || result.Object is not Models.Usuario usuario)
            {
                TempData["Error"] = "No se encontró el usuario.";
                return RedirectToAction("PendientesActivar");
            }

            // ✅ Regenerar token
            string token = Guid.NewGuid().ToString("N");
            var tokenResult = BL.Usuario.ActualizarTokenConfirmacion(idUsuario, token);

            if (!tokenResult.Correct)
            {
                TempData["Error"] = tokenResult.ErrorMessage ?? "No se pudo regenerar el token.";
                return RedirectToAction("PendientesActivar");
            }

            // ✅ Enviar correo con Hangfire usando EmailJobs (DI)
            BackgroundJob.Enqueue<EmailJobs>(job =>
                job.EnviarConfirmacion(usuario.Email, idUsuario, token)
            );

            TempData["Mensaje"] = "Correo de confirmación reenviado.";
            return RedirectToAction("PendientesActivar");
        }
    }
}
