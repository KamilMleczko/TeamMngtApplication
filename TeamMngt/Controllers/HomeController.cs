using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TeamMngt.Models;

namespace TeamMngt.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    
    public IActionResult Index()
    {
        if (HttpContext.Session.Keys.Contains("nazwa"))
        {
            ViewData["nazwa"] = "Zalogowano: " + HttpContext.Session.GetString("nazwa");
            return View();
        }
        else
        {
            ViewData["nazwa"] = "Jesteś niezalogowany"; 
            return View();
        }
    }
    

    public IActionResult Privacy()
    {
        if (HttpContext.Session.Keys.Contains("nazwa"))
        {
            ViewData["nazwa"] = "Zalogowano: " + HttpContext.Session.GetString("nazwa");
            return View();
        }
        else
        {
            ViewData["nazwa"] = "Jesteś niezalogowany"; 
            return View();
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}