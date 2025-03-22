using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ECommerce512.Models;
using ECommerce512.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce512.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context = new();

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        var products = _context.Products.Include(e => e.Category);

        // More Logic

        return View(products.ToList());
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
