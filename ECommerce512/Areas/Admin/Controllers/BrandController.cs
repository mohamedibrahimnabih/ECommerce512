using ECommerce512.Data;
using ECommerce512.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce512.Areas.Admin.Controllers
{
    public class BrandController : Controller
    {
        private readonly ApplicationDbContext _context = new();

        public IActionResult Index()
        {
            var Brands = _context.Brands;

            return View(Brands.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Brand brand)
        {
            _context.Brands.Add(brand);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}

