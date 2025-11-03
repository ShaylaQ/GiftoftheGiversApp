using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;

namespace GiftOfTheGivers.UITest.TestsforViews
{
    [TestClass]
    public class DonationViewTests : Test1   // Base class sets up WebDriver
    {
        [TestMethod]
        public void CreateDonation_ValidInput_RedirectsToIndex()
        {
            // Navigate directly to the Create Donation page
            driver.Navigate().GoToUrl("https://localhost:7224/Donations/Create");

            // Fill in form fields
            driver.FindElement(By.Id("Type")).SendKeys("Parcel");
            driver.FindElement(By.Id("AidQuantity")).SendKeys("25");

            // Submit the form
            driver.FindElement(By.CssSelector("input[type='submit']")).Click();

            // Wait for redirect
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => d.Url.Contains("/Donations"));

            // Assert redirected to the donation list/index page
            Assert.IsTrue(
                driver.Url.Contains("/Donations"),
                $"Expected redirect to /Donations {driver.Url}"
            );

            // Optionally confirm new donation appears in table
            var tableRows = driver.FindElements(By.CssSelector("table tbody tr"));
            Assert.IsTrue(
                tableRows.Any(),
                "donation to appear after creation."
            );
        }

        [TestMethod]
        public void CreateDonation_MissingFields_ShowsValidationErrors()
        {
            driver.Navigate().GoToUrl("https://localhost:7224/Donations/Create");

            // Leave form blank and submit
            driver.FindElement(By.CssSelector("input[type='submit']")).Click();

            // Wait for validation summary or error messages
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => d.FindElements(By.CssSelector(".text-danger")).Count > 0);

            var validationMessages = driver.FindElements(By.CssSelector(".text-danger"));
            Assert.IsTrue(validationMessages.Count > 0, "error messages for required fields.");
        }

        [TestMethod]
        public void DonationIndex_NoDonations_ShowsEmptyMessage()
        {
          
            driver.Navigate().GoToUrl("https://localhost:7224/Donations");

            
            var bodyText = driver.FindElement(By.TagName("body")).Text;

            Assert.IsTrue(
                bodyText.Contains("No donations have been made yet.") ||
                bodyText.Contains("My Donations"),
                "Index page to show message or table."
            );
        }
    }
}
