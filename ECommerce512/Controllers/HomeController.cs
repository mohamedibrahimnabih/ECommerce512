using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ECommerce512.Models;
using ECommerce512.Data;
using Microsoft.EntityFrameworkCore;
using ECommerce512.Models.ViewModels;

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

    public IActionResult Details(int id)
    {
        var product = _context.Products.FirstOrDefault(e => e.Id == id);

        if(product is not null)
        {
            var relatedProducts = _context.Products.Include(e => e.Category).Where(e => e.Name.Contains(product.Name.Substring(0, 5))).Skip(0).Take(4).ToList();

            var returnedData = new ProductWithRelatedVM()
            {
                Product = product,
                RelatedProducts = relatedProducts
            };

            return View(returnedData);
        }

        return RedirectToAction(nameof(NotFoundPage));
    }

    public IActionResult NotFoundPage()
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
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}