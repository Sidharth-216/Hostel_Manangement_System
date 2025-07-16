using HostelManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HostelManagementSystem.Controllers
{
    [Authorize(Roles = "Student")]
    public class ApplicationController : Controller
    {
        private readonly HostelDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ApplicationController(HostelDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Apply()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Apply(HostelApplication model)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    Console.WriteLine("❌ No user found.");
                    return Unauthorized();
                }

                if (ModelState.IsValid)
                {
                    model.RollNo = user.Rollno;  // ✅ set Rollno from AppUser
                    model.FullName = user.FullName;
                    model.Branch = user.Branch;
                    model.Email = user.Email;
                    model.Gender = user.Gender;
                    model.PhoneNumber = user.PhoneNumber;

                    _context.Applications.Add(model);
                    await _context.SaveChangesAsync();

                    Console.WriteLine("✅ Application saved successfully.");
                    return RedirectToAction("ApplicationStatus", "Student");
                }

                Console.WriteLine("❌ ModelState is invalid:");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine("   → " + error.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Error: " + ex.Message);
            }

            return View(model);
        }


    }
}
