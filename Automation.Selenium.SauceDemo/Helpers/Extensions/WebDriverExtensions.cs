using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Safari;
using OpenQA.Selenium.Support.UI;

namespace Automation.Selenium.SauceDemo.Helpers.Extensions
{
  public static class WebDriverExtensions
  {
    public static IWebDriver Driver = null!;

    public static void GetDriver(this string testBrowser)
    {
      switch (testBrowser)
      {
        case "MozillaFireFox":
        {
          Driver = new FirefoxDriver();
          break;
        }
        case "GoogleChrome":
        {
          var options = new ChromeOptions();
          options.PageLoadStrategy = PageLoadStrategy.None;
          Driver = new ChromeDriver(options);
          break;
        }        
        case "GoogleChromeHeadless":
        {
          var options = new ChromeOptions();
          options.PageLoadStrategy = PageLoadStrategy.None;
          options.AddArguments("--headless");
          Driver = new ChromeDriver(options);
          break;
        }
        case "InternetExplorer":
        {
          Driver = new InternetExplorerDriver();
          break;
        }
        case "Safari":
        {
          Driver = new SafariDriver();
          break;
        }
        default:
        {
          Driver = new ChromeDriver();
          break;
        }
      }
    }

    public static void WaitForItemDisplayed(By item, int waitSeconds = 60)
    {
      var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(waitSeconds));
      wait.Until(d => d.FindElement(item).Displayed.Equals(true));
    }

    public static void ResetElementText(By element, string newTextValue)
    {
      Driver.FindElement(element).Clear();
      Driver.FindElement(element).SendKeys(newTextValue);
    }
  }
}