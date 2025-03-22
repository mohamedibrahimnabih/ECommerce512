using Microsoft.AspNetCore.Mvc;

namespace ECommerce512.Controllers
{
    public class WelcomeController : Controller
    {
        public IActionResult Home()
        {
            // Data
            string name = "Mohamed Ali";
            int age = 15;
            double salary = 5000;

            var person = new { name, age, salary };

            return View(model: person);
        }
    }
}
