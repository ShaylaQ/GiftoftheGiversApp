using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;

namespace GiftOfTheGivers.UITest.TestsforViews
{
    [TestClass]
    public class IncidentViewTests : Test1
    {
        [TestMethod]
        public void IncidentIndex_NoReports_ShowsMessage()
        {
            driver.Navigate().GoToUrl("https://localhost:7224/Incidents");

            var bodyText = driver.FindElement(By.TagName("body")).Text;

            Assert.IsTrue(
                bodyText.Contains("No incident reports submitted.") || bodyText.Contains("Report New Incident"),
                "Expected 'No incident reports submitted yet.' message or the Create button."
            );
        }

        [TestMethod]
        public void IncidentIndex_WithReports_DisplaysTable()
        {
            driver.Navigate().GoToUrl("https://localhost:7224/Incidents");

            
            var tables = driver.FindElements(By.CssSelector("table.table-bordered"));
            Assert.IsTrue(tables.Count > 0, "incident reports to be displayed.");

            var rows = tables.First().FindElements(By.CssSelector("tbody tr"));
            Assert.IsTrue(rows.Count > 0, "Expected at least one incident report in the table.");
        }

        [TestMethod]
        public void IncidentIndex_CreateButton_NavigatesToCreatePage()
        {
            driver.Navigate().GoToUrl("https://localhost:7224/Incidents");

            var createButton = driver.FindElement(By.CssSelector("a.btn-primary"));
            createButton.Click();

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => d.Url.Contains("/Incidents/Create"));

            Assert.IsTrue(driver.Url.Contains("/Incidents/Create"), "navigate to Incident Create page.");
        }
    }
}
