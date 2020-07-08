using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Threading;
using TechTalk.SpecFlow;

namespace ConsoleApp1
{
    [Binding, Scope(Feature = "extraTests")]
    public class ExtraTestsSteps
    {
        private IWebDriver driver;

        [Given(@"I am on the home page")]
        public void GivenIAmOnTheHomePage()
        {
            driver = new ChromeDriver();

            driver.Navigate().GoToUrl("https://www.demoblaze.com");

            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 15));

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("nava")));
        }

        [When(@"I click Home button")]
        public void GivenIClickHomeButton()
        {
            (driver.FindElement(By.XPath("//*[@id=\"navbarExample\"]/ul/li[1]/a"))).Click();
        }

        [Then(@"the home page is displayed")]
        public void ThenTheHomePageIsDisplayed()
        {
            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 15));

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("nava")));
        }

        /******************************** new scenario *****************************/

        [When(@"I click Cart button")]
        public void WhenIClickCartButton()
        {
            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 15));

            (wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("cartur")))).Click();
        }

        [Then(@"The cart page is displayed")]
        public void ThenTheCartPageIsDisplayed()
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 15));

                wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[@class='btn btn-success']")));
            }

            catch (NoSuchElementException)
            {
                Console.WriteLine("Element was not found!");
            }
        }

        /******************************** new scenario *****************************/

        [AfterScenario]
        public void ClosePage()
        {
            driver.Quit();
        }
    }
}
