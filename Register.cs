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
    [Binding, Scope(Feature = "Register")] //structureaza si fisierele features si steps pe foldere
    public class Register//numeste si clasa asta RegisterSteps ca sa se stie pt ce e :D
    {
        private readonly HomePage _homePage;

        private readonly CartPage _cartPage;

        private readonly IWebDriver _driver = new ChromeDriver();
      
        private const string Url = "https://www.demoblaze.com/";

        private readonly string _randomUserName = Guid.NewGuid().ToString();

        private const string StandardUser = "xyz1234567890";

        private const string StandardPassword = "12345678";

        private string _initialImage;

        private int _budget;

        public Register()
        {
            _homePage = new HomePage(_driver);

            _cartPage = new CartPage(_driver);

            _driver.Manage().Window.Maximize();
        }

        /***************************************** Register new user ****************************************/

        [Given(@"I am on the homepage")]
        public void GivenIAmOnTheHomepage()
        {
            _homePage.NavigateToUrl(Url);

            _homePage.CheckHomePageIsDisplayed();
        }
        
        [Given(@"I click on Sign Up button")]
        public void GivenIClickOnSignUpButton()
        {
            _homePage.ClickSignUp();
        }
        
        [When(@"I fill in required data")]
        public void WhenIFillInRequiredData()
        {
            _homePage.PerformSignUp(_randomUserName, StandardPassword);
        }
        
        [Then(@"I get registered")]
        public void ThenIGetRegistered()
        {
            Assert.IsTrue(_homePage.RegisterConfirmation());

            _homePage.ClickLogIn();

            _homePage.PerformLogIn(_randomUserName, StandardPassword);
        }

        /***************************************** Log In ****************************************/

        [When(@"I click on the login button")]
        public void WhenIClickOnTheLoginButton()
        {
            _homePage.ClickLogIn();
        }

        [When(@"I enter my credentials")]
        public void WhenIEnterMyCredentials()
        {
            _homePage.PerformLogIn(StandardUser, StandardPassword);
        }

        [Then(@"I get logged in")]
        public void ThenIGetLoggedIn()
        {
            _homePage.LogInConfirmation();
        }

        /***************************************** Check that Image Slider change the content *****************************************/

        [When(@"I click on the Previous button from Image Slider")]
        public void WhenIClickOnThePreviousButtonFromImageSlider()
        {
            _initialImage = _homePage.GetActualImageFromSlider();

            _homePage.ClickPreviousButton();
        }

        [Then(@"I see a different product")]
        public void ThenISeeADifferentProduct()
        {            
            Assert.AreNotEqual(_homePage.GetActualImageFromSlider(), _initialImage);// verificarea se putea face luand sursa, imaginea incarcata ex: src="nexus1", "samsnug1" - done!
        }

        [When(@"I click on the Next button from Image Slider")]
        public void WhenIClickOnTheNextButtonFromImageSlider()
        {
            _initialImage = _homePage.GetActualImageFromSlider();

            _homePage.ClickNextButton();
        }

        /***************************************** Buy random phones using given budget ****************************************/

        [Given(@"I am logged in")]
        public void GivenIAmLoggedIn()
        {
            _homePage.NavigateToUrl(Url);

            _homePage.ClickLogIn();

            _homePage.PerformLogIn(StandardUser, StandardPassword);

            _homePage.LogInConfirmation();
        }

        [Given(@"I have a budget of (.*)\$")]
        public void GivenIHaveABudgetOf(int p0)
        {
            _budget = p0;
        }

        [Then(@"I can add to cart (.*) random phones that don't exceed my budget")]
        public void ThenICanAddToCartRandomPhonesThatDonTExceedMyBudget(int p0)
        {
            _homePage.AddToCartNoOfPhonesWithinBudget(p0, _budget);

            _cartPage.ClearCart();
        }

        /***************************************** Get mean value product cost ****************************************/

        [When(@"I filter by (.*)")]
        public void WhenIFilterBy(string p0)
        {
                switch (p0)
                {
                    case "Phones":
                        _homePage.ClickPhoneCategory();
                        break;
                    case "Laptops":
                        _homePage.ClickLaptopCategory();
                        break;
                    case "Monitors":
                        _homePage.ClickMonitorCategory();
                        break;
                    default:
                        Console.WriteLine("Error: No category found!");
                        break;
                }
        }

        [Then(@"I can see in the test output the mean value of each product")]
        public void ThenICanSeeInTheTestOutputTheMeanValueOfEachProduct()
        {
            Debug.WriteLine(_homePage.GetTheMeanValueOfProduct()); ////nu apare valoarea in output, foloseste Debug.WriteLine()

            Console.WriteLine(_homePage.GetTheMeanValueOfProduct()); // am adaugat Debug.WriteLine, dar nu apare in output
                                                                    // cu variante Console.WriteLine imi apare valoarea in "Open additional output for this result"
                                                                    // unde gresesc?
                                                                    //apare acum si cu Debug.
        }

        /***************************************** next scenario ****************************************/

        [AfterScenario]
        public void ClosePage()
        {
            _driver.Quit();
        }
    }
}
