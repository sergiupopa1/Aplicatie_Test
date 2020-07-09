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

namespace ConsoleApp1
{
    [Binding, Scope(Feature = "Register")]
    public class Register
    {
        private int budget;

        int meanValue;

        string initialImage; 
        
        private IWebDriver driver;
        
        [Given(@"I am on the homepage")]
        public void GivenIAmOnTheHomepage()
        {
            driver = new ChromeDriver();

            //Modificare
            driver.Manage().Window.Maximize();

            driver.Navigate().GoToUrl("https://www.demoblaze.com"); // e indicat sa declari ca parametru URL-ul si apoi sa-l folosesti unde ai nevoie

            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 30));

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("nava")));
        }
        
        [Given(@"I click on Sign Up button")]
        public void GivenIClickOnSignUpButton()
        {
            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 30));

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("signin2"))).Click();
        }
        
        [When(@"I fill in required data")]
        public void WhenIFillInRequiredData()
        {
            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 30));

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("sign-username"))).SendKeys("xyz1234567890");// daca se foloseste acelasi username
                                                                                                                                // testul automatizat nu are valoare pt ca la a 2 a rulare acesta da fail
                                                                                                                                // pentru ca userul e unul deja existent
                                                                                                                                // de modificat facand register cu un user nou la fiecare rulare. Foloseste generarea de user random

            driver.FindElement(By.Id("sign-password")).SendKeys("12345678");

            driver.FindElement(By.CssSelector("#signInModal > div > div > div.modal-footer > button.btn.btn-primary")).Click();
        }
        
        [Then(@"I get registered")]
        public void ThenIGetRegistered()
        {
            Thread.Sleep(500);

            driver.SwitchTo().Alert().Accept(); // se poate adauga un pas de verificare la register in care sa verifici ca merge sa faci login cu user-ul nou inregistrat/creat
        }

        /***************************************** new scenario ****************************************/

        [When(@"I click on the login button")]
        public void WhenIClickOnTheLoginButton()
        {
            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 30));

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("login2"))).Click();
        }

        [When(@"I enter my credentials")]
        public void WhenIEnterMyCredentials()
        {
            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 30));

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("loginusername"))).SendKeys("xyz1234567890");

            driver.FindElement(By.Id("loginpassword")).SendKeys("12345678");

            driver.FindElement(By.CssSelector("#logInModal > div > div > div.modal-footer > button.btn.btn-primary")).Click();
        }

        [Then(@"I get logged in")]
        public void ThenIGetLoggedIn()
        {
            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 30));

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("nameofuser")));

            string logInText;

            logInText = driver.FindElement(By.Id("nameofuser")).Text;

            Assert.AreEqual(logInText, "Welcome xyz1234567890");
        }

        /***************************************** new scenario ****************************************/

        public string SaveActiveElement()
        {
            var activeElement = driver.FindElement(By.XPath("//div[@class='carousel-item active']/img"));

            initialImage = activeElement.GetAttribute("alt");
    
            return initialImage;
        }

        [When(@"I click on the Previous button from Image Slider")]
        public void WhenIClickOnThePreviousButtonFromImageSlider()
        {
            SaveActiveElement();

            driver.FindElement(By.CssSelector("#carouselExampleIndicators > a.carousel-control-prev > span.carousel-control-prev-icon")).Click();

            Thread.Sleep(1000);

        }

        [Then(@"I see a different product")]
        public void ThenISeeADifferentProduct()
        {
            string actualImage;

            var activeElement = driver.FindElement(By.XPath("//div[@class='carousel-item active']/img"));

            actualImage = activeElement.GetAttribute("alt");

            Assert.AreNotEqual(actualImage, initialImage);// verificarea se putea face luand sursa, imaginea incarcata ex: src="nexus1", "samsnug1"
        }

        [When(@"I click on the Next button from Image Slider")]
        public void WhenIClickOnTheNextButtonFromImageSlider()
        {
            SaveActiveElement();

            driver.FindElement(By.CssSelector("#carouselExampleIndicators > a.carousel-control-next > span.carousel-control-next-icon")).Click();

            Thread.Sleep(1000);
        }

        /***************************************** new scenario ****************************************/

        [Given(@"I am logged in")]
        public void GivenIAmLoggedIn()
        {
            driver = new ChromeDriver();


            // Modificare
            driver.Manage().Window.Maximize();


            driver.Navigate().GoToUrl("https://www.demoblaze.com");

            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 30));

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("nava")));
                        
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("login2"))).Click();

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("loginusername"))).SendKeys("xyz1234567890");

            driver.FindElement(By.Id("loginpassword")).SendKeys("12345678");

            driver.FindElement(By.CssSelector("#logInModal > div > div > div.modal-footer > button.btn.btn-primary")).Click();

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("nameofuser")));

            string logInText;

            logInText = driver.FindElement(By.Id("nameofuser")).Text;

            Assert.AreEqual(logInText, "Welcome xyz1234567890");
        }

        [Given(@"I have a budget of (.*)\$")]
        public void GivenIHaveABudgetOf(int p0)
        {
            budget = p0;
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

            if (budget - phoneValue1 > 0)
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

            } while (phoneValue1 + phoneValue2 > budget);

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

            Assert.LessOrEqual(totalCart, budget);

            for (j=0;j<p0;j++)
            {
                wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//a[contains(text(),'Delete')]")));

                (driver.FindElement(By.XPath("//*[contains(@onclick,'deleteItem')]"))).Click();

                Thread.Sleep(1000);
            }

            Thread.Sleep(2000);
        }

        /***************************************** new scenario ****************************************/

        [When(@"I filter by (.*)")]
        public void WhenIFilterBy(string p0)
        {
            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 30));
            
                switch (p0)
                {
                    case "Phones":
                        (wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//*[contains(@onclick,'phone')]")))).Click();
                        break;
                    case "\"Phones\"":
                        (wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//*[contains(@onclick,'phone')]")))).Click();
                        break;
                    case "\"Laptops\"":
                        (wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//*[contains(@onclick,'notebook')]")))).Click();
                        break;
                    case "\"Monitors\"":
                        (wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//*[contains(@onclick,'monitor')]")))).Click();
                        break;
                    default:
                        Console.WriteLine("Error: No category found!");
                        break;
                }
            
            Thread.Sleep(2000);
        }

        [Then(@"I can see in the test output the mean value of each product")]
        public void ThenICanSeeInTheTestOutputTheMeanValueOfEachProduct()
        {
            int noOfProducts, i; // e indicat sa se foloseasca declarari separate pentru variabile, e mai usor de citit, urmarit
            
            noOfProducts = driver.FindElements(By.XPath("//*[contains(@class, 'col-lg-4 col-md-6 mb-4')]")).Count;

            for(i=1;i<=noOfProducts;i++)
            {
                meanValue += int.Parse(Regex.Match((driver.FindElement(By.XPath("//*[@id=\"tbodyid\"]/div[" + i + "]/div/div/h5"))).Text, @"\d+").Value);
            }

            Console.WriteLine(meanValue / noOfProducts);//nu apare valoarea in output, foloseste Debug.WriteLine();
        }

        [AfterScenario]
        public void ClosePage()
        {
            driver.Quit();
        }
    }
}
