using GiftOfTheGivers.UITest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;

namespace GiftoftheGiversApp.UITests
{
    [TestClass]
    public class VolunteerIndexTests : Test1
    {
        [TestMethod]
        public void VolunteerIndex_NoRegistrations_ShowsMessageAndCreateButton()
        {
           
            driver.Navigate().GoToUrl("https://localhost:7224/Volunteers");

            var message = driver.FindElement(By.TagName("p")).Text;
            Assert.IsTrue(message.Contains("You haven’t registered"));

            
            var createButton = driver.FindElement(By.CssSelector("a.btn-primary"));
            Assert.AreEqual("Become a Volunteer", createButton.Text);
        }

        [TestMethod]
        public void VolunteerIndex_WithRegistrations_ShowsTable()
        {
            
            driver.Navigate().GoToUrl("https://localhost:7224/Volunteers");

            
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => d.FindElements(By.CssSelector("table.table")).Any());

            var headers = driver.FindElements(By.CssSelector("table thead th")).Select(h => h.Text).ToList();
            CollectionAssert.AreEquivalent(new[] { "Task Type", "Availability" }, headers);

            
            var rows = driver.FindElements(By.CssSelector("table "));
            Assert.IsTrue(rows.Count > 0);

            var firstRowCells = rows[0].FindElements(By.TagName("td"));
            Assert.IsTrue(!string.IsNullOrEmpty(firstRowCells[0].Text));
            Assert.IsTrue(!string.IsNullOrEmpty(firstRowCells[1].Text));
        }
    }
}
