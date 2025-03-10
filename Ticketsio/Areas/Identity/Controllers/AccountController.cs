using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Ticketsio.Models;
using Ticketsio.Models.ViewModels;

namespace Ticketsio.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        public readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Register()
        {
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
                    LastName = registerVM.LastName,
                    PasswordHash = registerVM.Password
                };
                var result = await userManager.CreateAsync(applicationUser, registerVM.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home", new { area = "Customer" });
                }
                else
                {
                    ModelState.AddModelError("Password", "Password must be at least 8 characters long and contain an uppercase letter, a lowercase letter, a number, and a special character");
                }
            }
            return View(registerVM);
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
                var appuser = await userManager.FindByEmailAsync(loginVM.Email);

                if (appuser != null)
                {
                    var result = await userManager.CheckPasswordAsync(appuser, loginVM.Password);
                    if (result)
                    {
                        await signInManager.SignInAsync(appuser, loginVM.RememberMe);
                        return RedirectToAction("Index", "Home", new { area = "Customer" });
                    }
                    else
                    {
                        ModelState.AddModelError("Email", "Invalid Email");
                        ModelState.AddModelError("Password", "Invalid Password");
                    }
                }
                else
                {
                    ModelState.AddModelError("Email", "Invalid Email");
                    ModelState.AddModelError("Password", "Invalid Password");
                }
            }
            return View(loginVM);
        }
        public IActionResult Logout()
        {
            signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", new { area = "Customer" });
        }
        [HttpGet]
        public IActionResult Profile()
        {
            var account = userManager.GetUserAsync(User).Result;
            return View(account);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Profile(ApplicationUser applicationUser)
        {
            var account = userManager.GetUserAsync(User).Result;
            account.FirstName = applicationUser.FirstName;
            account.LastName = applicationUser.LastName;
            account.Email = applicationUser.Email;
            account.UserName = applicationUser.UserName;
           var success = userManager.UpdateAsync(account);
            if (success.Result.Succeeded)
            {
                return RedirectToAction("Index", "Home", new { area = "Customer" });
            }
            return View(account);
        }
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ForgotPasswordVM forgotPasswordVM)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);
                if (forgotPasswordVM.Email == user.Email)
                {
                    var result = await userManager.ChangePasswordAsync(user, forgotPasswordVM.OldPassword, forgotPasswordVM.NewPassword);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home", new { area = "Customer" });
                    }
                    else
                    {
                        ModelState.AddModelError("OldPassword", "Invalid Password");
                        ModelState.AddModelError("NewPassword", "Password must be at least 8 characters long and contain an uppercase letter, a lowercase letter, a number, and a special character");
                    }
                }
            }
            return View(forgotPasswordVM);
        }
    }
}
