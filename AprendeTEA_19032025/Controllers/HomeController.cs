using AprendeTEA_19032025.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AprendeTEA_19032025.Controllers;

[AllowAnonymous]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        // Set default application in session if not set
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("ActiveApp")))
        {
            HttpContext.Session.SetString("ActiveApp", "NeuroPro");
        }
        return View();
    }

    public IActionResult SwitchApplication(string app)
    {
        // Validate and set the active application
        if (app == "NeuroPro" || app == "SorprendizajeMagico")
        {
            HttpContext.Session.SetString("ActiveApp", app);
            
            // Redirect to the appropriate index page
            if (app == "SorprendizajeMagico")
            {
                return RedirectToAction("Index", "SorprendizajeMagico");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        
        // Default fallback
        return RedirectToAction("Index");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
