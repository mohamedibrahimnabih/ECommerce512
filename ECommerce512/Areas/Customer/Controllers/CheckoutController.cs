using ECommerce512.Data;
using ECommerce512.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using System.Threading.Tasks;

namespace ECommerce512.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public CheckoutController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Pay()
        {

            var user = await _userManager.GetUserAsync(User);

            if(user is not null)
            {
                var carts = _context.Carts.Include(e=>e.Product).Where(e => e.ApplicationUserId == user.Id);

                if (carts.Any())
                {
                    Order order = new()
                    {
                        ApplicationUserId = user.Id,
                        OrderDate = DateTime.UtcNow,
                        OrderStatus = OrderStatus.Pending,
                        TotalPrice = carts.Sum(e => e.Product.Price * e.Count),
                        TransactionStatus = TransactionStatus.Pending
                    };

                    _context.Orders.Add(order);
                    _context.SaveChanges();

                    var options = new SessionCreateOptions
                    {
                        PaymentMethodTypes = new List<string> { "card" },
                        LineItems = new List<SessionLineItemOptions>(),
                        Mode = "payment",
                        SuccessUrl = $"{Request.Scheme}://{Request.Host}/Customer/Checkout/Success?orderId={order.Id}",
                        CancelUrl = $"{Request.Scheme}://{Request.Host}/Customer/Checkout/Cancel",
                    };

                    foreach (var item in carts)
                    {
                        options.LineItems.Add(new SessionLineItemOptions
                        {
                            PriceData = new SessionLineItemPriceDataOptions
                            {
                                Currency = "egp",
                                ProductData = new SessionLineItemPriceDataProductDataOptions
                                {
                                    Name = item.Product.Name,
                                    Description = item.Product.Description,
                                },
                                UnitAmount = (long)item.Product.Price * 100,
                            },
                            Quantity = item.Count,
                        });
                    }

                    var service = new SessionService();
                    var session = service.Create(options);

                    order.SessionId = session.Id;
                    _context.SaveChanges();

                    TempData["_ValidationToken"] = Guid.NewGuid().ToString();

                    return Redirect(session.Url);
                }

                return BadRequest();
            }

            return BadRequest();
        }

        public IActionResult Success(int orderId)
        {
            if(TempData["_ValidationToken"] is not null)
            {

                var order = _context.Orders.Find(orderId);

                if(order is not null)
                {
                    var transaction = _context.Database.BeginTransaction();
                    try
                    {

                        // Decrement product quantity
                        var carts = _context.Carts.Include(e => e.Product).Where(e => e.ApplicationUserId == order.ApplicationUserId);

                        foreach (var item in carts)
                        {
                            item.Product.Quantity -= item.Count;
                        }

                        // Transfer Cart => Order Items
                        foreach (var item in carts)
                        {
                            _context.OrderItems.Add(new()
                            {
                                Count = item.Count,
                                ItemPrice = item.Product.Price,
                                ProductId = item.ProductId,
                                OrderId = orderId
                            });
                        }

                        // Remove Cart
                        _context.RemoveRange(carts);

                        // Update Order Prop.
                        order.OrderStatus = OrderStatus.InProcessing;
                        order.TransactionStatus = TransactionStatus.Completed;

                        var service = new SessionService();
                        var session = service.Get(order.SessionId);
                        order.PaymentId = session.PaymentIntentId;

                        _context.SaveChanges();
                        transaction.Commit();

                        return View();
                    }
                    catch(Exception ex)
                    {
                        transaction.Rollback();
                    }
                }

                return BadRequest();
            }

            return BadRequest();
        }

        public IActionResult Cancel()
        {
            return View();
        }
    }
}
