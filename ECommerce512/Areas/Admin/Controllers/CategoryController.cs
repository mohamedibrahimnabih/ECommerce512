    using ECommerce512.Data;
using ECommerce512.Models;
using ECommerce512.Repositories;
using ECommerce512.Repositories.IRepositories;
using ECommerce512.Utitlity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;

namespace ECommerce512.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class CategoryController : Controller
    {
        //private readonly ApplicationDbContext _context;// = new();
        private readonly ICategoryRepository _categoryRepository;// = new CategoryRepository();

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [Authorize(Roles = $"{SD.SuperAdmin},{SD.Admin},{SD.Customer}")]
        public IActionResult Index()
        {
            var categories = _categoryRepository.Get();

            return View(categories.ToList());
        }

        [Authorize(Roles = $"{SD.SuperAdmin},{SD.Admin}")]

        public IActionResult Create()
        {
            return View(new Category());
        }

        [Authorize(Roles = $"{SD.SuperAdmin},{SD.Admin}")]

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if(ModelState.IsValid)
            {
                await _categoryRepository.CreateAsync(category);
                await _categoryRepository.CommitAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }
        [Authorize(Roles = $"{SD.SuperAdmin},{SD.Admin}")]

        public IActionResult Edit(int id)
        {
            var category = _categoryRepository.GetOne(e => e.Id == id);

            if(category is not null)
            {
                return View(category);
            }

            return RedirectToAction("NotFoundPage", "Home");
        }

        [Authorize(Roles = $"{SD.SuperAdmin},{SD.Admin}")]

        [HttpPost]
        public async Task<IActionResult> Edit(Category category)
        {
            if(ModelState.IsValid)
            {
                _categoryRepository.Update(category);
                await _categoryRepository.CommitAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }
        [Authorize(Roles = $"{SD.SuperAdmin},{SD.Admin}")]

        public async Task<IActionResult> Delete(int id)
        {
            var category = _categoryRepository.GetOne(e => e.Id == id);

            if (category is not null)
            {
                _categoryRepository.Delete(category);
                await _categoryRepository.CommitAsync();

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("NotFoundPage", "Home");
        }
    }
}
