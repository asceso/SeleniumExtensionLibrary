# SeleniumExtensionLibrary
SeleniumExtension based on 
Selenium 4.3
Chrome driver 103.0.5060.13400

## using library

```C#
SeleniumConfig config = new SeleniumConfig();
ISeleniumExecutor executor = new SeleniumExecutor();
executor.SetConfig(config);
```

all fields with comments
extension methods will work when add using namespace
```C#
using SeleniumExtensionLibrary;
```

## available methods

```C#
IWebDriver driver = executor.InitDriver()
string sessionId = executor.InitDriverAndSaveToSessionManager();
```
init driver or init driver and place to session manager

to work with driver need get IWebDriver element, `InitDriver()` method return this and `GetDriverBySessionId(string sessionId)` also return IWebDriver element
```C#
executor.GetDriverBySessionId("<SESSION_ID>")
```

to close driver and exit chromedriver.exe process use `CloseDriver(IWebDriver driver)` or `CloseDriverBySessionId(string sessionId)` methods
```C#
executor.CloseDriverBySessionId("<SESSION_ID>");
executor.CloseDriver(driver);
```

## extension methods
### methods work with IWebDriver object

```C#
driver.GoToUrl("https://google.com");
driver.Navigate().GoToUrl("https://google.com")
```
method work like standart method

```C#
driver.FindElementOrGetNull(By locator);
```
method work like standart `FindElement` method but return null if element not finded

```C#
driver.FindElementOrGetNullWithTimeout(By locator, int timeoutSeconds);
```
extension to previous method but element try find with used time, standart timeout can set in config
for example: `driver.FindElementOrGetNullWithTimeout(By.Class("someClass"), 5);`
elemnt try find in page for 5 seconds, if element not finded return null

### methods work with IWebElement object

```C#
element.FillField(string value, FillRule rule);
```
method replace standart `SendKeys()` but using rules

``` C#
FillRule.Fast //work like standart send keys
FillRule.Normal //between chars using Task.Delay(10 ms)
FillRule.Long //between chars using Task.Delay(50 ms)
FillRule.Random //between chars using Task.Delay(between 10 - 50 ms)
```

```C#
element.ClearAndFillField(string value, FillRule rule);
```
method like previous but before fill field use `element.Clear()` method
