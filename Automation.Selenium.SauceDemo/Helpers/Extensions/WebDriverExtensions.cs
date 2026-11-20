using NUnit.Framework;
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
          options.PageLoadStrategy = PageLoadStrategy.Normal;
          Driver = new ChromeDriver(options);
          break;
        }
        case "GoogleChromeHeadless":
        {
          var options = new ChromeOptions();
          options.PageLoadStrategy = PageLoadStrategy.Normal;
          options.AddArguments("--headless", "--window-size=1920,1080");
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

      // SauceDemo renders client-side, so elements appear a moment after navigation.
      Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
    }

    public static void WaitForItemDisplayed(By item, int waitSeconds = 60)
    {
      var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(waitSeconds));
      wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));
      wait.Until(d => d.FindElement(item).Displayed.Equals(true));
    }

    // Waits until the element can really take a click: the document has finished loading,
    // the element is displayed and enabled, and it is the topmost element at its own centre,
    // so nothing (an outgoing page, a re-rendering header) is in the way of the click.
    public static IWebElement WaitUntilClickable(By element, int waitSeconds = 10)
    {
      var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(waitSeconds))
      {
        Message = $"{element} did not become clickable within {waitSeconds}s"
      };
      wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));
      return wait.Until(d =>
      {
        var js = (IJavaScriptExecutor)d;
        if (!"complete".Equals(js.ExecuteScript("return document.readyState;"))) return null;

        var candidate = d.FindElement(element);
        if (!candidate.Displayed || !candidate.Enabled) return null;

        var onTop = js.ExecuteScript(
          "const e = arguments[0];" +
          "e.scrollIntoView({block: 'center', inline: 'center'});" +
          "const r = e.getBoundingClientRect();" +
          "const t = document.elementFromPoint(r.left + r.width / 2, r.top + r.height / 2);" +
          "return t !== null && (t === e || e.contains(t));", candidate);

        return true.Equals(onTop) ? candidate : null;
      });
    }

    public static void Click(By element) => WaitUntilClickable(element).Click();

    public static void WaitForUrl(string urlFragment, int waitSeconds = 10)
    {
      var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(waitSeconds));
      wait.Until(d =>
      {
        if (d.Url.Contains(urlFragment)) return true;
        wait.Message = $"URL did not contain '{urlFragment}' within {waitSeconds}s (still on {d.Url})";
        return false;
      });
    }

    // Clicks a control that navigates, then waits for the next page's URL, so nothing reads
    // the next page while the old one is still on screen.
    // If the browser has not left the starting URL at all, the click never registered, so it
    // is retried once. A click that navigates somewhere unexpected is not retried, and fails.
    public static void ClickAndWaitForUrl(By element, string urlFragment, int waitSeconds = 10)
    {
      var startUrl = Driver.Url;
      Click(element);

      try
      {
        WaitForUrl(urlFragment, waitSeconds);
        return;
      }
      catch (WebDriverTimeoutException) when (Driver.Url == startUrl)
      {
        TestContext.Progress.WriteLine($"Click on {element} did not leave {startUrl}; clicking once more.");
      }

      Click(element);
      WaitForUrl(urlFragment, waitSeconds);
    }

    // After a click, the old page's heading can still be on screen for a moment.
    // Give the expected text a few seconds to appear, then return whatever is there,
    // so a wrong page still fails the assertion with the real text.
    public static string ReadTextOnceSettled(By element, string expected, int waitSeconds = 10)
    {
      var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(waitSeconds));
      wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));
      try { wait.Until(d => d.FindElement(element).Text == expected); }
      catch (WebDriverTimeoutException) { }
      return Driver.FindElement(element).Text;
    }

    // Types into an input, then waits until the field really holds the value, so a submit
    // that follows cannot go out with an empty or half-typed field.
    public static void ResetElementText(By element, string newTextValue)
    {
      var field = WaitUntilClickable(element);
      field.Clear();
      field.SendKeys(newTextValue);

      var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(5))
      {
        Message = $"{element} did not hold '{newTextValue}' after typing"
      };
      wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
      wait.Until(d => d.FindElement(element).GetDomProperty("value") == newTextValue);
    }
  }
}
