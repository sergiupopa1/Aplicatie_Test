using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Text.RegularExpressions;
using System.Threading;
using TechTalk.SpecFlow;

namespace ConsoleApp1
{
    [Binding, Scope(Feature = "extraTests")]
    public class ExtraTestsSteps
    {
        private IWebDriver driver;
        string productPrice;
        string productName;

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

        [When(@"I click on the first product")]
        public void WhenIClickOnTheFirstProduct()
        {
            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 15));

            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"tbodyid\"]/div[1]/div/a/img"))).Click();

            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//h2[@class='name']")));

            productName = driver.FindElement(By.XPath("//h2[@class='name']")).Text;

            productPrice = driver.FindElement(By.ClassName("price-container")).Text;
        }

        [Then(@"display product's price")]
        public void ThenDisplayProductSPrice()
        {
            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 15));

            wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("price-container")));

            productPrice = driver.FindElement(By.ClassName("price-container")).Text;

            Console.WriteLine(productPrice);
        }

        /******************************** new scenario *****************************/

        [When(@"I click on Add to Cart button")]
        public void WhenIClickOnAddToCartButton()
        {
            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 15));

            driver.FindElement(By.XPath("//*[@id=\"tbodyid\"]/div[2]/div/a")).Click();

            wait.Until(ExpectedConditions.AlertIsPresent());

            driver.SwitchTo().Alert().Accept();
        }

        [When(@"I click on Cart button")]
        public void WhenIClickOnCartButton()
        {
            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 15));

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("cartur"))).Click();
        }

        [Then(@"The selected product is displayed with the correct price")]
        public void ThenTheSelectedProductIsDisplayedWithTheCorrectPrice()
        {
            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 15));

            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"tbodyid\"]/tr/td[2]")));

            Assert.AreEqual(productName, (driver.FindElement(By.XPath("//*[@id=\"tbodyid\"]/tr/td[2]"))).Text);

            productPrice = (Regex.Match(productPrice, @"\d+").Value);

            Assert.AreEqual(productPrice, (driver.FindElement(By.XPath("//*[@id=\"tbodyid\"]/tr/td[3]"))).Text);

            (driver.FindElement(By.XPath("//*[contains(@onclick,'deleteItem')]"))).Click();
        }


        [AfterScenario]
        public void ClosePage()
        {
            driver.Quit();
        }
    }
}
