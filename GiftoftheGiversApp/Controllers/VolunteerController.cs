using GiftoftheGiversApp.Data;
using GiftoftheGiversApp.Models;

using GiftoftheGiversApp.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace GiftoftheGiversApp.Controllers
{
    public class VolunteerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public VolunteerController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            var volunteers = await _context.Volunteers
                .Where(v => v.UserId == user.Id)
                .OrderByDescending(v => v.Availability)
                .ToListAsync();

            return View(volunteers);
        }

        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            
            var existing = await _context.Volunteers.FirstOrDefaultAsync(v => v.UserId == user.Id);
            if (existing != null)
            {
                TempData["ErrorMessage"] = "You have already registered as a volunteer.";
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Volunteer model)
        {
            ModelState.Remove("UserId");

            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            var existing = await _context.Volunteers.FirstOrDefaultAsync(v => v.UserId == user.Id);
            if (existing != null)
            {
                TempData["ErrorMessage"] = "You have already registered as a volunteer.";
                return RedirectToAction(nameof(Index));
            }

            model.UserId = user.Id;

            _context.Volunteers.Add(model);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "You have successfully registered as a volunteer!";
            return RedirectToAction(nameof(Index));
        }
    }
}
