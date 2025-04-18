﻿using ECommerce512.Data;
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
            var brands = _context.Brands;

            return View(brands.ToList());
        }

        public IActionResult Create()
        {
            return View(new Brand());
        }

        [HttpPost]
        public IActionResult Create(Brand brand)
        {
            if(ModelState.IsValid)
            {
                _context.Brands.Add(brand);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(brand);
        }

        public IActionResult Edit(int id)
        {
            var brand = _context.Brands.Find(id);

            if(brand is not null)
            {
                return View(brand);
            }

            return RedirectToAction("NotFoundPage", "Home");
        }

        [HttpPost]
        public IActionResult Edit(Brand brand)
        {
            if(ModelState.IsValid)
            {
                _context.Brands.Update(brand);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(brand);
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

