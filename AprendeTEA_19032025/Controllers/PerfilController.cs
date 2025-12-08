using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AprendeTEA_19032025.Controllers
{
    [Authorize] // cualquier usuario logueado
    public class PerfilController : Controller
    {
        private int GetCurrentUserId()
        {
            var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(idClaim);
        }

        [HttpGet]
        public IActionResult Index()
        {
            int idUsuario = GetCurrentUserId();

            var result = BL.Perfil.GetPerfilByIdUsuario(idUsuario);

            if (!result.Correct || result.Object == null)
            {
                // podrías mandar a error o mostrar mensaje
                ViewBag.Error = result.ErrorMessage ?? "No se pudo cargar el perfil.";
                return View(new Models.Perfil
                {
                    IdUsuario = idUsuario,
                    Email = User.Identity.Name
                });
            }

            var model = (Models.Perfil)result.Object;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //filtro de seguridad que evita ataques Cross-Site Request Forgery (CSRF).
        public IActionResult Actualizar(Models.Perfil model)
        {
            // Seguridad: forzar que el IdUsuario sea el del usuario logueado
            model.IdUsuario = GetCurrentUserId();

            var result = BL.Perfil.UpdatePerfil(model);

            if (!result.Correct)
            {
                ModelState.AddModelError("", result.ErrorMessage ?? "No se pudo actualizar el perfil.");
                return View("Index", model);
            }

            TempData["PerfilMensaje"] = "Perfil actualizado correctamente.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult FotoPerfil()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(idClaim) || !int.TryParse(idClaim, out int idUsuario))
            {
                return NotFound();
            }

            var result = BL.Perfil.GetPerfilByIdUsuario(idUsuario);
            if (!result.Correct || result.Object is not Models.Perfil perfil)
            {
                return DefaultAvatar();
            }

            if (string.IsNullOrEmpty(perfil.FotoBase64))
            {
                return DefaultAvatar();
            }

            try
            {
                byte[] bytes = Convert.FromBase64String(perfil.FotoBase64);
                return File(bytes, "image/png");
            }
            catch
            {
                return DefaultAvatar();
            }
        }

        private IActionResult DefaultAvatar()
        {
            // Puedes usar una imagen en wwwroot/img o generar algo fijo
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "default-avatar.png");

            if (!System.IO.File.Exists(path))
            {
                // Si no tienes una imagen, regresamos 404 o podrías redirigir
                return NotFound();
            }

            var bytes = System.IO.File.ReadAllBytes(path);
            return File(bytes, "image/png");
        }
    }
}
