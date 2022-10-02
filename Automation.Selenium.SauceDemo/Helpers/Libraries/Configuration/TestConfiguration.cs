using OpenQA.Selenium;

namespace Automation.Selenium.SauceDemo.Helpers.Libraries.Configuration
{
  internal class TestConfiguration
  {
    public static string SauceDemoUrl => "https://www.saucedemo.com";
    public static string SupportedBrowser => "GoogleChromeHeadless";

    public static string validUserName => "standard_user";
    public static string validPassword => "secret_sauce";

    public static By AddBackPackToCartButton => By.Name("add-to-cart-sauce-labs-backpack");

    public static string ValidFirstName => "Rick";
    public static string ValidLastName => "Grimes";
    public static string ValidPostalCode => "2293";
  }
}