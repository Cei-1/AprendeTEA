using AprendeTEA_19032025.BL;
using AprendeTEA_19032025.Helpers;
using AprendeTEA_19032025.Models;
using Hangfire;
using Hangfire.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;

namespace AprendeTEA_19032025.Controllers
{
    [AllowAnonymous]

    public class LoginController : Controller
    {


        [HttpGet]
        public IActionResult Index()
        {
            return View(new Models.Login());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(Models.Login model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = BL.Usuario.GetByEmail(model.Email);

            if (!result.Correct || result.Object == null)
            {
                ModelState.AddModelError("", "Correo o contraseña incorrectos.");
                return View(model);
            }

            var usuario = (Models.Usuario)result.Object;



            if (!usuario.Estatus)
            {
                ModelState.AddModelError("", "El usuario está inactivo.");
                return View(model);
            }

            if (!usuario.EmailConfirmado)
            {
                ModelState.AddModelError("",
                    "Tu correo no ha sido confirmado. Revisa tu bandeja o solicita un nuevo enlace.");

                ViewBag.Reenviar = usuario.IdUsuario; // para el botón
                return View(model);
            }
            // Traer info de perfil (para mostrar nombre bonito)
            var perfilResult = BL.Perfil.GetPerfilByIdUsuario(usuario.IdUsuario);
            string nombreMostrar = usuario.Email;
            string primerNombre = "";

            if (perfilResult.Correct && perfilResult.Object is Models.Perfil p)
            {
                if (!string.IsNullOrWhiteSpace(p.NombreCompleto))
                    nombreMostrar = p.NombreCompleto;

                // Primer nombre del perfil (puede tener varios separados por espacio)
                if (!string.IsNullOrWhiteSpace(p.Nombre))
                    primerNombre = p.Nombre.Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? "";
            }

            // Si no hay perfil, usar la parte del email antes del @
            if (string.IsNullOrWhiteSpace(primerNombre))
                primerNombre = usuario.Email.Split('@')[0];

            bool passwordOk = PasswordHelper.VerifyPassword(usuario.PasswordHash, model.Password);

            if (!passwordOk)
            {
                ModelState.AddModelError("", "Correo o contraseña incorrectos.");
                return View(model);
            }

            var rol = usuario.NombrePerfil?.Trim();

            if (string.Equals(rol, "admin", StringComparison.OrdinalIgnoreCase))
                rol = "Admin";
            else
                rol = "Usuario";
            // =============================
            // 1) Crear claims del usuario
            // =============================
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
            new Claim(ClaimTypes.Name, usuario.Email),
            new Claim("PrimerNombre", primerNombre), // 👈 Primer nombre para mostrar en navbar
            // 🔹 Aquí usamos el perfil como rol
            new Claim(ClaimTypes.Role, rol),
        };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true, // "Recordar sesión"
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
            };

            // =============================
            // 2) Firmar la cookie de login
            // =============================
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                authProperties);

            // (Opcional) guardar Id en Session si lo quieres:
            // HttpContext.Session.SetInt32("IdUsuario", usuario.IdUsuario);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // Cerrar sesión de autenticación (borra la cookie)
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            // Limpiar cualquier dato que tengas en Session (si lo usas)
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }

        public IActionResult AccessDenied()
        {
            return View(); // Una vista sencilla que diga "No tienes permiso"
        }

        [HttpGet]
        public IActionResult Registro()
        {
            var model = new Models.Registro();
            model.Estados = new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "Selecciona Estado" },
                new SelectListItem { Value = "Aguascalientes", Text = "Aguascalientes" },
                new SelectListItem { Value = "Baja California", Text = "Baja California" },
                new SelectListItem { Value = "Baja California Sur", Text = "Baja California Sur" },
                new SelectListItem { Value = "Campeche", Text = "Campeche" },
                new SelectListItem { Value = "Chiapas", Text = "Chiapas" },
                new SelectListItem { Value = "Chihuahua", Text = "Chihuahua" },
                new SelectListItem { Value = "Ciudad de México", Text = "Ciudad de México" },
                new SelectListItem { Value = "Coahuila", Text = "Coahuila" },
                new SelectListItem { Value = "Colima", Text = "Colima" },
                new SelectListItem { Value = "Durango", Text = "Durango" },
                new SelectListItem { Value = "Estado de México", Text = "Estado de México" },
                new SelectListItem { Value = "Guanajuato", Text = "Guanajuato" },
                new SelectListItem { Value = "Guerrero", Text = "Guerrero" },
                new SelectListItem { Value = "Hidalgo", Text = "Hidalgo" },
                new SelectListItem { Value = "Jalisco", Text = "Jalisco" },
                new SelectListItem { Value = "Michoacán", Text = "Michoacán" },
                new SelectListItem { Value = "Morelos", Text = "Morelos" },
                new SelectListItem { Value = "Nayarit", Text = "Nayarit" },
                new SelectListItem { Value = "Nuevo León", Text = "Nuevo León" },
                new SelectListItem { Value = "Oaxaca", Text = "Oaxaca" },
                new SelectListItem { Value = "Puebla", Text = "Puebla" },
                new SelectListItem { Value = "Querétaro", Text = "Querétaro" },
                new SelectListItem { Value = "Quintana Roo", Text = "Quintana Roo" },
                new SelectListItem { Value = "San Luis Potosí", Text = "San Luis Potosí" },
                new SelectListItem { Value = "Sinaloa", Text = "Sinaloa" },
                new SelectListItem { Value = "Sonora", Text = "Sonora" },
                new SelectListItem { Value = "Tabasco", Text = "Tabasco" },
                new SelectListItem { Value = "Tamaulipas", Text = "Tamaulipas" },
                new SelectListItem { Value = "Tlaxcala", Text = "Tlaxcala" },
                new SelectListItem { Value = "Veracruz", Text = "Veracruz" },
                new SelectListItem { Value = "Yucatán", Text = "Yucatán" },
                new SelectListItem { Value = "Zacatecas", Text = "Zacatecas" }
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Registro(Models.Registro model)
        {
            if (!ModelState.IsValid)
            {
                // Regresamos la vista con los errores de validación
                return View(model);
            }

            // Hash productivo usando PasswordHasher (PBKDF2 + salt)
            model.Usuario.PasswordHash = PasswordHelper.HashPassword(model.Password);

            // Asegurarnos que el usuario nuevo nace como NO confirmado y activo
            model.Usuario.EmailConfirmado = false;
            model.Usuario.Estatus = true;
            model.InfoPersonal.Estatus = true;

            Result result = BL.Registro.Add(model);

            //if (result.Correct)
            if (result.Correct && result.Object is Models.RegistroResultado data)
            {
                // 👇 Aquí ya tienes el mismo IdUsuario y Token que se guardaron en BD
                int idGenerado = data.IdUsuario;
                string token = data.EmailConfirmToken;

                // Encolar el correo con Hangfire usando el EmailSender via EmailJobs
                Hangfire.BackgroundJob.Enqueue<AprendeTEA_19032025.Helpers.EmailJobs>(job =>
                    job.EnviarConfirmacion(model.Usuario.Email, idGenerado, token));

                TempData["Mensaje"] = "Registro completado correctamente. Te enviamos un correo para confirmar tu cuenta.";
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", result.ErrorMessage ?? "Ocurrió un error al registrar el usuario.");
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult ConfirmarEmail(int id, string token)
        {
            var result = BL.Usuario.ConfirmarEmail(id, token);

            if (result.Correct)
                return View("EmailConfirmado");

            return View("EmailError");
        }

        [AllowAnonymous]
        public IActionResult ReenviarConfirmacion(int id)
        {
            var result = BL.Usuario.GetById(id);

            if (!result.Correct || result.Object == null)
                return RedirectToAction("Index");

            var usuario = (Models.Usuario)result.Object;

            // Generar un nuevo token cada que se reenvía
            string nuevoToken = Guid.NewGuid().ToString("N");
            BL.Usuario.ActualizarTokenConfirmacion(id, nuevoToken);

            // Enviar por Hangfire
            Hangfire.BackgroundJob.Enqueue<Helpers.EmailJobs>(job =>
                job.EnviarConfirmacion(usuario.Email, id, nuevoToken));

            TempData["Mensaje"] = "Se envió un nuevo enlace de confirmación a tu correo.";
            return RedirectToAction("Index");
        }

    }
}
