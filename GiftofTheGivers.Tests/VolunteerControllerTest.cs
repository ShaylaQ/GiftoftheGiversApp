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
using System.Linq;
using System.Threading.Tasks;

namespace GiftoftheGiversApp.Tests
{
    [TestClass]
    public class VolunteerControllerTests
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
                store,
                null,
                new PasswordHasher<User>(),
                null, null, null, null, null, null
            );
        }

        private VolunteerController GetController(ApplicationDbContext context, UserManager<User> userManager)
        {
            var controller = new VolunteerController(context, userManager)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };

            return controller;
        }

        [TestMethod]
        public async Task Index_ReturnsVolunteersForUser()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var userManager = GetUserManager(context);
            var controller = GetController(context, userManager);
            var user = await userManager.FindByEmailAsync("shayla@test.com");

            context.Volunteers.Add(new Volunteer
            {
                UserId = user.Id,
                TaskType = "Food Distribution",
                Availability = DateTime.Now.AddDays(1)
            });
            context.SaveChanges();

            // Act
            var result = await controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var model = result.Model as List<Volunteer>;
            Assert.IsNotNull(model);
            Assert.AreEqual(1, model.Count);
        }

        [TestMethod]
        public async Task Create_Post_AddsVolunteer()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var userManager = GetUserManager(context);
            var controller = GetController(context, userManager);
            var user = await userManager.FindByEmailAsync("shayla@test.com");

            var volunteer = new Volunteer
            {
                TaskType = "First aid helper.",
                Availability = DateTime.Now.AddDays(2)
            };

            // Act
            var result = await controller.Create(volunteer) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(nameof(VolunteerController.Index), result.ActionName);
            Assert.AreEqual(1, context.Volunteers.Count());
        }

        [TestMethod]
        public async Task Create_DuplicateVolunteer_ReturnsRedirectWithError()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var userManager = GetUserManager(context);
            var controller = GetController(context, userManager);
            var user = await userManager.FindByEmailAsync("shayla@test.com");

            context.Volunteers.Add(new Volunteer
            {
                UserId = user.Id,
                TaskType = "Medical",
                Availability = DateTime.Now.AddDays(3)
            });
            context.SaveChanges();

            var duplicate = new Volunteer
            {
                TaskType = "Medical",
                Availability = DateTime.Now.AddDays(4)
            };

            // Act
            var result = await controller.Create(duplicate) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(nameof(VolunteerController.Index), result.ActionName);
            Assert.AreEqual(1, context.Volunteers.Count());
        }

        [TestMethod]
        public async Task Create_InvalidModel_ReturnsView()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var userManager = GetUserManager(context);
            var controller = GetController(context, userManager);
            controller.ModelState.AddModelError("TaskType", "Required");

            var volunteer = new Volunteer
            {
                Availability = DateTime.Now
            };

            // Act
            var result = await controller.Create(volunteer) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(volunteer, result.Model);
        }
    }
}
