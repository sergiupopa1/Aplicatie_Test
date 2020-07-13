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
            driver = new ChromeDriver(); // 1

            // Modificare

            driver.Manage().Window.Maximize(); // 2


            driver.Navigate().GoToUrl("https://www.demoblaze.com"); // 3
                                                                    // Linile comentate cu 1,2 si 3 pot fi extrase intr-o metoda si apoi apelata acolo unde este nevoie
                                                                    // De studiat Pattern-ul Page Object model

            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 15));// La fel si aceasta linie poate fi scoasa afara din metodele
                                                                                                          // de tip Steps, inafara lor si utilizat doar obiectul acolo unde ai nevoie
                                                                                                          // fara a crea unul nou de fiecare data

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("nava")));
        }

        [When(@"I click Home button")]
        public void GivenIClickHomeButton()
        {
            (driver.FindElement(By.XPath("//*[@id=\"navbarExample\"]/ul/li[1]/a"))).Click(); // Este indicat ca identificarea elementelor sa nu se faca in Steps
                                                                                             // sau in anumite metode ci inafara lor sau clase spefice paginilor din care fac parte
                                                                                             // conform pattern-ului Page Object Model
                                                                                             
        }

        [Then(@"the home page is displayed")]
        public void ThenTheHomePageIsDisplayed()
        {
            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 15)); // la fel. Vezi mai sus.

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("nava")));
        }

        /******************************** new scenario *****************************/

        [When(@"I click Cart button")]
        public void WhenIClickCartButton()
        {
            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 15)); // // la fel. Vezi mai sus.

            (wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("cartur")))).Click(); // la fel. Vezi mai sus.
        }

        [Then(@"The cart page is displayed")]
        public void ThenTheCartPageIsDisplayed()
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 15)); // la fel. Vezi mai sus.

                wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[@class='btn btn-success']"))); // la fel. Vezi mai sus.
            }

            catch (NoSuchElementException)
            {
                Console.WriteLine("Element was not found!"); // Folosirea try catch in acest context(identificarea elementelor) nu e necesara
            }
        }

        /******************************** new scenario *****************************/

        [When(@"I click on the first product")]
        public void WhenIClickOnTheFirstProduct()
        {
            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 15));

            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"tbodyid\"]/div[1]/div/a/img"))).Click(); // aici ar fi de folos folosirea try catch, in momentul in care actionezi asupra elementelor

            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//h2[@class='name']")));

            productName = driver.FindElement(By.XPath("//h2[@class='name']")).Text; // la fel, identificarea elementelor poate fi mutata inafara metodelor Steps

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

            (driver.FindElement(By.XPath("//*[contains(@onclick,'deleteItem')]"))).Click(); // In pasul de verificare ar trebui facuta doar verificarea, partea de Assert
                                                                                            // Partea de delete item aici nu e necesara
        }


        [AfterScenario]
        public void ClosePage()
        {
            driver.Quit();
        }
    }
}
