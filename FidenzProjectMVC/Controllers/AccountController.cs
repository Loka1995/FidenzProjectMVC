using FidenzProjectMVC.Common.Interfaces;
using FidenzProjectMVC.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FidenzProjectMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;     
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Login(string returnUrl=null)
        {
            returnUrl ??= Url.Content("~/");
            LoginVM loginVM = new ()
            {
                RedirectUrl = returnUrl,
            };
            return View(loginVM);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(loginVM.Email);

                if (user != null)
                {
                    // Attempt to sign in the user with the provided password
                    var result = await _signInManager.PasswordSignInAsync(user.UserName, loginVM.Password, loginVM.RememberMe, lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
                        // Retrieve user roles
                        var roles = await _userManager.GetRolesAsync(user);

                        // Redirect based on role
                        if (roles.Contains("Admin"))
                        {
                            return RedirectToAction("AdminDashboard", "Admin"); // Redirect to Admin view
                        }
                        else if (roles.Contains("User"))
                        {
                            return RedirectToAction("UserDashboard", "User"); // Redirect to User view
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        return View(loginVM);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User not found.");
                    return View(loginVM);
                }
            }
            // Handle failed login
            return View(loginVM);
        }

    }
}
