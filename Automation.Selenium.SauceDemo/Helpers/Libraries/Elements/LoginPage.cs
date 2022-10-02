using OpenQA.Selenium;

namespace Automation.Selenium.SauceDemo.Helpers.Libraries.Elements
{
  internal class LoginPage
  {
    public static readonly By TxtUsername = By.Id("user-name");
    public static readonly By TxtPassword = By.Id("password");
    public static readonly By BtnLogin = By.Id("login-button");

    public static string IncorrectUsername => "stndrd-user";
    public static string CorrectUsername => "standard_user";
    public static string CorrectPassword => "secret_sauce";

    public static readonly By LblErrorMessageElement =
      By.XPath("/html/body/div[1]/div/div[2]/div[1]/div[1]/div/form/div[3]/h3");

    public static string LblErrorMessageText =>
      "Epic sadface: Username and password do not match any user in this service";
  }
}