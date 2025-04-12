using ECommerce512.Data;
using ECommerce512.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerce512.Areas.Admin.Controllers
{
    [Area("Admin")]
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

        public IActionResult Edit(int id)
        {
            var brand = _context.Brands.Find(id);

            if (brand is not null)
            {
                return View(brand);
            }

            return View();
        }

        [HttpPost]
        public IActionResult Edit(Brand brand)
        {
            var brandInDb = _context.Brands.AsNoTracking().FirstOrDefault(e => e.Id == brand.Id);

            if (brandInDb is not null)
            {
                _context.Brands.Update(brand);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("NotFoundPage", "Home");
        }

        public IActionResult Delete(int id)
        {
            var brand = _context.Brands.Find(id);

            if (brand is not null)
            {
                _context.Remove(brand);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("NotFoundPage", "Home");
        }
    }
}

