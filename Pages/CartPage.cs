using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using TechTalk.SpecFlow.CommonModels;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace ConsoleApp1.Pages
{
    class CartPage
    {
        private readonly IWebDriver _driver;

        private readonly WebDriverWait _wait;

        private HomePage _homePage;
        public CartPage(IWebDriver driver)
        {
            _driver = driver;

            _wait = new WebDriverWait(_driver, new TimeSpan(0, 0, 30));            
        }

        private IWebElement _cartPageIdentification => _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//h2[contains(text(),'Total')]")));
        private IWebElement _totalPriceFromCart => _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//h3[@id='totalp']")));
        private IWebElement _cartMenu => _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//a[contains(text(),'Cart')]")));
        private IWebElement _placeOrder => _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[@class='btn btn-success']")));
        private IWebElement _orderName => _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("name")));
        private IWebElement _orderCountry => _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("country")));
        private IWebElement _orderCity => _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("city")));
        private IWebElement _orderCard => _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("card")));
        private IWebElement _orderMonth => _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("month")));
        private IWebElement _orderYear => _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("year")));
        private IWebElement _purchase => _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[contains(text(),'Purchase')]")));
        private IWebElement _purchaseConfirmationMessage => _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//h2[contains(text(),'Thank you for your purchase!')]")));
        private IWebElement _okBtn => _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[contains(text(),'OK')]")));

        public void ClickOkButton()
        {
            _okBtn.Click();
        }
        public bool PurchaseConfirmation()
        {
            var element = _purchaseConfirmationMessage.Displayed;

            var alertText = _driver.FindElement(By.XPath("//h2[contains(text(),'Thank you for your purchase!')]")).Text;//poti defini si acest element sus ca restul

            var result = alertText == "Thank you for your purchase!" ? true : false;

            Thread.Sleep(1000);

            ClickOkButton();

            return result;
        }
        public void PlaceOrder(string name, string country, string city, string card, string month, string year)
        {
            _orderName.SendKeys(name);

            _orderCountry.SendKeys(country);

            _orderCity.SendKeys(city);

            _orderCard.SendKeys(card);

            _orderMonth.SendKeys(month);

            _orderYear.SendKeys(year);

            ClickPurchase();
        }
        public void ClickPurchase()
        {
            _purchase.Click();
        }
        public void ClickPlaceOrder()
        {
            _placeOrder.Click();
        }
        public void CheckCartPageIsDisplayed()
        {
            Assert.IsTrue(_cartPageIdentification.Displayed);
        }
        public string GetProductNameFromCartPage(int productPositionFromPage)
        {
            IWebElement name = _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//tbody[@id='tbodyid']//tr[" + productPositionFromPage + "]//td[2]")));

            var productName = name.Text;

            return productName;
        }
        public int GetProductPriceFromCartPage(int productPositionFromPage)
        {
            IWebElement price = _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//tbody[@id='tbodyid']//tr[" + productPositionFromPage + "]//td[3]")));

            var productPrice = int.Parse(Regex.Match(price.Text, @"\d+").Value);
            
            return productPrice;
        }
        public int GetTotalPriceFromCart()
        {
            var total = int.Parse(Regex.Match(_totalPriceFromCart.Text, @"\d+").Value);
            return total;
        }
        public void ClearCart()
        {
            var counter = 0;

            _cartMenu.Click();

            try
            {
                _wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("success")));//si acestea cred ca le poti declara sus in clasa si apoi doar le folosesti aici ca variabile

                counter = (_driver.FindElements(By.ClassName("success"))).Count;
            }
            catch
            {
                Console.WriteLine("Cart is empty!");
            }
            
            if(counter > 0)
                do
                {
                    try
                    {
                        _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//tr[1]//td[4]//a[1]")));

                        (_driver.FindElement(By.XPath("//tr[1]//td[4]//a[1]"))).Click();

                        Thread.Sleep(1000);
                    }
                    catch
                    {
                        Console.WriteLine("Cart is empty!");
                    }

                    counter--;

                } while (counter > 0);  
        }
        public bool CheckCartIsEmpty()
        {
            var result = false;
            try
            {
                _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[@class='btn btn-success']")));

                _driver.FindElement(By.ClassName("success"));
            }
            catch(NoSuchElementException)
            {
                result = true;
            }

            return result;        
        }
        public void AddToCartProductsWithinBudget(int budget)//incearca sa sparti metoda asta in mai multe ca e greu de urmarit si in caz de schimbari pe UI
        //o sa fie greu si la mentenanta
        {
            _homePage = new HomePage(_driver);

            var rand = new Random();

            _homePage.ClickPhoneCategory();
            Thread.Sleep(500);
            _homePage.WaitForProducts();

            var counter = _homePage.CountNoOfProducts();

            string[] phoneNameList = new string[counter];
            int[] phonePriceList = new int[counter];

            for (var i = 1; i <= counter; i++)
            {
                phoneNameList[i-1] = _driver.FindElement(By.XPath("//div[@id='tbodyid']//div[" + i + "]//div[1]//div[1]//h4[1]//a[1]")).Text;//la fel si acestea
                phonePriceList[i - 1] = int.Parse(Regex.Match((_driver.FindElement(By.XPath("//*[@id=\"tbodyid\"]/div[" + i + "]/div/div/h5"))).Text, @"\d+").Value);
            }

            _homePage.ClickLaptopCategory();
            Thread.Sleep(500);
            _homePage.WaitForProducts();

            counter = _homePage.CountNoOfProducts();

            string[] laptopNameList = new string[counter];
            int[] laptopPriceList = new int[counter];

            for (var i = 1; i <= counter; i++)
            {
                laptopNameList[i - 1] = _driver.FindElement(By.XPath("//div[@id='tbodyid']//div[" + i + "]//div[1]//div[1]//h4[1]//a[1]")).Text;
                laptopPriceList[i - 1] = int.Parse(Regex.Match((_driver.FindElement(By.XPath("//*[@id=\"tbodyid\"]/div[" + i + "]/div/div/h5"))).Text, @"\d+").Value);
            }

            _homePage.ClickMonitorCategory();
            Thread.Sleep(500);
            _homePage.WaitForProducts();

            counter = _homePage.CountNoOfProducts();

            string[] monitorNameList = new string[counter];
            int[] monitorPriceList = new int[counter];

            for (var i = 1; i <= counter; i++)
            {
                monitorNameList[i - 1] = _driver.FindElement(By.XPath("//div[@id='tbodyid']//div[" + i + "]//div[1]//div[1]//h4[1]//a[1]")).Text;
                monitorPriceList[i - 1] = int.Parse(Regex.Match((_driver.FindElement(By.XPath("//*[@id=\"tbodyid\"]/div[" + i + "]/div/div/h5"))).Text, @"\d+").Value);
            }

            var phoneNameToAddToCart = "";
            var laptopNameToAddToCart = "";
            var monitorNameToAddToCart = "";
            
            for (var i = 0; i < phoneNameList.Length; i++)//acest for ar trebui refactorizat ca sa arate mai bine si sa fie mai usor de urmarit :)
            {
                var random = rand.Next(phoneNameList.Length);

                phoneNameToAddToCart = phoneNameList[random];
                var phonePriceToAddToCart = phonePriceList[random];

                random = rand.Next(laptopNameList.Length);

                laptopNameToAddToCart = laptopNameList[random];
                var laptopPriceToAddToCart = laptopPriceList[random];

                random = rand.Next(monitorNameList.Length);

                monitorNameToAddToCart = monitorNameList[random];
                var monitorPriceToAddToCart = monitorPriceList[random];

                var sum = phonePriceToAddToCart + laptopPriceToAddToCart + monitorPriceToAddToCart;

                if (sum <= budget)
                    break;
            }


            //pentru metodele din homepage pe care vrei sa le folosesti si in alte page-uri, foloseste-te de mostenire a claselor (cartpage sa mosteneasca homepage)
            //si o sa fie mai usor sa le apelezi apoi aici
            _homePage.ClickPhoneCategory();
            Thread.Sleep(1000);//sleep-urile, incearca sa scapi de ele :)
            _homePage.WaitForProducts();
            _homePage.WaitForElementContainText(phoneNameToAddToCart);
            _homePage.SelectProductByName(phoneNameToAddToCart);
            _homePage.ClickAddToCart();
            _homePage.ProductAddedToCart();
            _homePage.ClickHome();
            _homePage.CheckHomePageIsDisplayed();

            _homePage.ClickLaptopCategory();
            Thread.Sleep(1000);
            _homePage.WaitForProducts();
            _homePage.WaitForElementContainText(laptopNameToAddToCart);
            _homePage.SelectProductByName(laptopNameToAddToCart);
            _homePage.ClickAddToCart();
            _homePage.ProductAddedToCart();
            _homePage.ClickHome();
            _homePage.CheckHomePageIsDisplayed();

            _homePage.ClickMonitorCategory();
            Thread.Sleep(1000);
            _homePage.WaitForProducts();
            _homePage.WaitForElementContainText(monitorNameToAddToCart);
            _homePage.SelectProductByName(monitorNameToAddToCart);
            _homePage.ClickAddToCart();
            _homePage.ProductAddedToCart();
            _homePage.ClickHome();
            _homePage.CheckHomePageIsDisplayed();

            Thread.Sleep(1000);

            _homePage.ClickCart();

            CheckCartPageIsDisplayed();

            try
            {
                Assert.GreaterOrEqual(budget, GetTotalPriceFromCart());
            }
            catch (AssertionException)
            {
                ClearCart();
                throw;
            }

            _homePage.ClickHome();
            _homePage.CheckHomePageIsDisplayed(); Thread.Sleep(1000);
        }
    }
}
