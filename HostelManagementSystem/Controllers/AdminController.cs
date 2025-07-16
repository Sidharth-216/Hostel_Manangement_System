using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using HostelManagementSystem.Models;
using HostelManagementSystem.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using HostelManagementSystem.Models; 

namespace HostelManagementSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly HostelDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public AdminController(HostelDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        // Display all student applications
        public IActionResult ViewApplication()
        {
            var applications = _context.Applications.ToList();
            return View(applications);
        }

        // Approve application (POST)
        [HttpPost]
        public IActionResult Approve(int id)
        {
            var application = _context.Applications.Find(id);
            if (application != null && application.Status == "Pending")
            {
                application.Status = "Approved";
                _context.SaveChanges();
                TempData["Success"] = "Application approved successfully.";
            }
            else
            {
                TempData["Error"] = "Application not found or already processed.";
            }
            return RedirectToAction("ViewApplication");
        }

        // Reject application (POST)
        [HttpPost]
        public IActionResult Reject(int id)
        {
            var application = _context.Applications.Find(id);
            if (application != null && application.Status == "Pending")
            {
                application.Status = "Rejected";
                _context.SaveChanges();
                TempData["Success"] = "Application rejected.";
            }
            else
            {
                TempData["Error"] = "Application not found or already processed.";
            }
            return RedirectToAction("ViewApplication");
        }

        [HttpGet]
        public IActionResult DeleteApplication(int id)
        {
            var application = _context.Applications.Find(id);
            if (application == null)
            {
                return NotFound();
            }
            return View(application);
        }

        [HttpPost, ActionName("DeleteApplication")]
        public IActionResult DeleteApplicationConfirmed(int id)
        {
            var application = _context.Applications.Find(id);
            if (application == null)
            {
                return NotFound();
            }
            _context.Applications.Remove(application);
            _context.SaveChanges();
            TempData["Success"] = "Application deleted successfully.";
            return RedirectToAction("ViewApplication");
        }
        // Display room assignment form (GET)
        [HttpGet]
        public IActionResult AssignRoom(string email)
        {
            Console.WriteLine("➡️ AssignRoom GET called with Email: " + email);

            if (string.IsNullOrEmpty(email))
            {
                TempData["Error"] = "Email is missing.";
                return RedirectToAction("ViewApplication");
            }

            var application = _context.Applications
                .FirstOrDefault(a => a.Email == email && (a.Status == "Approved" || a.Status == "Room Assigned"));

            if (application == null)
            {
                Console.WriteLine("❌ Application not eligible or does not exist.");
                TempData["Error"] = "Application not eligible for room assignment.";
                return RedirectToAction("ViewApplication");
            }

            ViewBag.Rooms = new SelectList(_context.Rooms, "Id", "RoomNumber");

            // ✅ explicitly pass ApplicationId to the view
            var allocation = new Allocation
            {
                ApplicationId = application.Id
            };

            return View(allocation);
        }




        // Handle room assignment submission (POST)
        [HttpPost]
        public IActionResult AssignRoom(Allocation allocation)
        {
            try
            {
                Console.WriteLine("🚀 POST AssignRoom called with ApplicationId: " + allocation.ApplicationId + ", RoomId: " + allocation.RoomId);

                if (!ModelState.IsValid)
                {
                    Console.WriteLine("⚠️ Model state invalid.");
                    foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                    {
                        Console.WriteLine("⚠️ Validation error: " + error.ErrorMessage);
                    }

                    ViewBag.Rooms = new SelectList(_context.Rooms, "Id", "RoomNumber");
                    return View(allocation);
                }

                var room = _context.Rooms.FirstOrDefault(r => r.Id == allocation.RoomId);
                if (room == null)
                {
                    ModelState.AddModelError("", "Invalid room selected.");
                    ViewBag.Rooms = new SelectList(_context.Rooms, "Id", "RoomNumber");
                    return View(allocation);
                }

                var currentAllocations = _context.Allocations.Count(a => a.RoomId == allocation.RoomId);
                if (currentAllocations >= room.Capacity)
                {
                    ModelState.AddModelError("", "Room capacity exceeded.");
                    ViewBag.Rooms = new SelectList(_context.Rooms, "Id", "RoomNumber");
                    return View(allocation);
                }

                _context.Allocations.Add(allocation);

                var app = _context.Applications.FirstOrDefault(a => a.Id == allocation.ApplicationId);
                if (app != null)
                {
                    app.Status = "Room Assigned";
                }

                _context.SaveChanges();
                TempData["Success"] = "Room assigned successfully.";
                return RedirectToAction("ViewApplication");
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Exception: " + ex.Message);
                ModelState.AddModelError("", "An unexpected error occurred.");
                ViewBag.Rooms = new SelectList(_context.Rooms, "Id", "RoomNumber");
                return View(allocation);
            }
        }


        [HttpGet]
        public IActionResult Dashboard()
        {
            var viewModel = new AdminDashboardViewModel
            {
                TotalApplications = _context.Applications.Count(),
                PendingApplications = _context.Applications.Count(a => a.Status == "Pending"),
                ApprovedApplications = _context.Applications.Count(a => a.Status == "Approved"),
                AssignedRooms = _context.Allocations.Count()
            };

            return View(viewModel);
        }
        [HttpGet]
        public IActionResult ManageFees()
        {
            var allocations = _context.Allocations
                .Include(a => a.Application)
                .Include(a => a.Room)
                .ToList();

            var model = allocations.Select(a => new AdminFeeViewModel
            {
                RegNo = a.Application?.RegNo ?? "N/A",
                StudentName = a.Application?.FullName ?? "N/A",
                RoomNumber = a.Room?.RoomNumber ?? "Not Assigned",
                FeeAmount = _context.Fees.FirstOrDefault(f => f.ApplicationId == a.ApplicationId)?.Amount ?? 0,
                FeeStatus = _context.Fees.FirstOrDefault(f => f.ApplicationId == a.ApplicationId)?.PaymentStatus ?? "Not Generated",
                FeeGeneratedOn = _context.Fees.FirstOrDefault(f => f.ApplicationId == a.ApplicationId)?.GeneratedOn
            }).ToList();

            return View(model);
        }

        [HttpGet]
        public IActionResult GenerateFee()
        {
            // Only show applications with room assigned and no fee generated yet
            var approvedApplications = _context.Applications
                .Where(a => a.Status == "Room Assigned" &&
                            !_context.Fees.Any(f => f.ApplicationId == a.Id))
                .Select(a => new
                {
                    a.Id,
                    Display = $"{a.FullName} - {a.RollNo} - {a.Status}"
                })
                .ToList();

            ViewBag.Applications = new SelectList(approvedApplications, "Id", "Display");

            return View();
        }


        [HttpPost]
        public IActionResult GenerateFee(int applicationId, decimal amount)
        {
            if (applicationId == 0 || amount <= 0)
            {
                ModelState.AddModelError("", "Please select a valid application and enter a valid amount.");
            }

            // Prevent duplicate fee entries
            var existingFee = _context.Fees.FirstOrDefault(f => f.ApplicationId == applicationId);
            if (existingFee != null)
            {
                ModelState.AddModelError("", "Fee already generated for this application.");
            }

            if (ModelState.IsValid)
            {
                var fee = new Fees
                {
                    ApplicationId = applicationId,
                    Amount = amount,
                    PaymentStatus = "Pending",
                    GeneratedOn = DateTime.Now
                };

                _context.Fees.Add(fee);
                _context.SaveChanges();

                TempData["Success"] = "Fee generated successfully.";
                return RedirectToAction("ManageFees"); // or ViewFee if you plan a fee listing page
            }

            // Reload dropdown list if validation fails
            var approvedApplications = _context.Applications
                .Where(a => a.Status == "Room Assigned" &&
                            !_context.Fees.Any(f => f.ApplicationId == a.Id))
                .Select(a => new
                {
                    a.Id,
                    Display = $"{a.FullName} - {a.RollNo} - {a.Status}"
                })
                .ToList();

            ViewBag.Applications = new SelectList(approvedApplications, "Id", "Display");

            return View();
        }

        // Add this action to handle Manage Rooms page


        // List all rooms
        public IActionResult ManageRooms()
        {
            var rooms = _context.Rooms.ToList();
            return View(rooms);
        }

        // Show form to add a new room (GET)
        [HttpGet]
        public IActionResult AddRoom()
        {
            var blocks = _context.HostelBlocks.ToList();

            if (blocks == null || !blocks.Any())
            {
                Console.WriteLine("⚠️ No Hostel Blocks found in the database.");
            }
            else
            {
                Console.WriteLine($"✅ Found {blocks.Count} hostel blocks.");
            }

            ViewBag.HostelBlocks = new SelectList(blocks, "BlockId", "BlockName");
            return View();
        }

        [HttpPost]
        public IActionResult AddRoom(Room room)
        {
            Console.WriteLine("🔧 Submitting AddRoom POST...");
            Console.WriteLine($"RoomNumber: {room.RoomNumber}, Block: {room.Block}, Capacity: {room.Capacity}, HostelBlockBlockId: {room.HostelBlockBlockId}");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Rooms.Add(room);
                    _context.SaveChanges();

                    Console.WriteLine("✅ Room saved successfully.");
                    TempData["Success"] = "Room added successfully.";
                    return RedirectToAction("ManageRooms");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("❌ Exception while saving room: " + ex.Message);
                    TempData["Error"] = "An error occurred while saving the room.";
                }
            }
            else
            {
                Console.WriteLine("❗ Model state is invalid. Errors:");
                foreach (var key in ModelState.Keys)
                {
                    var errors = ModelState[key].Errors;
                    foreach (var error in errors)
                    {
                        Console.WriteLine($"  ⛔ Field: {key}, Error: {error.ErrorMessage}");
                    }
                }
                TempData["Error"] = "Please fix the errors in the form.";
            }

            // Always reload the block list
            ViewBag.HostelBlocks = new SelectList(_context.HostelBlocks, "BlockId", "BlockName");
            return View(room);
        }


        // Show form to edit room details (GET)
        [HttpGet]
        public IActionResult EditRoom(int id)
        {
            var room = _context.Rooms.Find(id);
            if (room == null)
            {
                return NotFound();
            }

            ViewBag.HostelBlocks = new SelectList(_context.HostelBlocks, "BlockId", "BlockName", room.HostelBlockBlockId);
            return View(room);
        }



        // Handle room edit submission (POST)
        [HttpPost]
        public IActionResult EditRoom(Room room)
        {
            if (ModelState.IsValid)
            {
                _context.Rooms.Update(room);
                _context.SaveChanges();
                TempData["Success"] = "Room updated successfully.";
                return RedirectToAction("ManageRooms");
            }
            return View(room);
        }

        // Show confirmation for deleting a room (GET)
        [HttpGet]
        public IActionResult DeleteRoom(int id)
        {
            var room = _context.Rooms.Find(id);
            if (room == null)
            {
                return NotFound();
            }
            return View(room);
        }

        // Handle room deletion (POST)
        [HttpPost, ActionName("DeleteRoom")]
        public IActionResult DeleteRoomConfirmed(int id)
        {
            var room = _context.Rooms.Find(id);
            if (room == null)
            {
                return NotFound();
            }

            _context.Rooms.Remove(room);
            _context.SaveChanges();
            TempData["Success"] = "Room deleted successfully.";
            return RedirectToAction("ManageRooms");
        }

        // GET: Show form
        [HttpGet]
        public IActionResult warden_register()
        {
            return View();
        }

        // POST: Handle form submission
        [HttpPost]
        public async Task<IActionResult> warden_register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FullName = model.FullName,
                    Branch = model.Branch,
                    Gender = model.Gender,
                    PhoneNumber = model.PhoneNumber
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, model.Role); // Assign Admin or Warden

                    TempData["Success"] = "User registered successfully.";
                    return RedirectToAction("Dashboard");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }


        [HttpGet]
        public IActionResult ManageBlock()
        {
            var blocks = _context.HostelBlocks.ToList();
            return View(blocks);
        }


        [HttpGet]
        public IActionResult ViewFee(string regNo)
        {
            if (string.IsNullOrEmpty(regNo))
            {
                TempData["Error"] = "Registration number is required.";
                return RedirectToAction("ManageFees");
            }

            var application = _context.Applications.FirstOrDefault(a => a.RegNo == regNo);
            if (application == null)
            {
                TempData["Error"] = "No application found for this registration number.";
                return RedirectToAction("ManageFees");
            }

            var fee = _context.Fees.FirstOrDefault(f => f.ApplicationId == application.Id);
            if (fee == null)
            {
                TempData["Error"] = "Fee not generated for this student.";
                return RedirectToAction("ManageFees");
            }

            var allocation = _context.Allocations.FirstOrDefault(a => a.ApplicationId == application.Id);
            var room = allocation != null ? _context.Rooms.FirstOrDefault(r => r.Id == allocation.RoomId) : null;

            var model = new AdminFeeViewModel
            {
                StudentName = application.FullName,
                RegNo = application.RegNo,
                RoomNumber = room?.RoomNumber ?? "Not Assigned",
                FeeAmount = fee.Amount,
                FeeStatus = fee.PaymentStatus,
                FeeGeneratedOn = fee.GeneratedOn
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult AddHostelBlock()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddHostelBlock(HostelBlock block)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.HostelBlocks.Add(block);
                    _context.SaveChanges();
                    TempData["Success"] = "Hostel Block added successfully.";
                    return RedirectToAction("ManageBlock"); // or redirect anywhere else
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error adding block: " + ex.Message);
                    TempData["Error"] = "An error occurred while adding the block.";
                }
            }
            else
            {
                Console.WriteLine("ModelState Invalid");
            }

            return View(block);
        }
        public IActionResult AssignedRooms()
        {
            var data = (from allocation in _context.Allocations
                        join application in _context.Applications on allocation.ApplicationId equals application.Id
                        join room in _context.Rooms on allocation.RoomId equals room.Id
                        join block in _context.HostelBlocks on room.HostelBlockBlockId equals block.BlockId
                        select new AssignedRoomViewModel
                        {
                            StudentName = application.FullName,
                            RegNo = application.RegNo,
                            Branch=application.Branch,
                            RoomNumber = room.RoomNumber,
                            BlockName = block.BlockName,
                            BlockId = block.BlockId
                        }).ToList();

            return View(data);
        }


    }
}
