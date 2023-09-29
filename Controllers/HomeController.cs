using System.Diagnostics;
using HelmoBilite.Models;
using Microsoft.AspNetCore.Mvc;

namespace HelmoBilite.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error","An error has occured. " +
                    "Please contact the webmaster if the problem persists." );
    }
}