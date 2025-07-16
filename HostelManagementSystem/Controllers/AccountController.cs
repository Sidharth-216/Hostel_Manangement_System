using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using HostelManagementSystem.Models;

namespace HostelManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FullName = model.FullName,
                    Rollno = model.Rollno,
                    Branch = model.Branch,
                    Gender = model.Gender,
                    PhoneNumber = model.PhoneNumber
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    Console.WriteLine(" User creation succeeded");

                    await _userManager.AddToRoleAsync(user, model.Role);
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    Console.WriteLine(" Role assigned and signed in");

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    Console.WriteLine(" User creation failed:");
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"   → {error.Description}");
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            else
            {
                Console.WriteLine(" Model state is not valid");
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        Console.WriteLine($"   → Validation Error: {error.ErrorMessage}");
                    }
                }
            }

            // Ensure the correct ViewModel type is passed to the view
            return View(model);
        }

        


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                    if (result.Succeeded)
                    {
                        var roles = await _userManager.GetRolesAsync(user);
                        if (roles.Contains(model.Role))
                        {
                            if (model.Role == "Admin")
                                return RedirectToAction("Dashboard", "Admin");
                            else if (model.Role == "Warden")
                                return RedirectToAction("WardenDashboard", "Warden");
                            else
                                return RedirectToAction("Dashboard", "Student");
                        }
                        else
                        {
                            ModelState.AddModelError("", $"User does not have the role: {model.Role}");
                        }
                    }
                }

                ModelState.AddModelError("", "Invalid login attempt.");
            }

            return View(model);
        }



        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
