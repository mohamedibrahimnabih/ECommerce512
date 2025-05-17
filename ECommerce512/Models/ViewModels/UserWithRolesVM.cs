using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ECommerce512.Models.ViewModels
{
    public class UserWithRolesVM
    {
        public ApplicationUser ApplicationUser { get; set; }
        [ValidateNever]
        public List<IdentityRole> IdentityRoles { get; set; }
    }
}
