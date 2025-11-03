using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftOfTheGivers.UITest.TestsforViews
{
    [TestClass]
    public class LoginViewTest : Test1
    {
        [TestMethod]
        public void AdminLogin_ValidCredentials_RedirectsToVolunteerSignUp()
        {
            driver.Navigate().GoToUrl("https://localhost:7224/Account/Login");

            driver.FindElement(By.Id("Email")).SendKeys("Valentynshayla@donor.com");
            driver.FindElement(By.Id("Password")).SendKeys("Shayla123");

            driver.FindElement(By.CssSelector("input[type='submit']")).Click();

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d =>
                d.Url.Contains("/User/Volunteer") ||
                d.Url.Contains("/Volunteers/Create") ||
                d.Url.Contains("/Home") 
            );
            Assert.IsTrue(
                driver.Url.Contains("/User/Volunteer") ||
                driver.Url.Contains("/Volunteers/Create") ||
                driver.Url.Contains("/Home"),
                $"Unexpected redirect after login: {driver.Url}"
            );
        }
    }
}
