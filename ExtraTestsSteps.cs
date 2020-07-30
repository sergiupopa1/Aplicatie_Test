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
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace ConsoleApp1
{
    [Binding, Scope(Feature = "extraTests")]
    public class ExtraTestsSteps
    {
        private readonly HomePage _homePage;

        private readonly CartPage _cartPage;

        private readonly IWebDriver _driver = new ChromeDriver();

        private const string Url = "https://www.demoblaze.com/";

        private const string OrderName = "John";

        private const string OrderCountry = "United States";

        private const string OrderCity = "New York";

        private const string OrderCard = "4000 5000 6000 7000";

        private const string OrderMonth = "April";

        private const string OrderYear = "2020";

        private int _budget;

        private string _productNameFromHomePage;

        private int _productPriceFromHomePage;

        public ExtraTestsSteps()
        {
            _homePage = new HomePage(_driver);

            _cartPage = new CartPage(_driver);

            _driver.Manage().Window.Maximize();
        }

        /******************************** Display Home Page *****************************/

        [Given(@"I am on the home page")]
        public void GivenIAmOnTheHomePage()
        {
            _homePage.NavigateToUrl(Url);

            _homePage.CheckHomePageIsDisplayed();//nu ai nevoie aici de verificarea asta
        }

        [When(@"I click Home button")]
        public void GivenIClickHomeButton()
        {
            _homePage.ClickHome();                                                                         
        }

        [Then(@"the home page is displayed")]
        public void ThenTheHomePageIsDisplayed()
        {
            _homePage.CheckHomePageIsDisplayed();//aici ar trebui facuta verificarea/assert-ul
        }

        /******************************** Go to Cart Page *****************************/

        [When(@"I go to cart")]
        public void WhenIGoToCart()
        {
            _homePage.ClickCart();
        }

        [Then(@"The cart page is displayed")]
        public void ThenTheCartPageIsDisplayed()
        {
            _cartPage.CheckCartPageIsDisplayed();
        }

        /******************************** Select first product *****************************/

        [When(@"I select the first product")]
        public void WhenISelectTheFirstProduct()
        {
            _productNameFromHomePage = _homePage.GetProductNameFromHomePage(1);

            _productPriceFromHomePage = _homePage.GetProductPriceFromHomePage(1);

            _homePage.ClickOnFirstProduct();
        }

        [Then(@"display product's price")]
        public void ThenDisplayProductSPrice()
        {
            var productPrice = _homePage.GetProductPrice();

            Console.WriteLine(productPrice);
        }

        /******************************** 4.Select first product and check cart *****************************/

        [Then(@"I add product to cart")]
        public void ThenIAddProductToCart()
        {
            _homePage.ClickAddToCart();

            _homePage.ProductAddedToCart();
        }

        [Then(@"The selected product is displayed with the correct price")]
        public void ThenTheSelectedProductIsDisplayedWithTheCorrectPrice()
        {
            var productNameFromCart = _cartPage.GetProductNameFromCartPage(1);

            var productPriceFromCart = _cartPage.GetProductPriceFromCartPage(1);

            Assert.AreEqual(_productNameFromHomePage, productNameFromCart);

            Assert.AreEqual(_productPriceFromHomePage, productPriceFromCart);
        }

        /******************************** Check all pages from the header *****************************/

        [When(@"I click on (.*)")]
        public void WhenIClickOn(string p0)
        {
            switch (p0)
            {
                case "Home":
                    _homePage.ClickHome();
                    break;
                case "Contact":
                    _homePage.ClickContact();
                    break;
                case "About us":
                    _homePage.ClickAboutUs();
                    break;
                case "Cart":
                    _homePage.ClickCart();
                    break;
                case "Log in":
                    _homePage.ClickLogIn();
                    break;
                case "Sign up":
                    _homePage.ClickSignUp();
                    break;
            }
        }

        [Then(@"I can see the correct (.*)")]
        public void ThenICanSeeTheCorrect(string p0)
        {
            switch (p0)
            {
                case "Home":
                    _homePage.CheckHomePageIsDisplayed();
                    break;
                case "Contact":
                    _homePage.CheckContactPageIsDisplayed();
                    break;
                case "About us":
                    _homePage.CheckAboutUsPageIsDisplayed();
                    break;
                case "Cart":
                    _cartPage.CheckCartPageIsDisplayed();
                    break;
                case "Log in":
                    _homePage.CheckLogInPageIsDisplayed();
                    break;
                case "Sign up":
                    _homePage.CheckSignUpPageIsDisplayed();
                    break;
            }
        }

        /******************************** Buy a Dell from 2017 *****************************/

        [When(@"I filter by (.*)")]
        public void WhenIFilterByLaptops(string p0)
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

        [When(@"I search for (.*)")]
        public void WhenISearchForDell(string p0)
        {
            _homePage.SelectProductByName(p0);
        }

        [When(@"I place the order")]
        public void WhenIPlaceTheOrder()
        {
            _homePage.ClickCart();

            Thread.Sleep(1000);

            _cartPage.ClickPlaceOrder();

            _cartPage.PlaceOrder(OrderName, OrderCountry, OrderCity, OrderCard, OrderMonth, OrderYear);
        }

        [Then(@"I get the order confirmation")]
        public void ThenIGetTheOrderConfirmation()
        {
            _cartPage.PurchaseConfirmation();
        }

        /******************************** Buy an Apple monitor *****************************/

        [Then(@"The cart is empty")]
        public void ThenTheCartIsEmpty()
        {
            _homePage.CheckHomePageIsDisplayed();

            _homePage.ClickCart();

            Thread.Sleep(1000);

            Assert.IsTrue(_cartPage.CheckCartIsEmpty());
        }

        /******************************** Buy products within budget *****************************/

        [Given(@"I have a budget of (.*)\$")]
        public void GivenIHaveABudgetOf(int p0)
        {
            _budget = p0;
        }

        [Given(@"I select a phone, a laptop and a monitor within budget")]
        public void GivenISelectAPhoneALaptopAndAMonitorWithinBudget()
        {
            _cartPage.AddToCartProductsWithinBudget(_budget);
        }

        [AfterScenario]
        public void ClosePage()
        {
            _driver.Quit();
        }
    }
}
