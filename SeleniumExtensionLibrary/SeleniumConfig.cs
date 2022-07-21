namespace SeleniumExtensionLibrary
{
    public class SeleniumConfig
    {
        /// <summary>
        /// Length for generate ID
        /// </summary>
        public int SessionIdLength { get; set; }

        /// <summary>
        /// Path to portable chrome binary as default set to 'C:\\ChromePortable/bin/chrome.exe'
        /// </summary>
        public string PortableBinaryPath { get; set; }

        /// <summary>
        /// If this param set to false driver will using installed chrome
        /// </summary>
        public bool UsePortableBinary { get; set; }

        /// <summary>
        /// Page timeout param, defaul set to 5 sec
        /// </summary>
        public int PageLoadingTimeout { get; set; }

        /// <summary>
        /// Element find timeout param, defaul set to 3 sec
        /// </summary>
        public int ElementFindTimeout { get; set; }

        /// <summary>
        /// Fill field by extension rule, more info in enum
        /// </summary>
        public FillRule FillFieldRule { get; set; }

        /// <summary>
        /// Proxy setup, defaul object is null, whet it null not using proxy
        /// </summary>
        public ProxyConfig Proxy { get; set; }

        public SeleniumConfig()
        {
            SessionIdLength = 10;
            PortableBinaryPath = "C:\\ChromePortable/bin/chrome.exe";
            UsePortableBinary = true;
            PageLoadingTimeout = 5;
            ElementFindTimeout = 3;
            FillFieldRule = FillRule.Fast;
            Proxy = null;
        }
    }

    public class ProxyConfig
    {
        public string Hostname { get; set; }
        public string Port { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public ProxyType ProxyType { get; set; }
    }

    public enum ProxyType
    {
        Socks,
        Http,
        Ssl
    }

    public enum FillRule
    {
        /// <summary>
        /// Fast fill field, without pause
        /// </summary>
        Fast,

        /// <summary>
        /// Normal fill field, pause 10 ms
        /// </summary>
        Normal,

        /// <summary>
        /// Long fill field, pasue 50 ms
        /// </summary>
        Long,

        /// <summary>
        /// Random fill field, pause random between 10 - 50 ms
        /// </summary>
        Random
    }
}