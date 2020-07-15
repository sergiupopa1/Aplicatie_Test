using NUnit.Framework;
using NUnit.Framework.Constraints;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace ConsoleApp1.Pages
{

    class HomePage
    {
        private IWebDriver _driver;

        private WebDriverWait wait;

        public HomePage(IWebDriver driver)
        {
            _driver = driver;

            wait = new WebDriverWait(_driver, new TimeSpan(0, 0, 30));
        }

        private IWebElement _logo => wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("nava")));
        private IWebElement _signUpMenu => wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("signin2")));
        private IWebElement _signInUsername => wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("sign-username")));
        private IWebElement _signInPassword => wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("sign-password")));
        private IWebElement _signUpButton => wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[contains(text(),'Sign up')]")));
        private IWebElement _logInMenu => wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("login2")));
        private IWebElement _logInUserName => wait.Until(ExpectedConditions.ElementIsVisible(By.Id("loginusername")));
        private IWebElement _logInPassword => wait.Until(ExpectedConditions.ElementIsVisible(By.Id("loginpassword")));
        private IWebElement _logInButton => wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[contains(text(),'Log in')]")));
        private IWebElement _welcome => wait.Until(ExpectedConditions.ElementIsVisible(By.Id("nameofuser")));
        private IWebElement _imageSliderNextBtn => wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//span[@class='carousel-control-next-icon']")));
        private IWebElement _imageSliderPreviousBtn => wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//span[@class='carousel-control-prev-icon']")));
        private IWebElement _imageSlider => wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[@class='carousel-item active']/img")));
        private IWebElement _phoneCategory => wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//a[contains(text(),'Phones')]")));
        private IWebElement _laptopCategory => wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//a[contains(text(),'Laptops')]")));
        private IWebElement _monitorCategory => wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//a[contains(text(),'Monitors')]")));
        private IWebElement _addToCart => wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Add to cart")));
        private IWebElement _homeMenu => wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//a[contains(text(),'Home')]")));
        private IWebElement _cartMenu => wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("cartur")));
        private IWebElement _firstProduct => wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"tbodyid\"]/div[1]/div/a/img")));
        private IWebElement _productPrice => wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//h3[@class='price-container']")));
        private IWebElement _productsFromHomePage => wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[contains(@class, 'col-lg-4 col-md-6 mb-4')]")));

        public int CountNoOfProducts()
        {
            var waitForElements = _productsFromHomePage.Displayed;

            var noOfProducts = _driver.FindElements(By.XPath("//*[contains(@class, 'col-lg-4 col-md-6 mb-4')]")).Count; ;

            return noOfProducts;
        }

        public string GetProductPrice()
        {
            var productPrice = _productPrice.Text;

            return productPrice;
        }
        public void ClickOnFirstProduct()
        {
            _firstProduct.Click();
        }
        public void ClickCart()
        {
            _cartMenu.Click();
        }
        public void ClickHome()
        {
            _homeMenu.Click();
        }
        public void ClickSignUp()
        {
            _signUpMenu.Click();
        }
        public void ClickLogIn()
        {
            _logInMenu.Click();
        }
        public void ClickNextButton()
        {
            _imageSliderNextBtn.Click();
        }
        public void ClickPreviousButton()
        {
            _imageSliderPreviousBtn.Click();
        }
        public void ClickPhoneCategory()
        {
            _phoneCategory.Click();
        }
        public void ClickLaptopCategory()
        {
            _laptopCategory.Click();
        }
        public void ClickMonitorCategory()
        {
            _monitorCategory.Click();
        }
        public void ClickAddToCart()
        {
            _addToCart.Click();
        }
        public void NavigateToUrl(string URL)
        {
            _driver.Navigate().GoToUrl(URL);
        }
        public void CheckHomePageIsDisplayed()
        {
            Assert.IsTrue(_logo.Displayed);
        }
        public string PerformSignUp(string userName, string password)
        {
            _signInUsername.SendKeys(userName);

            _signInPassword.SendKeys(password);

            _signUpButton.Click();

            return userName;
        }
        public bool RegisterConfirmation()
        {
            wait.Until(ExpectedConditions.AlertIsPresent());

            var alertText = _driver.SwitchTo().Alert().Text;

            var result = alertText == "Sign up successful." ? true : false;

            _driver.SwitchTo().Alert().Accept();

            return result;
        }
        public void PerformLogIn(string userName, string password)
        {
            _logInUserName.SendKeys(userName);

            _logInPassword.SendKeys(password);

            _logInButton.Click();
        }
        public void LogInConfirmation()
        {
            Assert.IsTrue(_welcome.Displayed);
        }
        public bool ProductAddedToCart()
        {
            wait.Until(ExpectedConditions.AlertIsPresent());

            var alertText = _driver.SwitchTo().Alert().Text;
            
            var result = alertText == "Product added" ? true : false;

            _driver.SwitchTo().Alert().Accept();

            return result;
        }
        public string GetActualImageFromSlider()
        {
            var actualImage = _imageSlider.GetAttribute("src");

            return actualImage;
        }
        public string GetProductNameFromHomePage(int productPositionFromPage)
        {
            IWebElement Name = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@class='col-lg-9']//div[" + productPositionFromPage + "]//div[1]//div[1]//h4[1]//a")));
            
            var productName = Name.Text;

            return productName;
        }
        public int GetProductPriceFromHomePage(int productPositionFromPage)
        {
            IWebElement Price = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@class='col-lg-9']//div[" + productPositionFromPage + "]//div[1]//div[1]//h5[1]")));

            var productPrice = int.Parse(Regex.Match(Price.Text, @"\d+").Value);
            
            return productPrice;
        }
    }
}
