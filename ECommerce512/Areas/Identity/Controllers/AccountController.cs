using ECommerce512.Models;
using ECommerce512.Models.ViewModels;
using ECommerce512.Repositories;
using ECommerce512.Repositories.IRepositories;
using ECommerce512.Utitlity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using NuGet.Common;
using System.Threading.Tasks;

namespace ECommerce512.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IApplicationUserOtpRepository _applicationUserOtpRepository;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender, IApplicationUserOtpRepository applicationUserOtpRepository, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _applicationUserOtpRepository = applicationUserOtpRepository;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Register()
        {
            if(_roleManager.Roles.IsNullOrEmpty())
            {
                await _roleManager.CreateAsync(new(SD.SuperAdmin));
                await _roleManager.CreateAsync(new(SD.Admin));
                await _roleManager.CreateAsync(new(SD.Company));
                await _roleManager.CreateAsync(new(SD.Customer));
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View(registerVM);
            }

            ApplicationUser applicationUser = new()
            {
                UserName = registerVM.UserName,
                Email = registerVM.Email,
                Address = registerVM.Address,
            };

            var result = await _userManager.CreateAsync(applicationUser, registerVM.Password);

            if (result.Succeeded)
            {
                string token = await _userManager.GenerateEmailConfirmationTokenAsync(applicationUser);

                var confirmationLink = Url.Action("ConfirmEmail", "Account", new { area = "Identity", applicationUser.Id, token }, Request.Scheme);

                await _emailSender.SendEmailAsync(applicationUser.Email, "Confirmation Email", $"<h1>Confirm Your Account By Click <a href='{confirmationLink}'>Here</a></h1>");

                await _userManager.AddToRoleAsync(applicationUser, SD.Customer);

                TempData["Notification"] = "Add Account successfully, Confirm Your Account";

                return RedirectToAction("Index", "Home", new { area = "Customer" });
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }
            }

            return View(registerVM);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View(loginVM);
            }

            var applicationUser = await _userManager.FindByEmailAsync(loginVM.UserNameOREmail);

            if(applicationUser is null)
            {
                applicationUser = await _userManager.FindByNameAsync(loginVM.UserNameOREmail);
            }

            if(applicationUser is not null)
            {

                if(!applicationUser.LockoutEnabled)
                {
                    ModelState.AddModelError(string.Empty, $"You have block {applicationUser.LockoutEnd}");
                    return View(loginVM);
                }

                var result = await _userManager.CheckPasswordAsync(applicationUser, loginVM.Password);

                if(result)
                {
                    await _signInManager.SignInAsync(applicationUser, loginVM.RememberMe);

                    TempData["Notification"] = "Login successfully";

                    return RedirectToAction("Index", "Home", new { area = "Customer" });
                }

                ModelState.AddModelError("Password", "Invalid Password");
                return View(loginVM);
            }

            ModelState.AddModelError("UserNameOREmail", "Invalid User Name Or Email");
            return View(loginVM);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            TempData["Notification"] = "Login successfully";

            return RedirectToAction("Index", "Home", new { area = "Customer" });
        }

        public async Task<IActionResult> ConfirmEmail(string Id, string token)
        {
            var applicationUser = await _userManager.FindByIdAsync(Id);

            if(applicationUser is not null)
            {

                var result = await _userManager.ConfirmEmailAsync(applicationUser, token);

                if (result.Succeeded)
                {
                    TempData["Notification"] = "Confirmed Email successfully";

                    return RedirectToAction("Index", "Home", new { area = "Customer" });
                }
                else
                {
                    TempData["Notification-error"] = String.Join(", " , result.Errors.Select(e=>e.Description));
                }
            }

            return BadRequest();
        }

        public IActionResult ResendEmailConfirmation()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResendEmailConfirmation(ResendEmailConfirmationVM resendEmailConfirmationVM)
        {
            if (!ModelState.IsValid)
            {
                return View(resendEmailConfirmationVM);
            }

            var applicationUser = await _userManager.FindByEmailAsync(resendEmailConfirmationVM.UserNameOREmail);

            if (applicationUser is null)
            {
                applicationUser = await _userManager.FindByNameAsync(resendEmailConfirmationVM.UserNameOREmail);
            }

            if (applicationUser is not null)
            {
                if(applicationUser.EmailConfirmed)
                {
                    TempData["Notification-error"] = "Already Confirmed";
                }
                else
                {
                    string token = await _userManager.GenerateEmailConfirmationTokenAsync(applicationUser);

                    var confirmationLink = Url.Action("ConfirmEmail", "Account", new { area = "Identity", applicationUser.Id, token }, Request.Scheme);

                    await _emailSender.SendEmailAsync(applicationUser.Email, "Resend Confirmation Email", $"<h1>Confirm Your Account By Click <a href='{confirmationLink}'>Here</a></h1>");

                    TempData["Notification"] = "Send Email successfully";
                }

                return RedirectToAction("Index", "Home", new { area = "Customer" });
            }

            ModelState.AddModelError("UserNameOREmail", "Invalid User Name Or Email");
            return View(resendEmailConfirmationVM);
        }

        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordVM forgetPasswordVM)
        {
            if (!ModelState.IsValid)
            {
                return View(forgetPasswordVM);
            }

            var applicationUser = await _userManager.FindByEmailAsync(forgetPasswordVM.UserNameOREmail);

            if (applicationUser is null)
            {
                applicationUser = await _userManager.FindByNameAsync(forgetPasswordVM.UserNameOREmail);
            }

            if (applicationUser is not null)
            {
                
                string token = await _userManager.GeneratePasswordResetTokenAsync(applicationUser);

                var otpNumber = new Random().Next(1000, 9999);

                //var resetPassword = Url.Action("ResetPassword", "Account", new { area = "Identity", ApplicationUserId = applicationUser.Id, Token = token }, Request.Scheme);

                //await _emailSender.SendEmailAsync(applicationUser.Email, "Reset Password", $"<h1>Reset Password Account By Click <a href='{resetPassword}'>Here</a></h1>");

                var otpInDB = _applicationUserOtpRepository.Get(e => e.ApplicationUserId == applicationUser.Id).LastOrDefault();

                if(otpInDB is not null && (DateTime.UtcNow - otpInDB.ReleaseData).TotalMinutes > 10)
                {
                    await _applicationUserOtpRepository.CreateAsync(new()
                    {
                        ApplicationUserId = applicationUser.Id,
                        OTP = otpNumber,
                        ReleaseData = DateTime.UtcNow,
                        ExpirationData = DateTime.UtcNow.AddMinutes(2)
                    });
                    await _applicationUserOtpRepository.CommitAsync();

                    await _emailSender.SendEmailAsync(applicationUser.Email, "Reset Password", $"<h1>Reset Password Account By this number {otpNumber}</h1>");

                    TempData["Notification"] = "Check Your email, you find otp";
                    TempData["_ValidationToken"] = Guid.NewGuid().ToString();

                    return RedirectToAction("ResetPassword", "Account", new { area = "Identity", ApplicationUserId = applicationUser.Id, Token = token });
                }

                ModelState.AddModelError(String.Empty, "There is error");
                return View(forgetPasswordVM);
            }

            ModelState.AddModelError("UserNameOREmail", "Invalid User Name Or Email");
            return View(forgetPasswordVM);
        }

        public IActionResult ResetPassword()
        {
            if(TempData["_ValidationToken"] is not null)
            {
                return View();
            }

            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM resetPasswordVM)
        {
            if (!ModelState.IsValid)
            {
                return View(resetPasswordVM);
            }

            var applicationUser = await _userManager.FindByIdAsync(resetPasswordVM.ApplicationUserId);

            if(applicationUser is not null)
            {

                var otpInDB = _applicationUserOtpRepository.Get(e => e.ApplicationUserId == resetPasswordVM.ApplicationUserId).LastOrDefault();

                if (otpInDB.OTP == resetPasswordVM.OTP && otpInDB.ExpirationData <= DateTime.UtcNow)
                {
                    var result = await _userManager.ResetPasswordAsync(applicationUser, resetPasswordVM.Token, resetPasswordVM.Password);


                    if (result.Succeeded)
                    {
                        TempData["Notification"] = "Reset Password Successfully";

                        return RedirectToAction("Index", "Home", new { area = "Customer" });
                    }
                    else
                    {
                        foreach (var item in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, item.Description);
                        }
                    }
                }

                ModelState.AddModelError("OTP", "Invalid OTP or Expired");
                return View(resetPasswordVM);
            }

            return BadRequest();
        }
    }
}
