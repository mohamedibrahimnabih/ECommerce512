using Microsoft.AspNetCore.Identity;

namespace ECommerce512.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Address { get; set; }
        public int Age { get; set; }
    }
}
