using GiftoftheGiversApp.Controllers;
using GiftoftheGiversApp.Data;
using GiftoftheGiversApp.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


// Test Failed because of Mock u, however when added there is an error in code.
namespace GiftoftheGiversApp.Tests
{
    [TestClass]
    public class DonationControllerTest
    {
        private ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new ApplicationDbContext(options);

            // Seed user
            var user = new User
            {
                UserName = "shaylatest",
                Email = "shayla@test.com",
                Name = "Shayla"
            };
            var hasher = new PasswordHasher<User>();
            user.PasswordHash = hasher.HashPassword(user, "Shayla123");
            context.Users.Add(user);
            context.SaveChanges();

            return context;
        }

        private UserManager<User> GetUserManager(ApplicationDbContext context)
        {
            var store = new UserStore<User>(context);
            return new UserManager<User>(
                store, null, new PasswordHasher<User>(), null, null, null, null, null, null);
        }

        private DonationController GetController(ApplicationDbContext context, UserManager<User> userManager)
        {
            var controller = new DonationController(context, userManager);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            return controller;
        }

        [TestMethod]
        public async Task Index_ReturnsDonationsForUser()
        {
            
            var context = GetInMemoryDbContext();
            var userManager = GetUserManager(context);
            var controller = GetController(context, userManager);

            var user = await userManager.FindByEmailAsync("Shayla@test.com");

            context.Donations.Add(new Donation
            {
                UserId = user.Id,
                Date = DateTime.Now
            });
            context.SaveChanges();

            
            var result = await controller.Index() as ViewResult;

            
            Assert.IsNotNull(result);

            var model = result.Model as List<Donation>;
            Assert.IsNotNull(model);
            Assert.AreEqual(1, model.Count); 
        }

        [TestMethod]
        public async Task Create_Post_AddsDonation()
        {
          
            var context = GetInMemoryDbContext();
            var userManager = GetUserManager(context);
            var controller = GetController(context, userManager);

            var user = await userManager.FindByEmailAsync("Shayla@test.com");

            var donation = new Donation
            {
                UserId = user.Id,
                Date = DateTime.Now
            };

            var result = await controller.Create(donation) as RedirectToActionResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(nameof(DonationController.Index), result.ActionName);

            var addedDonation = await context.Donations.FirstOrDefaultAsync();
            Assert.IsNotNull(addedDonation);
            Assert.AreEqual(user.Id, addedDonation.UserId);
        }
    }
}
