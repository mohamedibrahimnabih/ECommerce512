using ECommerce512.Data;
using ECommerce512.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ECommerce512.Areas.Customer.Controllers
{
    [Authorize]
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public CartController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> AddToCart(int productId, int count)
        {
            var user = await _userManager.GetUserAsync(User);

            if(user is not null)
            {

                var product = _context.Products.Find(productId);

                if(product is not null)
                {

                    if(count <= product.Quantity && count > 0)
                    {


                        var cartInDb = _context.Carts.FirstOrDefault(e => e.ProductId == productId && e.ApplicationUserId == user.Id);

                        if(cartInDb is not null)
                        {
                            cartInDb.Count += count;
                            _context.SaveChanges();
                        }
                        else
                        {
                            _context.Carts.Add(new()
                            {
                                ApplicationUserId = user.Id,
                                ProductId = productId,
                                Count = count
                            });
                            _context.SaveChanges();
                        }
                        
                        TempData["Notification"] = "Add Product To Cart";
                        return RedirectToAction("Index", "Home", new { area = "Customer" });

                    }

                    return BadRequest();

                }

            }

            return BadRequest();
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if(user is not null)
            {
                var carts = _context.Carts.Include(e => e.Product).Where(e => e.ApplicationUserId == user.Id);

                ViewBag.totalPrice = carts.Sum(e => e.Product.Price * e.Count);

                return View(carts.ToList());
            }

            return BadRequest();
        }

        public async Task<IActionResult> IncrementValue(int productId)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user is not null)
            {

                var cartInDb = _context.Carts.FirstOrDefault(e => e.ProductId == productId && e.ApplicationUserId == user.Id);

                if(cartInDb is not null)
                {
                    var product = _context.Products.Find(productId);

                    if(product is not null)
                    {
                        if(cartInDb.Count + 1 <= product.Quantity)
                        {
                            cartInDb.Count++;
                            _context.SaveChanges();
                        }

                        return RedirectToAction(nameof(Index));
                    }

                    return BadRequest();
                }

                return BadRequest();
            }

            return BadRequest();
        }

        public async Task<IActionResult> DecrementValue(int productId)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user is not null)
            {

                var cartInDb = _context.Carts.FirstOrDefault(e => e.ProductId == productId && e.ApplicationUserId == user.Id);

                if (cartInDb is not null)
                {
                    if (cartInDb.Count > 1)
                    {
                        cartInDb.Count--;
                        _context.SaveChanges();
                    }

                    return RedirectToAction(nameof(Index));
                }

                return BadRequest();
            }

            return BadRequest();
        }

        public async Task<IActionResult> Delete(int productId)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user is not null)
            {

                var cartInDb = _context.Carts.FirstOrDefault(e => e.ProductId == productId && e.ApplicationUserId == user.Id);

                if (cartInDb is not null)
                {
                    _context.Remove(cartInDb);
                    _context.SaveChanges();

                    return RedirectToAction(nameof(Index));
                }

                return BadRequest();
            }

            return BadRequest();
        }
    }
}
