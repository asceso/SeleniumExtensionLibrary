using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading.Tasks;

namespace SeleniumExtensionLibrary
{
    public static class SeleniumExtension
    {
        public static int ElementFindTimeout;

        public static void GoToUrl(this IWebDriver driver, string url) => driver.Navigate().GoToUrl(url);

        public static IWebElement FindElementOrGetNull(this IWebDriver driver, By by)
        {
            try
            {
                return driver.FindElement(by);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static IWebElement FindElementOrGetNullWithTimeout(this IWebDriver driver, By locator, int customTimeout = -1)
        {
            try
            {
                WebDriverWait waiter = new WebDriverWait(driver, TimeSpan.FromSeconds(customTimeout == -1 ? ElementFindTimeout : customTimeout));
                waiter.Until(w => w.FindElement(locator).Displayed);
                return FindElementOrGetNull(driver, locator);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static void ClearAndFillField(this IWebElement element, string value, FillRule fillRule = FillRule.Fast)
        {
            element.Clear();
            element.FillField(value, fillRule);
        }

        public static void FillField(this IWebElement element, string value, FillRule fillRule = FillRule.Fast)
        {
            switch (fillRule)
            {
                case FillRule.Fast:
                    element.SendKeys(value);
                    break;

                case FillRule.Normal:
                    {
                        foreach (char ch in value)
                        {
                            Task.Delay(10).Wait();
                            element.SendKeys(ch.ToString());
                        }
                    }
                    break;

                case FillRule.Long:
                    {
                        foreach (char ch in value)
                        {
                            Task.Delay(50).Wait();
                            element.SendKeys(ch.ToString());
                        }
                    }
                    break;

                case FillRule.Random:
                    {
                        Random random = new Random();
                        foreach (char ch in value)
                        {
                            Task.Delay(random.Next(10, 51)).Wait();
                            element.SendKeys(ch.ToString());
                        }
                    }
                    break;

                default:
                    throw new ArgumentException("Invalid argument, try with other rules");
            }
        }
    }
}