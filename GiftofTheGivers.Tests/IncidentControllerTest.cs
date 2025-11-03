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
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace GiftoftheGiversApp.Tests
{
    [TestClass]
    public class IncidentControllerTest
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
                Id = Guid.NewGuid().ToString(),
                UserName = "Valentynshayla@donor.com",
                Email = "Valentynshayla@donor.com",
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
                store,
                null,
                new PasswordHasher<User>(),
                null, null, null, null, null, null
            );
        }

        private IncidentReportController GetController(ApplicationDbContext context, UserManager<User> userManager)
        {
            var controller = new IncidentReportController(context, userManager)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };

            return controller;
        }

        [TestMethod]
        public async Task Index_ReturnsReportsForUser()
        {
            
            var context = GetInMemoryDbContext();
            var userManager = GetUserManager(context);
            var controller = GetController(context, userManager);
            var user = await userManager.FindByEmailAsync("shayla@test.com");

            context.IncidentReports.Add(new IncidentReport
            {
                UserId = user.Id,
                Date = DateTime.Now,
                Incident = "Gas leak ",
                Location = "Mitchells plain",
                Description = "Factory fire."
            });
            context.SaveChanges();

            
            var result = await controller.Index() as ViewResult;

            
            Assert.IsNotNull(result);
            var model = result.Model as List<IncidentReport>;
            Assert.IsNotNull(model);
            Assert.AreEqual(1, model.Count);
            Assert.AreEqual("Fire", model.First().Incident);
        }

        [TestMethod]
        public async Task Create_Post_AddsIncidentReport()
        {
           
            var context = GetInMemoryDbContext();
            var userManager = GetUserManager(context);
            var controller = GetController(context, userManager);

            var model = new IncidentReport
            {
                Incident = "Flood in hall",
                Location = "Kraaifontein",
                Description = "Severe flooding in hall."
            };

            
            var result = await controller.Create(model) as RedirectToActionResult;

        
            Assert.IsNotNull(result);
            Assert.AreEqual(nameof(IncidentReportController.Index), result.ActionName);
            Assert.AreEqual(1, context.IncidentReports.Count());
        }

        [TestMethod]
        public async Task Create_InvalidModel_ReturnsView()
        {
            var context = GetInMemoryDbContext();
            var userManager = GetUserManager(context);
            var controller = GetController(context, userManager);
            controller.ModelState.AddModelError("Incident", "Required");

            var model = new IncidentReport
            {
                Location = "Kraaifontein",
                Description = "Water leak in hall."
            };

        
            var result = await controller.Create(model) as ViewResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(model, result.Model);
        }
    }
}
