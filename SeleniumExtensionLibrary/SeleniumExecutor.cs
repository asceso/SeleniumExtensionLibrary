using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeleniumExtensionLibrary
{
    public class SeleniumExecutor : ISeleniumExecutor
    {
        private readonly Dictionary<string, IWebDriver> sessionManager = new Dictionary<string, IWebDriver>();
        private SeleniumConfig _config;

        public void SetConfig(SeleniumConfig config)
        {
            _config = config;
            SeleniumExtension.ElementFindTimeout = config.ElementFindTimeout;
        }

        public string GenerateSessionId()
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < _config.SessionIdLength; i++)
            {
                builder.Append(random.Next(0, 10));
            }
            return builder.ToString();
        }

        public IWebDriver InitDriver(string[] extensionsPathes, params Tuple<string, object>[] profilePreferences)
        {
            ChromeDriverService driverService = ChromeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;

            ChromeOptions options = new ChromeOptions();
            if (_config.UsePortableBinary)
            {
                options.BinaryLocation = _config.PortableBinaryPath;
            }

            if (_config.Proxy != null)
            {
                string proxyRowWithLogin = $"{_config.Proxy.Login}:{_config.Proxy.Password}@{_config.Proxy.Hostname}:{_config.Proxy.Port}";
                string proxyRowWithoutLogin = $"{_config.Proxy.Hostname}:{_config.Proxy.Port}";

                Proxy proxy = new Proxy();
                switch (_config.Proxy.ProxyType)
                {
                    case ProxyType.Socks:
                        {
                            proxy.SocksProxy = $"socks5://{proxyRowWithLogin}";
                        }
                        break;

                    case ProxyType.Http:
                        {
                            proxy.HttpProxy = $"http://{proxyRowWithLogin}";
                        }
                        break;

                    case ProxyType.Ssl:
                        {
                            proxy.SslProxy = $"{proxyRowWithLogin}";
                        }
                        break;
                }
                options.Proxy = proxy;
            }

            options.AddArgument("ignore-certificate-errors");
            options.AddArgument("--lang=en");
            options.AddArgument("--disable-notifications");
            options.AddArgument("--disable-loging");
            options.AddArgument("--disable-blink-features=AutomationControlled");
            if (extensionsPathes != null && extensionsPathes.Length != 0)
            {
                foreach (var path in extensionsPathes)
                {
                    options.AddExtension(path);
                }
            }
            if (profilePreferences != null && profilePreferences.Length != 0)
            {
                foreach (var preference in profilePreferences)
                {
                    options.AddUserProfilePreference(preference.Item1, preference.Item2);
                }
            }

            IWebDriver driver = new ChromeDriver(driverService, options);
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(_config.PageLoadingTimeout);
            return driver;
        }

        public void CloseDriver(IWebDriver driver)
        {
            if (driver != null)
            {
                driver.Quit();
            }
        }

        public string InitDriverAndSaveToSessionManager(string[] extensionsPathes, params Tuple<string, object>[] profilePreferences)
        {
            IWebDriver driver = InitDriver(extensionsPathes, profilePreferences);
        GenerateId:
            string sessionId = GenerateSessionId();
            if (sessionManager.ContainsKey(sessionId))
            {
                goto GenerateId;
            }

            sessionManager.Add(sessionId, driver);
            return sessionId;
        }

        public bool CloseDriverBySessionId(string sessionId)
        {
            var driver = GetDriverBySessionId(sessionId);
            if (driver == null)
            {
                return false;
            }

            CloseDriver(driver);
            sessionManager.Remove(sessionId);
            return true;
        }

        public IWebDriver GetDriverBySessionId(string sessionId) => sessionManager.ContainsKey(sessionId) ? sessionManager[sessionId] : null;
    }
}