using Automation.Selenium.SauceDemo.Helpers.Extensions;
using Automation.Selenium.SauceDemo.Helpers.Libraries.Elements;


namespace Automation.Selenium.SauceDemo.Helpers.Scenarios.Setup.Login
{
  public static class LoginHelper
  {
    public static void PerformValidLogin(string validUsername, string validPassword)
    {
    WebDriverExtensions.ResetElementText(LoginPage.TxtUsername, validUsername);
    WebDriverExtensions.ResetElementText(LoginPage.TxtPassword, validPassword);
    WebDriverExtensions.Driver.FindElement(LoginPage.BtnLogin).Click();
    }
  }
}
