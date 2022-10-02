using Automation.Selenium.SauceDemo.Helpers.Extensions;
using Automation.Selenium.SauceDemo.Helpers.Libraries.Elements;


namespace Automation.Selenium.SauceDemo.Helpers.Scenarios.Setup.Login
{
  public static class LoginHelper
  {
    public static void PerformLogin(string username, string password)
    {
    WebDriverExtensions.ResetElementText(LoginPage.TxtUsername, username);
    WebDriverExtensions.ResetElementText(LoginPage.TxtPassword, password);
    WebDriverExtensions.Driver.FindElement(LoginPage.BtnLogin).Click();
    }
  }
}
