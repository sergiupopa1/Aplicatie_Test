using NUnit.Framework;
using NUnit.Framework.Constraints;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Telerik.JustMock.Helpers;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace ConsoleApp1.Pages
{

    class HomePage
    {
        private readonly IWebDriver _driver;

        private readonly WebDriverWait _wait;

        private readonly CartPage _cartPage;

        public HomePage(IWebDriver driver)
        {
            _driver = driver;

            _wait = new WebDriverWait(_driver, new TimeSpan(0, 0, 30));

            _cartPage = new CartPage(_driver);
        }

        private IWebElement _contactPageIdentification => _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("recipient-email")));
        private IWebElement _aboutUsPageIdentification => _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("videoModalLabel")));
        private IWebElement _category => _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("cat")));
        private IWebElement _homeMenu => _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//a[contains(text(),'Home')]")));
        private IWebElement _contactMenu => _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//a[contains(text(),'Contact')]")));
        private IWebElement _aboutUsMenu => _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//a[contains(text(),'About us')]")));
        private IWebElement _cartMenu => _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("cartur")));
        private IWebElement _logInMenu => _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("login2")));
        private IWebElement _signUpMenu => _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("signin2")));
        private IWebElement _signInUsername => _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("sign-username")));
        private IWebElement _signInPassword => _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("sign-password")));
        private IWebElement _signUpButton => _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[contains(text(),'Sign up')]")));
        private IWebElement _logInUserName => _wait.Until(ExpectedConditions.ElementIsVisible(By.Id("loginusername")));
        private IWebElement _logInPassword => _wait.Until(ExpectedConditions.ElementIsVisible(By.Id("loginpassword")));
        private IWebElement _logInButton => _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[contains(text(),'Log in')]")));
        private IWebElement _welcome => _wait.Until(ExpectedConditions.ElementIsVisible(By.Id("nameofuser")));
        private IWebElement _imageSliderNextBtn => _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//span[@class='carousel-control-next-icon']")));
        private IWebElement _imageSliderPreviousBtn => _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//span[@class='carousel-control-prev-icon']")));
        private IWebElement _imageSlider => _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[@class='carousel-item active']/img")));
        private IWebElement _phoneCategory => _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[contains(@onclick,'phone')]")));
        private IWebElement _laptopCategory => _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[contains(@onclick,'notebook')]")));
        private IWebElement _monitorCategory => _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[contains(@onclick,'monitor')]")));
        private IWebElement _addToCart => _wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Add to cart")));
        private IWebElement _firstProduct => _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"tbodyid\"]/div[1]/div/a/img")));
        private IWebElement _productPrice => _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//h3[@class='price-container']")));

        public void CheckContactPageIsDisplayed()
        {
            Assert.IsTrue(_contactPageIdentification.Displayed);
        }
        public void CheckAboutUsPageIsDisplayed()
        {
            Assert.AreEqual(_aboutUsPageIdentification.Text, "About us");
        }
        public void CheckLogInPageIsDisplayed()
        {
            Assert.IsTrue(_logInUserName.Displayed);
        }
        public void CheckSignUpPageIsDisplayed()
        {
            Assert.IsTrue(_signInUsername.Displayed);
        }
        public void ClickAboutUs()
        {
            _aboutUsMenu.Click();
        }
        public void ClickContact()
        {
            _contactMenu.Click();
        }
        public void WaitForElementContainText(string elementContainText)
        {
            _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[contains(text(),'" + elementContainText + "')]")));
        }
        public void WaitForProducts()
        {
            var counter = CountNoOfProducts();

            for(var i = 1; i <= counter; i++)
            {
                _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//body/div/div/div/div/div["+ i +"]")));
            }
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
            Assert.IsTrue(_category.Displayed);
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
            _wait.Until(ExpectedConditions.AlertIsPresent());

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
        public void SelectProductByName(string nameOfProduct)
        {
            try
            {
                (_wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//a[contains(text(),'" + nameOfProduct + "')]")))).Click();
            }
            catch
            {
                _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("next2")));

                (_wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//a[contains(text(),'" + nameOfProduct + "')]")))).Click();
            }
        }
        public bool ProductAddedToCart()
        {
            _wait.Until(ExpectedConditions.AlertIsPresent());

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
            IWebElement Name = _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@class='col-lg-9']//div[" + productPositionFromPage + "]//div[1]//div[1]//h4[1]//a")));
            
            var productName = Name.Text;

            return productName;
        }
        public int GetProductPriceFromHomePage(int productPositionFromPage)
        {
            IWebElement Price = _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@class='col-lg-9']//div[" + productPositionFromPage + "]//div[1]//div[1]//h5[1]")));

            var productPrice = int.Parse(Regex.Match(Price.Text, @"\d+").Value);
            
            return productPrice;
        }
        public int CountNoOfProducts()
        {
            _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("//*[contains(@class, 'col-lg-4 col-md-6 mb-4')]")));

            var noOfProducts = _driver.FindElements(By.XPath("//*[contains(@class, 'col-lg-4 col-md-6 mb-4')]")).Count;

            return noOfProducts;
        }
        public int GetTheMeanValueOfProduct()
        {
            var totalValue = 0;

            var noOfProducts = CountNoOfProducts();

            for (var i = 1; i <= noOfProducts; i++)
            {
                totalValue += int.Parse(Regex.Match((_driver.FindElement(By.XPath("//*[@id=\"tbodyid\"]/div[" + i + "]/div/div/h5"))).Text, @"\d+").Value);
            }

            var meanValue = totalValue / noOfProducts;

            return meanValue;
        }        
        public void AddToCartNoOfPhonesWithinBudget(int noOfPhones, int budget)
        {
            Random rand = new Random();

            ClickPhoneCategory();

            Thread.Sleep(500);

            var counter = CountNoOfProducts();

            int[] allPhonesPrice = new int[counter];

            string[] allPhonesName = new string[counter];

            string[] phoneNamesToAddToCart = new string[noOfPhones];

            int[] phonePricesToAddToCart = new int[noOfPhones];

            //save all products with name and price in array
            for (var i = 1; i <= counter; i++)
            {
                allPhonesName[i-1] = _driver.FindElement(By.XPath("//div[@id='tbodyid']//div[" + i + "]//div[1]//div[1]//h4[1]//a[1]")).Text;

                allPhonesPrice[i-1] = int.Parse(Regex.Match((_driver.FindElement(By.XPath("//*[@id=\"tbodyid\"]/div[" + i + "]/div/div/h5"))).Text, @"\d+").Value);
            }

            //
            for (var i = 0; i < counter; i++)
            {
                var sum = 0;

                //get random products from array
                for (var j = 0; j < noOfPhones; j++)
                {
                    var random = rand.Next(counter);
                    phonePricesToAddToCart[j] = allPhonesPrice[random];
                    phoneNamesToAddToCart[j] = allPhonesName[random];
                }
                //get products total value
                for (var k = 0; k < noOfPhones; k++)
                {
                    sum += phonePricesToAddToCart[k];
                }
                //check if products total value is within budget
                if (sum <= budget)
                    break;
            }
            //add selected products to cart
            for(var i = 0; i < noOfPhones; i++)
            {
                SelectProductByName(phoneNamesToAddToCart[i]);

                ClickAddToCart();

                ProductAddedToCart();

                ClickHome();
            }

            Thread.Sleep(1000);

            ClickCart();

            _cartPage.CheckCartPageIsDisplayed();

            try
            {
                Assert.GreaterOrEqual(budget, _cartPage.GetTotalPriceFromCart());
            }
            catch (AssertionException)
            {
                _cartPage.ClearCart();
                throw;
            }
        }
    }
}