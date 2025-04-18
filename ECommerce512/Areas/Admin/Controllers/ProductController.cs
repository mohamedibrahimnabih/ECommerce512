﻿using ECommerce512.Data;
using ECommerce512.Models;
using ECommerce512.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerce512.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context = new();

        public IActionResult Index()
        {
            var products = _context.Products
                .Include(e => e.Category)
                .Include(e => e.Brand)
                .Select(e => new ProductWithCategoryWithBrandVM()
                {
                    Id = e.Id,
                    Name = e.Name,
                    Price = e.Price,
                    Quantity = e.Quantity,
                    Rate = e.Rate,
                    CategoryName = e.Category.Name,
                    BrandName = e.Brand.Name,
                    Status = e.Status
                });

            return View(products.ToList());
        }

        public IActionResult Create()
        {
            var categories = _context.Categories.Where(e=>e.Status == true);
            var brands = _context.Brands.Where(e => e.Status == true);

            CategoryWithBrandVM categoryWithBrandVM = new()
            {
                Product = new Product(),
                Categories = categories.ToList(),
                Brands = brands.ToList(),
            };

            return View(categoryWithBrandVM);
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            //ModelState.Remove("product.Brand");
            //ModelState.Remove("product.Category");

            if(!ModelState.IsValid)
            {
                var categories = _context.Categories.Where(e => e.Status == true);
                var brands = _context.Brands.Where(e => e.Status == true);

                CategoryWithBrandVM categoryWithBrandVM = new()
                {
                    Product = new Product(),
                    Categories = categories.ToList(),
                    Brands = brands.ToList(),
                };

                return View(categoryWithBrandVM);
            }

            _context.Products.Add(product);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var product = _context.Products.Find(id);

            if(product is not null)
            {
                var categories = _context.Categories.Where(e => e.Status == true);
                var brands = _context.Brands.Where(e => e.Status == true);

                CategoryWithBrandVM categoryWithBrandVM = new()
                {
                    Product = product,
                    Categories = categories.ToList(),
                    Brands = brands.ToList(),
                };

                return View(categoryWithBrandVM);
            }

            return RedirectToAction("NotFoundPage", "Home");
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            //ModelState.Remove("product.Brand");
            //ModelState.Remove("product.Category");

            if (!ModelState.IsValid)
            {
                var categories = _context.Categories.Where(e => e.Status == true);
                var brands = _context.Brands.Where(e => e.Status == true);

                CategoryWithBrandVM categoryWithBrandVM = new()
                {
                    Product = product,
                    Categories = categories.ToList(),
                    Brands = brands.ToList(),
                };

                return View(categoryWithBrandVM);
            }

            _context.Products.Update(product);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);

            if (product is not null)
            {
                _context.Remove(product);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("NotFoundPage", "Home");
        }
    }
}
