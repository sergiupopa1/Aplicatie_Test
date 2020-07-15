using ConsoleApp1.Pages;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;
using TechTalk.SpecFlow;
using TestProject.SDK;
using TestProject.SDK.Tests;
using TestProject.SDK.Tests.Helpers;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace ConsoleApp1
{
    [Binding, Scope(Feature = "Register")]
    public class Register
    {
        private HomePage homePage;

        private IWebDriver driver = new ChromeDriver();
      
        private string _URL = "https://www.demoblaze.com/";

        private string _randomUserName = Guid.NewGuid().ToString();

        private string _standardUser = "xyz1234567890";

        private string _standardPassword = "12345678";

        private string _initialImage;

        private int _meanValue;

        private int _budget;

        public Register()
        {
            homePage = new HomePage(driver);

            driver.Manage().Window.Maximize();
        }

        /***************************************** Register new user ****************************************/

        [Given(@"I am on the homepage")]
        public void GivenIAmOnTheHomepage()
        {
            homePage.NavigateToUrl(_URL);

            homePage.CheckHomePageIsDisplayed();
        }
        
        [Given(@"I click on Sign Up button")]
        public void GivenIClickOnSignUpButton()
        {
            homePage.ClickSignUp();
        }
        
        [When(@"I fill in required data")]
        public void WhenIFillInRequiredData()
        {
            homePage.PerformSignUp(_randomUserName, _standardPassword);
        }
        
        [Then(@"I get registered")]
        public void ThenIGetRegistered()
        {
            homePage.RegisterConfirmation();

            homePage.ClickLogIn();

            homePage.PerformLogIn(_randomUserName, _standardPassword);
        }

        /***************************************** Log In ****************************************/

        [When(@"I click on the login button")]
        public void WhenIClickOnTheLoginButton()
        {
            homePage.ClickLogIn();
        }

        [When(@"I enter my credentials")]
        public void WhenIEnterMyCredentials()
        {
            homePage.PerformLogIn(_standardUser, _standardPassword);
        }

        [Then(@"I get logged in")]
        public void ThenIGetLoggedIn()
        {
            homePage.LogInConfirmation();
        }

        /***************************************** Check that Image Slider change the content *****************************************/

        public string GetImageDisplayedInSlider()
        {
            _initialImage = homePage.GetActualImageFromSlider();
    
            return _initialImage;
        }

        [When(@"I click on the Previous button from Image Slider")]
        public void WhenIClickOnThePreviousButtonFromImageSlider()
        {
            GetImageDisplayedInSlider();

            homePage.ClickPreviousButton();

            Thread.Sleep(1000);
        }

        [Then(@"I see a different product")]
        public void ThenISeeADifferentProduct()
        {            
            var actualImage = homePage.GetActualImageFromSlider();

            Assert.AreNotEqual(actualImage, _initialImage);// verificarea se putea face luand sursa, imaginea incarcata ex: src="nexus1", "samsnug1" - done!
        }

        [When(@"I click on the Next button from Image Slider")]
        public void WhenIClickOnTheNextButtonFromImageSlider()
        {
            GetImageDisplayedInSlider();

            homePage.ClickNextButton();

            Thread.Sleep(1000);
        }

        /***************************************** Buy random phones using given budget ****************************************/

        [Given(@"I am logged in")]
        public void GivenIAmLoggedIn()
        {
            homePage.NavigateToUrl(_URL);

            homePage.ClickLogIn();

            homePage.PerformLogIn(_standardUser, _standardPassword);
        }

        [Given(@"I have a budget of (.*)\$")]
        public void GivenIHaveABudgetOf(int p0)
        {
            _budget = p0;
        }

        [Then(@"I can add to cart (.*) random phones that don't exceed my budget")]
        public void ThenICanAddToCartRandomPhonesThatDonTExceedMyBudget(int p0)  // Incearca sa spargi in metode codul din acest Step
                                                                                 // hint: poti lua toate telefoanele si preturile lor intr-o lista de elemente
                                                                                 // vezi foreach pentru C#, care te poate ajuta sa simplifici codul
        {
            int j, phoneValue1, phoneValue2, totalCart, Contor;
            
            string xPathSelectedPhone1, xPathSelectedPhone2;

            Random rand = new Random();

            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 30));

            Contor = driver.FindElements(By.XPath("//*[contains(@class, 'col-lg-4 col-md-6 mb-4')]")).Count;

            j = rand.Next(Contor);
            if (j == 0)
                j++;

            phoneValue1 = int.Parse(Regex.Match((driver.FindElement(By.XPath("//*[@id=\"tbodyid\"]/div[" + j + "]/div/div/h5"))).Text, @"\d+").Value);

            if (_budget - phoneValue1 > 0)
            {
                xPathSelectedPhone1 = "//*[@id=\"tbodyid\"]/div[" + j + "]/div/div/h4/a";
            }

            else
            {
                j = rand.Next(Contor);
                if (j == 0)
                    j++;

                phoneValue1 = int.Parse(Regex.Match((driver.FindElement(By.XPath("//*[@id=\"tbodyid\"]/div[" + j + "]/div/div/h5"))).Text, @"\d+").Value);

                xPathSelectedPhone1 = "//*[@id=\"tbodyid\"]/div[" + j + "]/div/div/h4/a";
            }

            do
            {
                j = rand.Next(Contor);
                if (j == 0)
                    j++;

                phoneValue2 = int.Parse(Regex.Match((driver.FindElement(By.XPath("//*[@id=\"tbodyid\"]/div[" + j + "]/div/div/h5"))).Text, @"\d+").Value);

            } while (phoneValue1 + phoneValue2 > _budget);

            xPathSelectedPhone2 = "//*[@id=\"tbodyid\"]/div[" + j + "]/div/div/h4/a";

            driver.FindElement(By.XPath(xPathSelectedPhone1)).Click();

            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"tbodyid\"]/div[2]/div/a"))).Click();

            wait.Until(ExpectedConditions.AlertIsPresent());

            driver.SwitchTo().Alert().Accept();

            driver.FindElement(By.Id("nava")).Click();

            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xPathSelectedPhone2))).Click();

            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"tbodyid\"]/div[2]/div/a"))).Click();

            wait.Until(ExpectedConditions.AlertIsPresent());

            driver.SwitchTo().Alert().Accept();

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("cartur"))).Click();

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("totalp")));

            totalCart = int.Parse(driver.FindElement(By.Id("totalp")).Text);

            Assert.LessOrEqual(totalCart, _budget);

            for (j=0;j<p0;j++)
            {
                wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//a[contains(text(),'Delete')]")));

                (driver.FindElement(By.XPath("//*[contains(@onclick,'deleteItem')]"))).Click();

                Thread.Sleep(1000);
            }

            Thread.Sleep(2000);
        }

        /***************************************** Get mean value product cost ****************************************/

        [When(@"I filter by (.*)")]
        public void WhenIFilterBy(string p0)
        {
                switch (p0)
                {
                    case "Phones":
                        homePage.ClickPhoneCategory();
                        break;
                    case "\"Phones\"":
                        homePage.ClickPhoneCategory();
                        break;
                    case "\"Laptops\"":
                        homePage.ClickLaptopCategory();
                        break;
                    case "\"Monitors\"":
                        homePage.ClickMonitorCategory();
                        break;
                    default:
                        Console.WriteLine("Error: No category found!");
                        break;
                }
        }

        [Then(@"I can see in the test output the mean value of each product")]
        public void ThenICanSeeInTheTestOutputTheMeanValueOfEachProduct()
        {
            int noOfProducts;
            int i; // e indicat sa se foloseasca declarari separate pentru variabile, e mai usor de citit, urmarit - Done!

            noOfProducts = homePage.CountNoOfProducts();

            for (i=1;i<=noOfProducts;i++)
            {
                _meanValue += int.Parse(Regex.Match((driver.FindElement(By.XPath("//*[@id=\"tbodyid\"]/div[" + i + "]/div/div/h5"))).Text, @"\d+").Value);
            }
            
            //nu apare valoarea in output, foloseste Debug.WriteLine() - Done!
            Debug.WriteLine(_meanValue / noOfProducts);
        }

        [AfterScenario]
        public void ClosePage()
        {
            driver.Quit();
        }
    }
}
