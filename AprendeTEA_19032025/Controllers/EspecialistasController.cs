using AprendeTEA_19032025.Data;
using Microsoft.AspNetCore.Mvc;

namespace AprendeTEA_19032025.Controllers
{
    public class EspecialistasController : Controller
    {
        private readonly BL.Especialista _especialistaBL;

        public EspecialistasController(ApplicationDbContext context)
        {
            _especialistaBL = new BL.Especialista(context);
        }

        // Public catalog view
        public IActionResult Catalogo()
        {
            Models.Especialista especialista = new Models.Especialista();
            Models.Result result = BL.Especialista.GetAll();

            if (result.Correct)
            {
                especialista.Especialistas = result.Objects.ToList();
            }

            return View(especialista);
        }

        // Administrative list view
        public IActionResult Index()
        {
            Models.Especialista especialista = new Models.Especialista();
            Models.Result result = BL.Especialista.GetAll();

            if (result.Correct)
            {
                especialista.Especialistas = result.Objects.ToList();
            }

            return View(especialista);
        }

        // Card view for sharing specialist info as image
        [HttpGet]
        public IActionResult Card(int IdEspecialista)
        {
            var result = BL.Especialista.GetById(IdEspecialista);

            if (result.Correct)
            {
                Models.Especialista especialista = (Models.Especialista)result.Object;
                return View(especialista);
            }

            // If not found, redirect to index
            TempData["Mensaje"] = "Especialista no encontrado.";
            return RedirectToAction("Index");
        }

        // Form for Insert/Update
        [HttpGet]
        public IActionResult Form(int? IdEspecialista)
        {
            Models.Especialista especialista = new Models.Especialista();

            if (IdEspecialista == null)
            {
                return View(especialista);
            }
            else
            {
                var result = BL.Especialista.GetById(IdEspecialista.Value);

                if (result.Correct)
                {
                    especialista = (Models.Especialista)result.Object;
                }

                return View(especialista);
            }
        }

        // Save (Insert/Update)
        [HttpPost]
        [RequestSizeLimit(10_485_760)] 
        [RequestFormLimits(MultipartBodyLengthLimit = 10_485_760)] // 10 MB
        public async Task<IActionResult> Form(Models.Especialista especialista, IFormFile? FotografiaFile)
        {
            try
            {
                // Convert image to Base64 if uploaded
                if (FotografiaFile != null && FotografiaFile.Length > 0)
                {
                    // Validate file size (max 5MB for better compatibility)
                    if (FotografiaFile.Length > 5 * 1024 * 1024)
                    {
                        TempData["Mensaje"] = "La imagen no puede superar los 5MB.";
                        return View(especialista);
                    }

                    // Validate file type
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var extension = Path.GetExtension(FotografiaFile.FileName).ToLowerInvariant();
                    if (!allowedExtensions.Contains(extension))
                    {
                        TempData["Mensaje"] = "Solo se permiten imágenes (jpg, jpeg, png, gif).";
                        return View(especialista);
                    }

                    try
                    {
                        especialista.FotografiaBase64 = await ConvertImageToBase64(FotografiaFile);
                    }
                    catch (Exception imgEx)
                    {
                        TempData["Mensaje"] = $"Error al procesar la imagen: {imgEx.Message}";
                        return View(especialista);
                    }
                }

                if (especialista.IdEspecialista == 0)
                {
                    // Insert
                    Models.Result result = BL.Especialista.Add(especialista);
                    if (!result.Correct)
                    {
                        TempData["Mensaje"] = $"Error al guardar: {result.ErrorMessage}";
                        if (result.Ex != null)
                        {
                            TempData["Mensaje"] += $" - Detalles: {result.Ex.ToString()}";
                        }
                        return View(especialista);
                    }
                    TempData["Mensaje"] = "Especialista registrado correctamente.";
                }
                else
                {
                    // Update - if no new image was uploaded, keep the existing one
                    if (FotografiaFile == null || FotografiaFile.Length == 0)
                    {
                        var existingResult = BL.Especialista.GetById(especialista.IdEspecialista);
                        if (existingResult.Correct)
                        {
                            var existing = (Models.Especialista)existingResult.Object;
                            especialista.FotografiaBase64 = existing.FotografiaBase64;
                        }
                    }

                    Models.Result result = BL.Especialista.Update(especialista);
                    if (!result.Correct)
                    {
                        TempData["Mensaje"] = $"Error al actualizar: {result.ErrorMessage}";
                        if (result.Ex != null)
                        {
                            TempData["Mensaje"] += $" - Detalles: {result.Ex.ToString()}";
                        }
                        return View(especialista);
                    }
                    TempData["Mensaje"] = "Especialista actualizado correctamente.";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = $"Error inesperado: {ex.Message} - Stack: {ex.StackTrace}";
                return View(especialista);
            }
        }

        // Delete
        [HttpGet]
        public IActionResult Delete(int IdEspecialista)
        {
            Models.Result result = BL.Especialista.Delete(IdEspecialista);

            if (result.Correct)
            {
                TempData["Mensaje"] = "Especialista eliminado correctamente.";
            }
            else
            {
                TempData["Mensaje"] = $"Error al eliminar: {result.ErrorMessage}";
            }

            return RedirectToAction("Index");
        }

        // Helper method: Convert image to Base64
        private async Task<string> ConvertImageToBase64(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                byte[] imageBytes = memoryStream.ToArray();
                string base64String = Convert.ToBase64String(imageBytes);
                
                // Get the file extension to determine MIME type
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                string mimeType = extension switch
                {
                    ".jpg" or ".jpeg" => "image/jpeg",
                    ".png" => "image/png",
                    ".gif" => "image/gif",
                    _ => "image/jpeg"
                };

                return $"data:{mimeType};base64,{base64String}";
            }
        }
    }
}
