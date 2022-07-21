using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumExtensionLibrary;
using System;
using System.Diagnostics;

namespace TestNetCore
{
    public class Tests
    {
        private ISeleniumExecutor executor;
        private SeleniumConfig config;

        [SetUp]
        public void Setup()
        {
            executor = new SeleniumExecutor();
            config = new SeleniumConfig();
            executor.SetConfig(config);
        }

        [Test]
        public void TestGenerateSessionId()
        {
            for (int i = 0; i < 10; i++)
            {
                string id = executor.GenerateSessionId();
                TestContext.WriteLine(id);
                Assert.IsTrue(id.Length == 10);
            }
        }

        [Test]
        public void TestPageLoadTimeout()
        {
            config.PageLoadingTimeout = 0;
            executor.SetConfig(config);

            IWebDriver driver = executor.InitDriver();
            try
            {
                driver.Navigate().GoToUrl("https://youtube.com");
                throw new System.Exception("Page loaded. Timeout set to 0 sec");
            }
            catch (WebDriverTimeoutException ex)
            {
                Assert.IsNotNull(ex);
                TestContext.WriteLine("Page not loaded. Timeout set to 0 sec");
            }
            finally
            {
                executor.CloseDriver(driver);
            }

            config.PageLoadingTimeout = 5;
            executor.SetConfig(config);

            driver = executor.InitDriver();
            try
            {
                driver.Navigate().GoToUrl("https://youtube.com");
                TestContext.WriteLine("Page loaded. Timeout set to 5 sec");
            }
            catch (WebDriverTimeoutException ex)
            {
                Assert.NotNull(ex, "Page not loaded. Timeout set to 5 sec");
            }
            finally
            {
                executor.CloseDriver(driver);
            }
        }

        [Test]
        public void TestFindElementOrGetNull()
        {
            IWebDriver driver = executor.InitDriver();
            driver.GoToUrl("https://www.selenium.dev/documentation/overview/");

            TestContext.WriteLine("Test case 1: Try find null element with timeout");
            Stopwatch watch = Stopwatch.StartNew();
            IWebElement nullElement = driver.FindElementOrGetNullWithTimeout(By.XPath("//*[text()='ABCDEFG']"));
            Assert.IsNull(nullElement, "Text ABCDEFG founded on page? WTF.");
            TestContext.WriteLine("Text ABCDEFG not founded on page. OK!");
            watch.Stop();
            TestContext.WriteLine($"Time ellapsed after try found text: {Math.Round(watch.Elapsed.TotalSeconds, 2)} seconds{Environment.NewLine}");

            TestContext.WriteLine("Test case 2: Try find null element without timeout");
            watch = Stopwatch.StartNew();
            nullElement = driver.FindElementOrGetNull(By.XPath("//*[text()='ABCDEFG']"));
            Assert.IsNull(nullElement, "Text ABCDEFG founded on page? WTF.");
            TestContext.WriteLine("Text ABCDEFG not founded on page. OK!");
            watch.Stop();
            TestContext.WriteLine($"Time ellapsed after try found text: {Math.Round(watch.Elapsed.TotalSeconds, 2)} seconds{Environment.NewLine}");

            TestContext.WriteLine("Test case 3: Try find exist element without timeout");
            watch = Stopwatch.StartNew();
            IWebElement overviewText = driver.FindElementOrGetNull(By.XPath("//*[text()='Selenium overview']"));
            Assert.IsTrue(overviewText.Text == "Selenium overview", "Text with text not found");
            TestContext.WriteLine("Text 'Selenium overview' founded on page. OK!");
            watch.Stop();
            TestContext.WriteLine($"Time ellapsed after try found text: {Math.Round(watch.Elapsed.TotalSeconds, 2)} seconds{Environment.NewLine}");

            executor.CloseDriver(driver);
        }

        [Test]
        public void TestSessionManager()
        {
            string sessionA = executor.InitDriverAndSaveToSessionManager();
            string sessionB = executor.InitDriverAndSaveToSessionManager();

            Assert.IsNotNull(executor.GetDriverBySessionId(sessionA), "Not found driver by session A");
            Assert.IsNotNull(executor.GetDriverBySessionId(sessionB), "Not found driver by session B");

            TestContext.WriteLine("Test case 1: get driver session A and found overview element");
            IWebDriver driverSessionA = executor.GetDriverBySessionId(sessionA);
            TestContext.WriteLine($"Driver with session A id({sessionA}) founded. OK!");
            driverSessionA.GoToUrl("https://www.selenium.dev/documentation/overview/");
            IWebElement overviewText = driverSessionA.FindElementOrGetNull(By.XPath("//*[text()='Selenium overview']"));
            TestContext.WriteLine($"Text 'Selenium overview' founded on page in driver session A. OK!{Environment.NewLine}");

            TestContext.WriteLine("Test case 2: close session A and get try get session driver");
            executor.CloseDriverBySessionId(sessionA);
            Assert.IsNull(executor.GetDriverBySessionId(sessionA), "Driver from session A not closed");
            TestContext.WriteLine($"Driver from session A closed. OK!{Environment.NewLine}");

            TestContext.WriteLine("Test case 3: close session B and get try get session driver");
            executor.CloseDriverBySessionId(sessionB);
            Assert.IsNull(executor.GetDriverBySessionId(sessionB), "Driver from session B not closed");
            TestContext.WriteLine($"Driver from session B closed. OK!{Environment.NewLine}");
        }
    }
}