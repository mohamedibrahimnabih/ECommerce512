using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ECommerce512.Models;
using ECommerce512.Data;
using Microsoft.EntityFrameworkCore;
using ECommerce512.Models.ViewModels;

namespace ECommerce512.Areas.Customer.Controllers;

[Area("Customer")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context = new();

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index(int categoryId, string? query, double minPrice, double maxPrice, int page = 1)
        {
        IQueryable<Product> products = _context.Products.Include(e => e.Category);

        var categories = _context.Categories;
        ViewData["categories"] = categories.ToList();
        //ViewBag.categories = categories.ToList();

        if (categoryId > 0 && categoryId < categories.Count())
        {
            products = products.Where(e => e.CategoryId == categoryId);
            ViewBag.categoryId = categoryId;
        }

        if (query is not null)
        {
            products = products.Where(e => e.Name.Contains(query));
            ViewBag.query = query;
        }

        if (minPrice > 0)
        {
            products = products.Where(e => e.Price >= (decimal)minPrice);
            ViewBag.minPrice = minPrice;
        }

        if (maxPrice > 0)
        {
            products = products.Where(e => e.Price <= (decimal)maxPrice);
            ViewBag.maxPrice = maxPrice;
        }

        products = products.Skip((page - 1) * 8).Take(8);
        ViewBag.TotalCountOfProduct = Math.Ceiling(_context.Products.Count() / 8.0);

        return View(products.ToList());
    }

    public IActionResult Details(int id)
    {
        var product = _context.Products.Find(id);

        if(product is not null)
        {
            var relatedProducts = _context.Products.Include(e => e.Category).Where(e => e.Name.Contains(product.Name.Substring(0, 5)) && e.Id != id).Skip(0).Take(4).ToList();

            var sameCategory = _context.Products.Include(e => e.Category).Where(e => e.CategoryId == product.CategoryId && e.Id != id).Skip(0).Take(4).ToList();

            var returnedData = new ProductWithRelatedVM()
            {
                Product = product,
                RelatedProducts = relatedProducts,
                SameCategory = sameCategory
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