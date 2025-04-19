    using ECommerce512.Data;
using ECommerce512.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Drawing2D;

namespace ECommerce512.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context = new();

        public IActionResult Index()
        {
            var categories = _context.Categories;

            return View(categories.ToList());
        }

        public IActionResult Create()
        {
            return View(new Category());
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            if(ModelState.IsValid)
            {
                _context.Categories.Add(category);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        public IActionResult Edit(int id)
        {
            var category = _context.Categories.Find(id);

            if(category is not null)
            {
                return View(category);
            }

            return RedirectToAction("NotFoundPage", "Home");
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if(ModelState.IsValid)
            {
                _context.Categories.Update(category);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        public IActionResult Delete(int id)
        {
            var category = _context.Categories.Find(id);

            if (category is not null)
            {
                _context.Remove(category);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("NotFoundPage", "Home");
        }
    }
}
