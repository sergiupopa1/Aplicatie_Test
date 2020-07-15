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
        private HomePage homePage;

        private CartPage cartPage;

        private IWebDriver driver = new ChromeDriver();

        private string _URL = "https://www.demoblaze.com/";

        private string _productNameFromHomePage;

        private int _productPriceFromHomePage;

        public ExtraTestsSteps()
        {
            homePage = new HomePage(driver);

            cartPage = new CartPage(driver);

            driver.Manage().Window.Maximize();
        }

        /******************************** Display Home Page *****************************/

        [Given(@"I am on the home page")]
        public void GivenIAmOnTheHomePage()
        {
            homePage.NavigateToUrl(_URL);

            homePage.CheckHomePageIsDisplayed();
        }

        [When(@"I click Home button")]
        public void GivenIClickHomeButton()
        {
            homePage.ClickHome();                                                                         
        }

        [Then(@"the home page is displayed")]
        public void ThenTheHomePageIsDisplayed()
        {
            homePage.CheckHomePageIsDisplayed();
        }

        /******************************** Go to Cart Page *****************************/

        [When(@"I click Cart button")]
        public void WhenIClickCartButton()
        {
            homePage.ClickCart();
        }

        [Then(@"The cart page is displayed")]
        public void ThenTheCartPageIsDisplayed()
        {
            cartPage.CheckCartPageIsDisplayed();
        }

        /******************************** Select first product *****************************/

        [When(@"I click on the first product")]
        public void WhenIClickOnTheFirstProduct()
        {
            _productNameFromHomePage = homePage.GetProductNameFromHomePage(1);

            _productPriceFromHomePage = homePage.GetProductPriceFromHomePage(1);

            homePage.ClickOnFirstProduct();
        }

        [Then(@"display product's price")]
        public void ThenDisplayProductSPrice()
        {
            var productPrice = homePage.GetProductPrice();

            Console.WriteLine(productPrice);
        }

        /******************************** Select first product *****************************/

        [When(@"I click on Add to Cart button")]
        public void WhenIClickOnAddToCartButton()
        {
            homePage.ClickAddToCart();

            homePage.ProductAddedToCart();
        }

        [When(@"I click on Cart button")]
        public void WhenIClickOnCartButton()
        {
            homePage.ClickCart();
        }

        [Then(@"The selected product is displayed with the correct price")]
        public void ThenTheSelectedProductIsDisplayedWithTheCorrectPrice()
        {
            var productNameFromCart = cartPage.GetProductNameFromCartPage(1);

            var productPriceFromCart = cartPage.GetProductPriceFromCartPage(1);

            Assert.AreEqual(_productNameFromHomePage, productNameFromCart);

            Assert.AreEqual(_productPriceFromHomePage, productPriceFromCart);
        }

        [AfterScenario]
        public void ClosePage()
        {
            driver.Quit();
        }
    }
}
