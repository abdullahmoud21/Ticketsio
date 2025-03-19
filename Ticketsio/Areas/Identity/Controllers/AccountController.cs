using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Ticketsio.Models;
using Ticketsio.Models.ViewModels;
using Ticketsio.Services;
using System.Text.Encodings.Web;

namespace Ticketsio.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            if (_roleManager.Roles.IsNullOrEmpty())
            {
                await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
                await _roleManager.CreateAsync(new IdentityRole("User"));
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser = new()
                {
                    UserName = registerVM.UserName,
                    Email = registerVM.Email,
                    FirstName = registerVM.FirstName,
                    LastName = registerVM.LastName
                };

                var result = await _userManager.CreateAsync(applicationUser, registerVM.Password);
                if (result.Succeeded)
                {
                    
                    await _userManager.AddToRoleAsync(applicationUser, "User");

                    
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(applicationUser);
                    var confirmationLink = Url.Action("ConfirmEmail", "Account", new
                    {
                        userId = applicationUser.Id,
                        token = token
                    }, Request.Scheme);

                    
                    await _emailSender.SendEmailAsync(applicationUser.Email, "Confirm Your Email",
                        $"Please confirm your account by clicking this link: <a href='{HtmlEncoder.Default.Encode(confirmationLink)}'>Confirm Email</a>");

                    TempData["Message"] = "Registration successful! Please check your email to confirm your account.";
                    return RedirectToAction("Login");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(registerVM);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
                return BadRequest("Invalid confirmation request.");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("User not found.");

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                TempData["Message"] = "Email confirmed successfully! You can now log in.";
                return RedirectToAction("Login");
            }

            return BadRequest("Email confirmation failed.");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (ModelState.IsValid)
            {
                var appUser = await _userManager.FindByEmailAsync(loginVM.Email);
                if (appUser != null)
                {
                    if (!await _userManager.IsEmailConfirmedAsync(appUser))
                    {
                        ModelState.AddModelError(string.Empty, "You must confirm your email before logging in.");
                        return View(loginVM);
                    }

                    var result = await _signInManager.PasswordSignInAsync(appUser, loginVM.Password, loginVM.RememberMe, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home", new { area = "Customer" });
                    }
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return View(loginVM);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", new { area = "Customer" });
        }
    }
}
