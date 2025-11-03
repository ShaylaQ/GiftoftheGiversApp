
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;


namespace GiftOfTheGivers.UITest
{
    [TestClass]
    public class Test1
    {
        protected IWebDriver driver;
        [TestMethod]
        public void TestMethod1()
        {

            var options = new ChromeOptions();

            if (Environment.GetEnvironmentVariable("AZURE_PIPELINES") == "true")
            {
                options.AddArgument("--headless");
                options.AddArgument("--disable-gpu");
            }

            driver = new ChromeDriver(options);
            driver.Manage().Window.Maximize();

        }

        [TestCleanup]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}
