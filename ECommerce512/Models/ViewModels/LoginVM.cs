using System.ComponentModel.DataAnnotations;

namespace ECommerce512.Models.ViewModels
{
    public class LoginVM
    {
        [Required]
        public string UserNameOREmail { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        public bool RememberMe { get; set; }
    }
}
