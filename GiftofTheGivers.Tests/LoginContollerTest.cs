using GiftoftheGiversApp.Controllers;
using GiftoftheGiversApp.Data;
using GiftoftheGiversApp.Models.Entities;
using GiftoftheGiversApp.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;

namespace GiftoftheGiversApp.Tests
{
    public class LoginControllerTest
    {
        private ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new ApplicationDbContext(options);

            // Seed user for data
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

        private AccountController GetController(ApplicationDbContext context)
        {
            var controller = new AccountController(context);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            return controller;
        }

        [Fact]
        public async Task Login_Post_ValidCredentials_RedirectsToHome()
        {
            var context = GetInMemoryDbContext();
            var controller = GetController(context);

            var model = new LoginViewModel
            {
                Email = "shayla@test.com",
                Password = "Shayla123"
            };

            var result = await controller.Login(model) as RedirectToActionResult;

            Assert.IsNotNull(result);
            Assert.Equals("Index", result.ActionName);
        }

        [Fact]
        public async Task Login_Post_InvalidCredentials_ReturnsViewWithModelError()
        {
            var context = GetInMemoryDbContext();
            var controller = GetController(context);

            var model = new LoginViewModel
            {
                Email = "invalid",
                Password = "invalid"
            };

            var result = await controller.Login(model) as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsFalse(controller.ModelState.IsValid);
        }
    }
}
