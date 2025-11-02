using GiftoftheGiversApp.Data;
using GiftoftheGiversApp.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;



namespace GiftoftheGiversApp.Controllers
{
    public class IncidentReportController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private ApplicationDbContext context;
        private UserManager<User> userManager;

        public IncidentReportController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IncidentReportController(ApplicationDbContext context, UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            var reports = await _context.IncidentReports
                .Where(r => r.UserId == user.Id)
                .OrderByDescending(r => r.Date)
                .ToListAsync();

            return View(reports);
        }

        
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IncidentReport model)
        {
            ModelState.Remove("UserId");

            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            model.UserId = user.Id;
            model.Date = DateTime.Now;

            _context.IncidentReports.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
