using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace ConsoleApp1.Pages
{
    class CartPage
    {
        private IWebDriver _driver;

        private WebDriverWait wait;

        public CartPage(IWebDriver driver)
        {
            _driver = driver;

            wait = new WebDriverWait(_driver, new TimeSpan(0, 0, 30));
        }

        private IWebElement _cartPageIdentification => wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//h2[contains(text(),'Total')]")));


        public void CheckCartPageIsDisplayed()
        {
            Assert.IsTrue(_cartPageIdentification.Displayed);
        }
        public string GetProductNameFromCartPage(int productPositionFromPage)
        {
            IWebElement Name = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//tbody[@id='tbodyid']//tr[" + productPositionFromPage + "]//td[2]")));

            var productName = Name.Text;

            return productName;
        }
        public int GetProductPriceFromCartPage(int productPositionFromPage)
        {
            IWebElement Price = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//tbody[@id='tbodyid']//tr[" + productPositionFromPage + "]//td[3]")));

            var productPrice = int.Parse(Regex.Match(Price.Text, @"\d+").Value);
            
            return productPrice;
        }
    }
}
