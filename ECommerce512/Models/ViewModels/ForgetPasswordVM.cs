using System.ComponentModel.DataAnnotations;

namespace ECommerce512.Models.ViewModels
{
    public class ForgetPasswordVM
    {
        [Required]
        public string UserNameOREmail { get; set; } = null!;
    }
}
