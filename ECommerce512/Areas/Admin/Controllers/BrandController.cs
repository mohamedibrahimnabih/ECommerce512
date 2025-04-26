using ECommerce512.Data;
using ECommerce512.Models;
using ECommerce512.Repositories;
using ECommerce512.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ECommerce512.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandController : Controller
    {
        //private readonly ApplicationDbContext _context = new();
        private readonly IBrandRepository _brandRepository = new BrandRepository();

        public IActionResult Index()
        {
            var brands = _brandRepository.Get();

            return View(brands.ToList());
        }

        public IActionResult Create()
        {
            return View(new Brand());
        }

        [HttpPost]
        public async Task<IActionResult> Create(Brand brand)
        {
            if(ModelState.IsValid)
            {
                await _brandRepository.CreateAsync(brand);
                await _brandRepository.CommitAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(brand);
        }

        public IActionResult Edit(int id)
        {
            var brand = _brandRepository.GetOne(e => e.Id == id);

            if (brand is not null)
            {
                return View(brand);
            }

            return RedirectToAction("NotFoundPage", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Brand brand)
        {
            if(ModelState.IsValid)
            {
                _brandRepository.Update(brand);
                await _brandRepository.CommitAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(brand);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var brand = _brandRepository.GetOne(e => e.Id == id);

            if (brand is not null)
            {
                _brandRepository.Delete(brand);
                await _brandRepository.CommitAsync();

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("NotFoundPage", "Home");
        }
    }
}

