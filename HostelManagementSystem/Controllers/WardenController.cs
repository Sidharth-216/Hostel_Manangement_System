using Microsoft.AspNetCore.Mvc;
//susing HostelManagementSystem.Data;
using HostelManagementSystem.Models;
using Microsoft.EntityFrameworkCore; // For .Include()
using System.Linq;

namespace HostelManagementSystem.Controllers
{
    public class WardenController : Controller
    {
        private readonly HostelDbContext _context;

        public WardenController(HostelDbContext context)
        {
            _context = context;
        }

        public IActionResult WardenDashboard()  
        {
            return View("WardenDashboard");
        }


        /*public IActionResult ViewAssignedStudents()
        {
            var students = _context.Allocations
                .Include(a => a.Application)
                .Include(a => a.Rooms)
                .ThenInclude(r => r.HostelBlock)
                .ToList();

            return View(students);
        }

        public IActionResult PostNotice()
        {
            return View(); // View page for posting notices (to be created)
        }

        public IActionResult ViewComplaints()
        {
            var complaints = _context.Complaints.ToList(); // If Complaints table exists
            return View(complaints);
        }

        public IActionResult HostelCalendar()
        {
            return View(); // Show upcoming hostel events
        }*/
    }
}
