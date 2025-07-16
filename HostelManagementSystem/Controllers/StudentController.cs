using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using HostelManagementSystem.Models;
using System.Threading.Tasks;
using System.Linq;

namespace HostelManagementSystem.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly HostelDbContext _context;

        public StudentController(UserManager<AppUser> userManager, HostelDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

    [Authorize(Roles = "Student")]
    public async Task<IActionResult> Dashboard()
    {
        try
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                Console.WriteLine("User is null.");
                ViewBag.Message = "User not found.";
                return View();
            }

            Console.WriteLine($"Logged-in User FullName: {user.FullName}");

            // Try to get the application by FullName
            var application = _context.Applications.FirstOrDefault(a => a.FullName == user.FullName);

            StudentDashboardViewModel model;

            if (application == null)
            {
                Console.WriteLine($"No application found for FullName: {user.FullName}");

                model = new StudentDashboardViewModel
                {
                    FullName = user.FullName ?? "N/A",
                    Email = user.Email ?? "N/A",
                    RegNo = "Not Available",
                    Rollno = user.Rollno ??"N/A",
                    Branch = user.Branch ?? "N/A",
                    Gender = user.Gender ?? "N/A",
                    PhoneNumber = user.PhoneNumber ?? "N/A",
                    RoomNumber = "Not Assigned",
                    BlockName = "Not Assigned",
                    FeeAmount = 0,
                    FeeStatus = "Not Generated",
                    FeeGeneratedOn = null
                };

                ViewBag.Message = "No hostel application found. Please apply.";
            }
            else
            {
                Console.WriteLine($"Application found for ID: {application.Id}, RegNo: {application.RegNo}");

                var allocation = _context.Allocations.FirstOrDefault(a => a.ApplicationId == application.Id);
                var room = allocation != null ? _context.Rooms.FirstOrDefault(r => r.Id == allocation.RoomId) : null;
                var block = room != null ? _context.HostelBlocks.FirstOrDefault(b => b.BlockId == room.HostelBlockBlockId) : null;
                var fee = _context.Fees.FirstOrDefault(f => f.ApplicationId == application.Id);

                model = new StudentDashboardViewModel
                {
                    FullName = user.FullName ?? "N/A",
                    Email = user.Email ?? "N/A",
                    Rollno = user.Rollno ??"N/A",
                    RegNo = application.RegNo,
                    Branch = user.Branch ?? "N/A",
                    Gender = user.Gender ?? "N/A",
                    PhoneNumber = user.PhoneNumber ?? "N/A",
                    RoomNumber = room?.RoomNumber ?? "Not Assigned",
                    BlockName = block?.BlockName ?? "Not Assigned",
                    FeeAmount = fee?.Amount ?? 0,
                    FeeStatus = fee?.PaymentStatus ?? "Not Generated",
                    FeeGeneratedOn = fee?.GeneratedOn
                };

                ViewBag.Message = null; // no message needed if data is found
            }

            Console.WriteLine("Dashboard model created successfully.");
            return View(model);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception in Dashboard: " + ex.Message);
            Console.WriteLine("StackTrace: " + ex.StackTrace);
            ViewBag.Message = "An error occurred while loading the dashboard.";
            return View();
        }
    }


        [Authorize(Roles = "Student")]
        public async Task<IActionResult> ApplicationStatus()
        {
            var user = await _userManager.GetUserAsync(User);
            var FullName = user.FullName;

            var application = _context.Applications
                .FirstOrDefault(a => a.FullName == FullName);

            if (application == null)
            {
                ViewBag.Message = "You have not applied for hostel yet.";
                return View();
            }

            return View(application);
        }
        [HttpPost]
        public IActionResult PayFee(string regNo)
        {
            var application = _context.Applications.FirstOrDefault(a => a.RegNo == regNo);
            if (application == null)
            {
                TempData["Error"] = "Application not found.";
                return RedirectToAction("Dashboard");
            }

            var fee = _context.Fees.FirstOrDefault(f => f.ApplicationId == application.Id);
            if (fee != null && fee.PaymentStatus == "Pending")
            {
                fee.PaymentStatus = "Paid";
                _context.SaveChanges();
                TempData["Success"] = "Payment successful.";
            }
            else
            {
                TempData["Error"] = "No pending fee found.";
            }

            return RedirectToAction("Dashboard");
        }

    }
}
