    using ECommerce512.Data;
using ECommerce512.Models;
using ECommerce512.Models.ViewModels;
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
    [Authorize(Roles = $"{SD.SuperAdmin}")]
    public class UserController : Controller
    {
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRoleRepository _roleRepository;

        public UserController(IApplicationUserRepository applicationUserRepository, UserManager<ApplicationUser> userManager, IRoleRepository roleRepository)
        {
            _applicationUserRepository = applicationUserRepository;
            _userManager = userManager;
            _roleRepository = roleRepository;
        }

        public async Task<IActionResult> Index()
        {
            var users = _applicationUserRepository.Get();

            Dictionary<ApplicationUser, string> keyValuePairs = new();

            foreach (var item in users)
            {
                var userRoles = await _userManager.GetRolesAsync(item);
                keyValuePairs.Add(item, String.Join(", ", userRoles));
            }


            return View(keyValuePairs);
        }

        public async Task<IActionResult> ChangeRole(string id)
        {
            var roles = _roleRepository.Get();
            var user = await _userManager.FindByIdAsync(id);

            if(user is not null)
            {
                return View(new UserWithRolesVM()
                {
                    ApplicationUser = user,
                    IdentityRoles = roles.ToList()
                }); 

            }

            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> ChangeRole(UserWithRolesVM userWithRolesVM, string role)
        {
            if(!ModelState.IsValid)
            {
                return View(userWithRolesVM);
            }

            var user = await _userManager.FindByIdAsync(userWithRolesVM.ApplicationUser.Id);

            if(user is not null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                await _userManager.RemoveFromRolesAsync(user, userRoles);

                await _userManager.AddToRoleAsync(user, role);
            }

            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = $"{SD.SuperAdmin},{SD.Admin}")]

        public async Task<IActionResult> LockUnLock(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user is not null)
            {
                user.LockoutEnabled = !user.LockoutEnabled;

                if(!user.LockoutEnabled)
                {
                    user.LockoutEnd = DateTime.UtcNow.AddDays(1);
                }
                else
                {
                    user.LockoutEnd = null;
                }

                await _userManager.UpdateAsync(user);

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("NotFoundPage", "Home");
        }
    }
}
