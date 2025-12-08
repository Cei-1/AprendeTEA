using AprendeTEA_19032025.BL;
using AprendeTEA_19032025.Helpers;
using AprendeTEA_19032025.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            // Traer info de perfil (para mostrar nombre bonito, si quieres)
            var perfilResult = BL.Perfil.GetPerfilByIdUsuario(usuario.IdUsuario);
            string nombreMostrar = usuario.Email;

            if (perfilResult.Correct && perfilResult.Object is Models.Perfil p)
            {
                if (!string.IsNullOrWhiteSpace(p.NombreCompleto))
                    nombreMostrar = p.NombreCompleto;
            }

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
            // Si quieres más info:
            // new Claim("NombreCompleto", $"{info.Nombre} {info.ApellidoPaterno}")
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

            // ✅ Hash productivo usando PasswordHasher (PBKDF2 + salt)
            model.Usuario.PasswordHash = PasswordHelper.HashPassword(model.Password);

            Result result = BL.Registro.Add(model);

            if (result.Correct)
            {
                TempData["Mensaje"] = "Registro completado correctamente.";
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", result.ErrorMessage ?? "Ocurrió un error al registrar el usuario.");
                return View(model);
            }
        }
    }
}
