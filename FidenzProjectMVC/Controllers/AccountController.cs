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
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AccountController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager, IJwtTokenGenerator jwtTokenGenerator)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;
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

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
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
                        var role = roles.FirstOrDefault();

                        // Generate JWT token
                        var token = _jwtTokenGenerator.GenerateJwtToken(user.Email, role);

                        // Save the token in a cookie
                        HttpContext.Response.Cookies.Append("AuthToken", token, new CookieOptions
                        {
                            HttpOnly = true,
                            Expires = DateTime.Now.AddMinutes(30) // Adjust the expiration as needed
                        });

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
                        // If the password is incorrect
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        return View(loginVM);
                    }
                }
                else
                {
                    // If the user is not in the system
                    ModelState.AddModelError(string.Empty, "User not found.");
                    return View(loginVM);
                }
            }
            // Handle failed login
            return View(loginVM);
        }

    }
}
