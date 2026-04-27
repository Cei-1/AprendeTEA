using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AprendeTEA_19032025.Controllers;

[Authorize]
public class SorprendizajeMagicoController : Controller
{
    private readonly ILogger<SorprendizajeMagicoController> _logger;

    public SorprendizajeMagicoController(ILogger<SorprendizajeMagicoController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        // Set the active application in session
        HttpContext.Session.SetString("ActiveApp", "SorprendizajeMagico");
        return View();
    }
}
