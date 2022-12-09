using OpenQA.Selenium;
using System;

namespace SeleniumExtensionLibrary
{
    public interface ISeleniumExecutor
    {
        /// <summary>
        /// Method set selenium config to extension
        /// </summary>
        /// <param name="config"></param>
        void SetConfig(SeleniumConfig config);

        /// <summary>
        /// Method generate session id by length from config
        /// </summary>
        /// <returns>sessionId, format = XXX...XXX where count X is length</returns>
        string GenerateSessionId();

        /// <summary>
        /// Just init driver
        /// </summary>
        /// <param name="extensionsPathes">pathes to needed extensions</param>
        /// <param name="profilePreferences">preferences to options</param>
        /// <param name="profile">directory for profile and profile name</param>
        /// <returns>return inited driver</returns>
        IWebDriver InitDriver(string[] extensionsPathes = null, Tuple<string, object>[] profilePreferences = null, Tuple<string, string> profile = null);

        /// <summary>
        /// Just close driver
        /// </summary>
        /// <param name="driver">inited driver object</param>
        void CloseDriver(IWebDriver driver);

        /// <summary>
        /// Init driver and save it to session manager
        /// </summary>
        /// <param name="extensionsPathes">pathes to needed extensions</param>
        /// <param name="profilePreferences">preferences to options</param>
        /// <param name="profile">directory for profile and profile name</param>
        /// <returns>SessionId with inited driver</returns>
        string InitDriverAndSaveToSessionManager(string[] extensionsPathes = null, Tuple<string, object>[] profilePreferences = null, Tuple<string, string> profile = null);

        /// <summary>
        /// Close driver from session manager by his sessionId
        /// </summary>
        /// <param name="sessionId">Driver session id</param>
        /// <returns>return true if driver closed success</returns>
        bool CloseDriverBySessionId(string sessionId);

        /// <summary>
        /// Get driver by his sessionId
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        IWebDriver GetDriverBySessionId(string sessionId);
    }
}