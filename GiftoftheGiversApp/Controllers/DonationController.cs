using GiftoftheGiversApp.Data;
using GiftoftheGiversApp.Models;
using GiftoftheGiversApp.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace GiftoftheGiversApp.Controllers
{
    public class DonationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private ApplicationDbContext context;
        private UserManager<User> userManager;

        public DonationController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public DonationController(ApplicationDbContext context, UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var donations = await _context.Donations
                .Where(d => d.UserId == user.Id)
                .OrderByDescending(d => d.Date)
                .ToListAsync();

            return View(donations);
        }

  
        public IActionResult Create()
        {
            return View();
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Donation model)
        {
         
            ModelState.Remove("UserId");

            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            model.UserId = user.Id;
            model.Date = DateTime.Now;

            _context.Donations.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}

