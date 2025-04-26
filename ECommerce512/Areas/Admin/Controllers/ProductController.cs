using ECommerce512.Data;
using ECommerce512.Models;
using ECommerce512.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Threading.Tasks;

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
        public async Task<IActionResult> Create(Product product, IFormFile MainImg)
        {
            //ModelState.Remove("product.Brand");
            //ModelState.Remove("product.Category");

            if (ModelState.IsValid && MainImg != null && MainImg.Length > 0)
            {
                // Add new img to wwwroot
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(MainImg.FileName);
                
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    await MainImg.CopyToAsync(stream);
                }

                // Update img in Db
                product.MainImg = fileName;

                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

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
        public IActionResult Edit(Product product, IFormFile? MainImg)
        {
            //ModelState.Remove("product.Brand");
            //ModelState.Remove("product.Category");

            var productInDb = _context.Products.AsNoTracking().FirstOrDefault(e => e.Id == product.Id);

            if(ModelState.IsValid && productInDb != null)
            {
                if (MainImg != null && MainImg.Length > 0)
                {
                    // Add new img to wwwroot
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(MainImg.FileName);

                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        MainImg.CopyTo(stream);
                    }

                    // Delete old img from wwwroot
                    var oldFileName = productInDb.MainImg;
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", oldFileName);

                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }

                    // Update img in Db
                    product.MainImg = fileName;
                }
                else
                {
                    // Save the old product img
                    product.MainImg = productInDb.MainImg;
                }

                _context.Products.Update(product);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

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

        public IActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);

            if (product is not null)
            {
                var oldFileName = product.MainImg;
                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", oldFileName);

                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }

                _context.Remove(product);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("NotFoundPage", "Home");
        }

        public IActionResult DeleteImg(int id)
        {
            var product = _context.Products.Find(id);

            if (product is not null)
            {
                var oldFileName = product.MainImg;
                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", oldFileName);

                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }

                product.MainImg = "defaultImg.png";
                _context.SaveChanges();

                return RedirectToAction(nameof(Edit), new { id = id });
            }

            return RedirectToAction("NotFoundPage", "Home");
        }
    }
}
