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
          // Keeps the page's console output, so a failure can show any script errors.
          options.SetLoggingPreference(LogType.Browser, LogLevel.All);
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

    // Seconds to wait for the URL to leave the starting page before treating the first click as
    // dropped. SauceDemo changes page client-side, so a click that registered moves the URL
    // almost at once; two seconds is generous for that and short enough to retry promptly.
    private const int DroppedClickWaitSeconds = 2;

    // Clicks a control that navigates, then waits for the next page's URL, so nothing reads
    // the next page while the old one is still on screen.
    //
    // SauceDemo sometimes drops a click that lands right after a page has rendered: the button
    // is visible, enabled and on top, yet the click never reaches the page and nothing happens.
    // The Playwright port of this suite sees the same thing in WebKit, where a click about 40 ms
    // after render was dropped in 2 of 15 runs and a later click was not. So if the URL has not
    // left the starting page within a short wait, the first click is treated as dropped and
    // clicked once more, then the full wait applies. A click that navigates somewhere
    // unexpected is not retried, and fails.
    public static void ClickAndWaitForUrl(By element, string urlFragment, int waitSeconds = 10)
    {
      var startUrl = Driver.Url;
      InstallClickRecorder();
      Click(element);

      try
      {
        WaitForUrl(urlFragment, Math.Min(DroppedClickWaitSeconds, waitSeconds));
        return;
      }
      catch (WebDriverTimeoutException) when (Driver.Url == startUrl)
      {
        TestContext.Progress.WriteLine(
          $"Click on {element} did not leave {startUrl} within {DroppedClickWaitSeconds}s; treating it as dropped and clicking once more.");
      }
      catch (WebDriverTimeoutException)
      {
        // The URL moved, but not yet to the expected page: give it the rest of the full wait.
        try
        {
          WaitForUrl(urlFragment, waitSeconds);
          return;
        }
        catch (WebDriverTimeoutException)
        {
          WriteNavigationDiagnostics(element);
          throw;
        }
      }

      try
      {
        Click(element);
        WaitForUrl(urlFragment, waitSeconds);
      }
      catch (WebDriverTimeoutException)
      {
        WriteNavigationDiagnostics(element);
        throw;
      }
    }

    // Records, inside the page, every click event and any script error, so that when a click
    // does not navigate the test output shows whether the click reached the page at all.
    // A same-page (client-side) navigation keeps the window, so the record survives it.
    private static void InstallClickRecorder()
    {
      try
      {
        ((IJavaScriptExecutor)Driver).ExecuteScript(
          "if (window.__diag) return;" +
          "const d = window.__diag = { errors: [], clicks: [] };" +
          "window.addEventListener('error', e => d.errors.push('error: ' + e.message + ' @ ' + e.filename + ':' + e.lineno));" +
          "window.addEventListener('unhandledrejection', e => d.errors.push('unhandled rejection: ' + e.reason));" +
          "document.addEventListener('click', e => d.clicks.push({ at: location.pathname, target: (e.target.outerHTML || '').slice(0, 160), trusted: e.isTrusted }), true);" +
          "window.addEventListener('click', e => d.clicks.push({ at: location.pathname, reachedWindow: true, defaultPrevented: e.defaultPrevented }));");
      }
      catch (WebDriverException) { }
    }

    private static void WriteNavigationDiagnostics(By element)
    {
      var output = TestContext.Out;
      output.WriteLine($"--- Navigation diagnostics for click on {element} ---");
      Try(output, "URL", () => Driver.Url);
      Try(output, "Page state", () => ((IJavaScriptExecutor)Driver).ExecuteScript(
        "return JSON.stringify({ readyState: document.readyState, historyLength: history.length, " +
        "title: (document.querySelector('.title') || {}).textContent, " +
        "error: (document.querySelector('[data-test=error]') || {}).textContent });"));
      Try(output, "Element", () => ((IJavaScriptExecutor)Driver).ExecuteScript(
        "const e = arguments[0]; const r = e.getBoundingClientRect();" +
        "const t = document.elementFromPoint(r.left + r.width / 2, r.top + r.height / 2);" +
        "return JSON.stringify({ html: e.outerHTML.slice(0, 200), rect: [r.left, r.top, r.width, r.height], " +
        "topmost: t ? t.outerHTML.slice(0, 200) : null });", Driver.FindElement(element)));
      Try(output, "Recorded clicks and errors", () => ((IJavaScriptExecutor)Driver).ExecuteScript(
        "return JSON.stringify(window.__diag || 'recorder lost: the page was reloaded');"));
      Try(output, "Browser console", () => string.Join(Environment.NewLine,
        Driver.Manage().Logs.GetLog(LogType.Browser).Select(l => $"  {l.Timestamp:HH:mm:ss.fff} {l.Level} {l.Message}")));

      try
      {
        var directory = Path.Combine(TestContext.CurrentContext.WorkDirectory, "TestResults", "diagnostics");
        Directory.CreateDirectory(directory);
        var name = string.Concat(TestContext.CurrentContext.Test.Name.Where(char.IsLetterOrDigit));
        var screenshot = Path.Combine(directory, name + ".png");
        ((ITakesScreenshot)Driver).GetScreenshot().SaveAsFile(screenshot);
        File.WriteAllText(Path.Combine(directory, name + ".html"), Driver.PageSource);
        output.WriteLine($"Screenshot and page source saved under {directory}");
      }
      catch (Exception e) { output.WriteLine($"Could not save screenshot or page source: {e.Message}"); }
      output.WriteLine("--- end of diagnostics ---");
    }

    private static void Try(TextWriter output, string label, Func<object?> read)
    {
      try { output.WriteLine($"{label}: {read()}"); }
      catch (Exception e) { output.WriteLine($"{label}: (unavailable: {e.Message})"); }
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
